using System.Collections.Generic;
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

    [Header("Tilemap Data")]
    [SerializeField] private Tilemap border;
    [SerializeField] private Tilemap walls;
    [SerializeField] private TileBase wallTileUp;
    [SerializeField] private TileBase wallTileDown;
    [SerializeField] private TileBase wallTileLeft;
    [SerializeField] private TileBase wallTileRight;

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

        if (enemiesToSpawn <= 0 || EnemySpawners.Count == 0) return;

        List<EnemySpawner> spawnersToUse = new();

        if (enemiesToSpawn > EnemySpawners.Count)
        {
            Debug.LogWarning($"Room '{name}' has {enemiesToSpawn} enemies to spawn but only {EnemySpawners.Count} spawners. Reducing enemy count.");
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

    void OnValidate()
    {
        if (border != null)
        {
            border.CompressBounds();
            Size = new Vector2Int(border.cellBounds.size.x, border.cellBounds.size.y);
        }
    }

    public void CloseDoors(List<Door> doorsToClose)
    {
        foreach (var door in doorsToClose)
        {
            SetDoorOpen(door.transform.localPosition, door.Direction, false);
            door.GizmoColor = Color.gray;
        }
    }

    public void SetDoorOpen(Vector3 doorPosition, Door.DoorDirection direction, bool isOpen)
    {
        Vector2Int gridPos = new((int)(doorPosition.x - 0.5f), (int)(doorPosition.y - 0.5f));
        Vector3Int tilePos = new(gridPos.x, gridPos.y, 0);
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
