using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class LevelGenerator : MonoBehaviour
{
    public RoomPrefabLists RoomPrefabs;
    public RoomLayoutData Layout;
    public Transform LevelContainer;

    // A class to hold the planned, but not yet instantiated, room data.
    private class GhostRoom
    {
        public RoomLayoutNode LayoutNode;
        public GameObject Prefab;
        public Vector3 Position;
        public Bounds BoundingBox;
        public Dictionary<Door.DoorDirection, (string connectedRoomID, Vector3 doorLocalPosition)> OpenDoors = new(); // Direction -> (Connected RoomID, Door Local Position)
    }

    private Dictionary<string, RoomLayoutNode> _roomNodes;
    private Dictionary<string, List<string>> _connections;
    private HashSet<string> _visitedNodes;
    private Dictionary<string, Room> _spawnedRooms;
    private List<GhostRoom> _ghostPlan;

    void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        // This outer loop will restart the entire generation process if the inner 10 attempts fail.
        // This avoids stack overflows that could be caused by recursion.
        while (true)
        {
            if (Layout == null || Layout.Nodes.Count == 0)
            {
                Debug.LogError("Layout data is not set or is empty.");
                return;
            }

            // Keep trying to generate a level a few times in case of random overlaps
            const int maxAttempts = 10;
            for (int i = 0; i < maxAttempts; i++)
            {
                // Clear previous level if any
                foreach (Transform child in LevelContainer) Destroy(child.gameObject);

                _roomNodes = Layout.Nodes.ToDictionary(node => node.RoomID, node => node);
                _connections = new Dictionary<string, List<string>>();
                foreach (var connection in Layout.Connections)
                {
                    if (!_connections.ContainsKey(connection.StartRoomID)) _connections[connection.StartRoomID] = new List<string>();
                    _connections[connection.StartRoomID].Add(connection.EndRoomID);

                    if (!_connections.ContainsKey(connection.EndRoomID)) _connections[connection.EndRoomID] = new List<string>();
                    _connections[connection.EndRoomID].Add(connection.StartRoomID);
                }

                _visitedNodes = new HashSet<string>();
                _spawnedRooms = new Dictionary<string, Room>();
                _ghostPlan = new List<GhostRoom>();

                RoomLayoutNode startNode = Layout.Nodes.FirstOrDefault(n => n.Type == Room.RoomType.START) ?? Layout.Nodes[0];

                if (PlanLevel(startNode))
                {
                    PlaceLevel();

                    // Initialize all rooms after placement and door setup
                    foreach (var room in _spawnedRooms.Values)
                    {
                        room.InitializeRoom();
                    }
                    return; // Success, exit the while loop and the method.
                }

                Debug.LogWarning($"Attempt {i + 1} failed due to overlap. Retrying...");
            }

            Debug.LogWarning($"Failed to generate a valid level layout after {maxAttempts} attempts. Restarting generation process.");
        }
    }

    private bool PlanLevel(RoomLayoutNode startNode)
    {
        return PlanRoomRecursive(startNode, null, null);
    }

    private bool PlanRoomRecursive(RoomLayoutNode currentNode, GhostRoom parentGhost, Door.DoorDirection? parentConnectionDirection)
    {
        if (_visitedNodes.Contains(currentNode.RoomID))
        {
            return true;
        }

        _visitedNodes.Add(currentNode.RoomID);

        GameObject prefab = GetRandomPrefabForType(currentNode.Type);
        if (prefab == null)
        {
            Debug.LogError($"No prefabs found for room type: {currentNode.Type}");
            return false;
        }

        var ghostRoom = new GhostRoom { LayoutNode = currentNode, Prefab = prefab };

        if (parentGhost != null && parentConnectionDirection.HasValue)
        {
            Door.DoorDirection requiredDirection = GetOppositeDirection(parentConnectionDirection.Value);
            var prefabDoors = prefab.GetComponentsInChildren<Door>();
            var potentialDoors = prefabDoors.Where(d => d.Direction == requiredDirection).ToList();

            if (!potentialDoors.Any())
            {
                Debug.LogError($"Prefab '{prefab.name}' has no door for direction {requiredDirection} to connect to parent '{parentGhost.LayoutNode.RoomID}'.");
                return false;
            }

            Door newRoomDoor = potentialDoors[UnityEngine.Random.Range(0, potentialDoors.Count)];

            // The target for the new room's door is the parent's door position, offset by one unit in the connection direction.
            // This ensures the rooms' borders align correctly.
            Vector3 parentDoorWorldPosition = parentGhost.Position + parentGhost.OpenDoors[parentConnectionDirection.Value].doorLocalPosition;
            Vector3 doorTargetPosition = parentDoorWorldPosition + GetDirectionVector(parentConnectionDirection.Value);

            Vector3 newRoomDoorOffset = newRoomDoor.transform.localPosition;
            ghostRoom.Position = doorTargetPosition - newRoomDoorOffset;
            ghostRoom.OpenDoors[requiredDirection] = (parentGhost.LayoutNode.RoomID, newRoomDoor.transform.localPosition);
        }
        else
        {
            ghostRoom.Position = transform.position;
        }

        // Calculate world bounds and check for overlap
        // The Room's 'Size' property is pre-calculated from its tilemap in the editor.
        // This is more reliable than searching for a collider on the prefab.
        var roomComponent = prefab.GetComponent<Room>();
        Vector3 roomWorldSize = new Vector3(roomComponent.Size.x, roomComponent.Size.y, 1); // Use a Z-size of 1 for 3D bounds

        // Shrink the bounds slightly to prevent adjacent rooms from being flagged as intersecting.
        var collisionBounds = new Bounds(ghostRoom.Position + (roomWorldSize / 2f), roomWorldSize);
        collisionBounds.Expand(-0.1f); // A small negative expansion shrinks the bounds.
        ghostRoom.BoundingBox = collisionBounds;

        foreach (var existingGhost in _ghostPlan)
        {
            if (ghostRoom.BoundingBox.Intersects(existingGhost.BoundingBox))
            {
                // For this implementation, we fail the attempt and let the top-level loop retry.
                // A more advanced system could backtrack here.
                return false;
            }
        }

        _ghostPlan.Add(ghostRoom);

        if (_connections.TryGetValue(currentNode.RoomID, out var neighborIDs))
        {
            foreach (var neighborID in neighborIDs)
            {
                if (_visitedNodes.Contains(neighborID)) continue;

                RoomLayoutNode neighborNode = _roomNodes[neighborID];
                Door.DoorDirection? connectionDirection = GetConnectionDirection(currentNode, neighborNode);
                if (!connectionDirection.HasValue)
                {
                    Debug.LogError($"Could not determine connection direction between '{currentNode.RoomID}' and '{neighborID}'. Are they orthogonally adjacent in the editor?");
                    continue;
                }

                // Find a door on the current prefab to connect to the neighbor
                var prefabDoors = prefab.GetComponentsInChildren<Door>();
                var availableDoors = prefabDoors.Where(d => d.Direction == connectionDirection.Value && !ghostRoom.OpenDoors.Values.Any(v => v.doorLocalPosition == d.transform.localPosition)).ToList();

                if (!availableDoors.Any()) continue; // No available door in this direction

                var doorToNeighbor = availableDoors[UnityEngine.Random.Range(0, availableDoors.Count)];
                if (doorToNeighbor == null) continue; // No available door in this direction

                // Store which door we chose for this connection
                ghostRoom.OpenDoors[connectionDirection.Value] = (neighborID, doorToNeighbor.transform.localPosition);

                if (!PlanRoomRecursive(neighborNode, ghostRoom, connectionDirection.Value))
                {
                    return false; // Propagate failure up
                }
            }
        }
        return true;
    }

    private void PlaceLevel()
    {
        // Phase 2: Instantiate rooms from the plan
        foreach (var ghostRoom in _ghostPlan)
        {
            GameObject roomObject = Instantiate(ghostRoom.Prefab, ghostRoom.Position, Quaternion.identity, LevelContainer);
            Room newRoom = roomObject.GetComponent<Room>();
            newRoom.Doors.AddRange(roomObject.GetComponentsInChildren<Door>());
            _spawnedRooms[ghostRoom.LayoutNode.RoomID] = newRoom;
        }

        // Phase 2.5: Connect and close doors
        foreach (var ghostRoom in _ghostPlan)
        {
            Room room = _spawnedRooms[ghostRoom.LayoutNode.RoomID];
            var allDoors = new List<Door>(room.Doors);
            var doorsToKeepOpen = new HashSet<Door>();

            foreach (var connection in ghostRoom.OpenDoors)
            {
                var direction = connection.Key;
                var doorLocalPos = connection.Value.doorLocalPosition;

                // Find the specific door instance that matches the one chosen during planning
                var doorToOpen = allDoors.FirstOrDefault(d => d.Direction == direction && d.transform.localPosition == doorLocalPos);

                if (doorToOpen != null) doorsToKeepOpen.Add(doorToOpen);
            }

            var doorsToClose = allDoors.Except(doorsToKeepOpen).ToList();
            room.SetDoorsOpen(doorsToClose, false);
            room.SetDoorsOpen(doorsToKeepOpen.ToList(), true);
        }
    }

    private GameObject GetRandomPrefabForType(Room.RoomType type)
    {
        RoomPrefabs prefabs = RoomPrefabs.RoomPrefabs.FirstOrDefault(rp => rp.Type == type);
        if (prefabs == null || prefabs.Prefabs.Count == 0)
        {
            Debug.LogWarning($"No prefabs found for room type: {type}");
            return null;
        }

        return prefabs.Prefabs[UnityEngine.Random.Range(0, prefabs.Prefabs.Count)];
    }

    private Vector3 GetDirectionVector(Door.DoorDirection direction)
    {
        return direction switch
        {
            Door.DoorDirection.UP => Vector3.up,
            Door.DoorDirection.DOWN => Vector3.down,
            Door.DoorDirection.LEFT => Vector3.left,
            Door.DoorDirection.RIGHT => Vector3.right,
            _ => Vector3.zero,
        };
    }

    private Door.DoorDirection GetOppositeDirection(Door.DoorDirection direction)
    {
        return direction switch
        {
            Door.DoorDirection.UP => Door.DoorDirection.DOWN,
            Door.DoorDirection.DOWN => Door.DoorDirection.UP,
            Door.DoorDirection.LEFT => Door.DoorDirection.RIGHT,
            Door.DoorDirection.RIGHT => Door.DoorDirection.LEFT,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Not a valid DoorDirection: {direction}"),
        };
    }

    private Door.DoorDirection? GetConnectionDirection(RoomLayoutNode from, RoomLayoutNode to)
    {
        Vector2 diff = to.EditorPosition - from.EditorPosition;
        const float tolerance = 5f; 

        if (Mathf.Abs(diff.x) < tolerance) // Vertical alignment
        {
            if (diff.y < 0) return Door.DoorDirection.UP;
            if (diff.y > 0) return Door.DoorDirection.DOWN;
        }
        else if (Mathf.Abs(diff.y) < tolerance) // Horizontal alignment
        {
            if (diff.x > 0) return Door.DoorDirection.RIGHT;
            if (diff.x < 0) return Door.DoorDirection.LEFT;
        }

        return null; // Not orthogonally aligned
    }
}
