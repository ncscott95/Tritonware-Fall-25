using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    public RoomPrefabLists RoomPrefabs;
    public RoomLayoutData Layout;
    public Transform LevelContainer;

    private Dictionary<string, RoomLayoutNode> _roomNodes;
    private Dictionary<string, List<string>> _connections;
    private HashSet<string> _visitedNodes;
    private Dictionary<string, Room> _spawnedRooms;

    void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        if (Layout == null || Layout.Nodes.Count == 0)
        {
            Debug.LogError("Layout data is not set or is empty.");
            return;
        }

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

        RoomLayoutNode startNode = Layout.Nodes.FirstOrDefault(n => n.Type == Room.RoomType.START) ?? Layout.Nodes[0];

        GenerateRoomRecursive(startNode, null, null);
    }

    private void GenerateRoomRecursive(RoomLayoutNode currentNode, Room parentRoom, Door parentDoor)
    {
        if (_visitedNodes.Contains(currentNode.RoomID))
        {
            return;
        }

        _visitedNodes.Add(currentNode.RoomID);

        Room newRoom = InstantiateRoom(currentNode.Type);
        if (newRoom == null)
        {
            Debug.LogError($"Failed to instantiate room for type {currentNode.Type}. No prefabs assigned?");
            return;
        }

        _spawnedRooms[currentNode.RoomID] = newRoom;

        // Position the new room relative to its parent
        if (parentRoom != null && parentDoor != null)
        {
            Door.DoorDirection requiredDirection = GetOppositeDirection(parentDoor.Direction);
            Door newRoomDoor = newRoom.Doors.FirstOrDefault(d => d.Direction == requiredDirection);

            if (newRoomDoor == null)
            {
                Debug.LogError($"Room '{newRoom.name}' has no door for direction {requiredDirection} to connect to parent '{parentRoom.name}'.", newRoom.gameObject);
                Destroy(newRoom.gameObject); // Clean up failed room
                return;
            }

            // Calculate the position to align the doors
            Vector3 parentDoorWorldPos = parentDoor.transform.position;
            Vector3 newRoomDoorLocalPos = newRoomDoor.transform.localPosition;

            Vector3 targetWorldPos = parentDoorWorldPos - newRoomDoorLocalPos;
            newRoom.transform.localPosition = targetWorldPos - LevelContainer.position;
        }
        else
        {
            // This is the first room, place it at the origin
            newRoom.transform.localPosition = Vector3.zero;
        }

        // Continue DFS for all connections
        if (_connections.TryGetValue(currentNode.RoomID, out var neighborIDs))
        {
            List<Door> availableDoors = new List<Door>(newRoom.Doors);
            if (parentDoor != null)
            {
                Door.DoorDirection usedDirection = GetOppositeDirection(parentDoor.Direction);
                availableDoors.RemoveAll(d => d.Direction == usedDirection);
            }

            foreach (var neighborID in neighborIDs)
            {
                if (_visitedNodes.Contains(neighborID)) continue;

                RoomLayoutNode neighborNode = _roomNodes[neighborID];
                Door.DoorDirection? connectionDirection = GetConnectionDirection(currentNode, neighborNode);

                if (connectionDirection == null)
                {
                    Debug.LogError($"Could not determine connection direction between '{currentNode.RoomID}' and '{neighborID}'. Are they orthogonally adjacent in the editor?");
                    continue;
                }

                Door doorToNeighbor = availableDoors.FirstOrDefault(d => d.Direction == connectionDirection.Value);

                if (doorToNeighbor == null)
                {
                    Debug.LogWarning($"Room '{newRoom.name}' has no available door in direction {connectionDirection.Value} to connect to neighbor '{neighborNode.Type}'.", newRoom.gameObject);
                    continue;
                }

                availableDoors.Remove(doorToNeighbor);
                GenerateRoomRecursive(_roomNodes[neighborID], newRoom, doorToNeighbor);
            }
        }
    }

    private Room InstantiateRoom(Room.RoomType type)
    {
        RoomPrefabs prefabs = RoomPrefabs.RoomPrefabs.FirstOrDefault(rp => rp.Type == type);
        if (prefabs == null || prefabs.Prefabs.Count == 0)
        {
            Debug.LogWarning($"No prefabs found for room type: {type}");
            return null;
        }

        GameObject prefab = prefabs.Prefabs[Random.Range(0, prefabs.Prefabs.Count)];
        GameObject roomObject = Instantiate(prefab, LevelContainer);
        Room room = roomObject.GetComponent<Room>();

        var doors = roomObject.GetComponentsInChildren<Door>();
        room.Doors.AddRange(doors);
        room.SetBounds();

        return room;
    }

    private Door.DoorDirection GetOppositeDirection(Door.DoorDirection direction)
    {
        return direction switch
        {
            Door.DoorDirection.UP => Door.DoorDirection.DOWN,
            Door.DoorDirection.DOWN => Door.DoorDirection.UP,
            Door.DoorDirection.LEFT => Door.DoorDirection.RIGHT,
            Door.DoorDirection.RIGHT => Door.DoorDirection.LEFT,
            _ => throw new System.ArgumentOutOfRangeException(nameof(direction), $"Not a valid DoorDirection: {direction}"),
        };
    }

    private Door.DoorDirection? GetConnectionDirection(RoomLayoutNode from, RoomLayoutNode to)
    {
        Vector2 diff = to.EditorPosition - from.EditorPosition;

        const float tolerance = 1f;

        if (Mathf.Abs(diff.x) < tolerance) // Vertical alignment
        {
            if (diff.y < tolerance) return Door.DoorDirection.UP;
            if (diff.y > -tolerance) return Door.DoorDirection.DOWN;
        }
        else if (Mathf.Abs(diff.y) < tolerance) // Horizontal alignment
        {
            if (diff.x > tolerance) return Door.DoorDirection.RIGHT;
            if (diff.x < -tolerance) return Door.DoorDirection.LEFT;
        }

        return null; // Not orthogonally aligned
    }
}
