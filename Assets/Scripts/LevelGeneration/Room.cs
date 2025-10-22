using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public enum RoomType
    {
        START,
        END,
        OUTSIDE_FLAT,
        INSIDE_BOTTOM,
        INSIDE_MIDDLE,
        INSIDE_TOP,
        INSIDE_HALLWAY,
        ARENA
    }

    public Vector2Int Size { get; private set; }
    public List<Door> Doors { get; set; } = new();
    public List<ItemSpawner> ItemSpawners { get; set; }
    public List<EnemySpawner> EnemySpawners { get; set; }

    [Header("Room Data")]
    public bool tryMakeConnections = false;
    public bool acceptsConnections = true;

    [Header("Tilemap Data")]
    [SerializeField] private Tilemap border;
    [SerializeField] private Tilemap walls;
    [SerializeField] private Tilemap background;
    [SerializeField] private TileBase wallTileUp;
    [SerializeField] private TileBase wallTileDown;
    [SerializeField] private TileBase wallTileLeft;
    [SerializeField] private TileBase wallTileRight;
    [SerializeField] private TileBase connectionWallTile;
    [SerializeField] private TileBase connectionBackgroundTile;
    private static readonly float wallConnectRaycastDistance = 5f;

    [Header("Spawning Data")]
    [SerializeField] private int enemiesToSpawn = 0;

    void Awake()
    {
        border.GetComponent<TilemapRenderer>().enabled = false;
        ItemSpawners = new(GetComponentsInChildren<ItemSpawner>());
        EnemySpawners = new(GetComponentsInChildren<EnemySpawner>());
    }

    public void InitializeRoom()
    {
        foreach (var spawner in ItemSpawners)
        {
            spawner.SpawnEntity();
        }

        ConnectWallsDown();

        if (enemiesToSpawn <= 0 || EnemySpawners.Count == 0) return;

        List<EnemySpawner> spawnersToUse = new();

        if (enemiesToSpawn > EnemySpawners.Count)
        {
            // Debug.LogWarning($"Room '{name}' has {enemiesToSpawn} enemies to spawn but only {EnemySpawners.Count} spawners. Reducing enemy count.");
            spawnersToUse = EnemySpawners;
        }
        else
        {
            for (int i = 0; i < enemiesToSpawn && EnemySpawners.Count > 0; i++)
            {
                var spawner = EnemySpawners[Random.Range(0, EnemySpawners.Count)];
                spawnersToUse.Add(spawner);
                EnemySpawners.Remove(spawner);
            }
        }

        foreach (var spawner in spawnersToUse)
        {
            spawner.SpawnEntity();
        }

        foreach (var spawner in EnemySpawners)
        {
            spawner.GizmoColor = Color.gray;
        }
    }

    private void ConnectWallsDown()
    {
        if (!tryMakeConnections) return;

        if (border == null || walls == null || wallTileDown == null)
        {
            // Debug.LogWarning($"Room '{name}' is missing tilemap references for wall connection. Aborting.");
            return;
        }

        BoundsInt bounds = border.cellBounds;
        int bottomY = bounds.yMin;

        // Fill in wall tiles downwards

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            Vector3Int localPos = new Vector3Int(x, bottomY, 0);
            if (!walls.HasTile(localPos)) continue;

            Vector3 worldPos = walls.GetCellCenterWorld(localPos);
            Vector3 startPos = worldPos + new Vector3(0, -1f, 0);

            // Raycast down to find other room walls
            RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.down, wallConnectRaycastDistance, LayerMask.GetMask("Terrain"));

            if (hit.collider != null && hit.collider.transform.parent.transform.parent.TryGetComponent<Room>(out var otherRoom))
            {
                if (otherRoom == this || !otherRoom.acceptsConnections) continue;

                // We hit another room's wall tilemap
                int otherRoomLocalY = otherRoom.border.cellBounds.yMax;
                float worldY = otherRoom.walls.CellToWorld(new Vector3Int(0, otherRoomLocalY, 0)).y;
                int hitY = walls.WorldToCell(new Vector3(0, worldY, 0)).y;

                for (int y = localPos.y - 1; y >= hitY; y--)
                {
                    walls.SetTile(new Vector3Int(x, y, 0), connectionWallTile);
                }
            }
        }

        // Find a door in range and open a chute if matching an upward door

        foreach (var door in Doors)
        {
            if (door.Direction != Door.DoorDirection.DOWN) continue;

            Vector3 worldPos = door.transform.position + new Vector3(0, -1f, 0);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.down, wallConnectRaycastDistance, LayerMask.GetMask("Terrain"));
            
            if (hit.collider != null && hit.collider.transform.parent.transform.parent.TryGetComponent<Room>(out var otherRoom))
            {
                if (otherRoom == this || !otherRoom.acceptsConnections) continue;

                // We hit another room's wall tilemap
                int otherRoomLocalY = otherRoom.border.cellBounds.yMax;
                float hitWorldY = otherRoom.walls.CellToWorld(new Vector3Int(0, otherRoomLocalY, 0)).y;
                int hitY = walls.WorldToCell(new Vector3(0, hitWorldY, 0)).y;

                // Check if the other room has an UP door at the same X position
                bool hasMatchingUpDoor = false;
                foreach (var otherDoor in otherRoom.Doors)
                {
                    if (otherDoor.Direction == Door.DoorDirection.UP)
                    {
                        float otherDoorWorldX = otherDoor.transform.position.x;
                        if (otherDoorWorldX == door.transform.position.x)
                        {
                            hasMatchingUpDoor = true;
                            otherRoom.SetDoorOpen(otherDoor, true);
                            break;
                        }
                    }
                }
                if (!hasMatchingUpDoor) continue;

                Vector2Int doorGridPos = new((int)(door.transform.localPosition.x - 0.5f), (int)(door.transform.localPosition.y - 0.5f));
                Vector3Int doorTilePos = new(doorGridPos.x, doorGridPos.y, 0);

                for (int y = doorTilePos.y; y >= hitY; y--)
                {
                    walls.SetTile(new Vector3Int(doorTilePos.x - 1, y, 0), null);
                    walls.SetTile(new Vector3Int(doorTilePos.x, y, 0), null);
                    walls.SetTile(new Vector3Int(doorTilePos.x + 1, y, 0), null);

                    background.SetTile(new Vector3Int(doorTilePos.x - 1, y, 0), connectionBackgroundTile);
                    background.SetTile(new Vector3Int(doorTilePos.x, y, 0), connectionBackgroundTile);
                    background.SetTile(new Vector3Int(doorTilePos.x + 1, y, 0), connectionBackgroundTile);
                }
            }
        }
    }

    void OnValidate()
    {
        if (border != null)
        {
            border.CompressBounds();
            Size = new Vector2Int(border.cellBounds.size.x, border.cellBounds.size.y);
        }
    }

    public void SetDoorsOpen(List<Door> doorsToOpen, bool open)
    {
        foreach (var door in doorsToOpen)
        {
            SetDoorOpen(door, open);
            door.GizmoColor = open ? Color.green : Color.gray;
        }
    }

    private void SetDoorOpen(Door doorToOpen, bool isOpen)
    {
        Vector2Int gridPos = new((int)(doorToOpen.transform.localPosition.x - 0.5f), (int)(doorToOpen.transform.localPosition.y - 0.5f));
        Vector3Int tilePos = new(gridPos.x, gridPos.y, 0);
        Door.DoorDirection direction = doorToOpen.Direction;
        TileBase wallTile = null;

        if (!isOpen)
        {
            wallTile = direction switch
            {
                Door.DoorDirection.UP => wallTileUp,
                Door.DoorDirection.DOWN => wallTileDown,
                Door.DoorDirection.LEFT => wallTileLeft,
                Door.DoorDirection.RIGHT => wallTileRight,
                _ => null
            };
        }

        walls.SetTile(tilePos, wallTile);

        if (Door.DoorDirection.UP == direction || Door.DoorDirection.DOWN == direction)
        {
            walls.SetTile(tilePos + new Vector3Int(1, 0, 0), wallTile);
            walls.SetTile(tilePos + new Vector3Int(-1, 0, 0), wallTile);
        }
        else if (Door.DoorDirection.LEFT == direction || Door.DoorDirection.RIGHT == direction)
        {
            walls.SetTile(tilePos + new Vector3Int(0, 1, 0), wallTile);
            walls.SetTile(tilePos + new Vector3Int(0, -1, 0), wallTile);
        }
    }
}
