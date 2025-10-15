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

    [SerializeField] private Tilemap border;
    [SerializeField] private Tilemap walls;
    [SerializeField] private TileBase wallTileUp;
    [SerializeField] private TileBase wallTileDown;
    [SerializeField] private TileBase wallTileLeft;
    [SerializeField] private TileBase wallTileRight;

    void Awake()
    {
        border.GetComponent<TilemapRenderer>().enabled = false;
    }

    void OnValidate()
    {
        if (border != null)
        {
            SetBounds();
        }
    }

    public void SetBounds()
    {
        border.CompressBounds();
        Size = new Vector2Int(border.cellBounds.size.x, border.cellBounds.size.y);
    }

    public void CloseDoors(List<Door> doorsToClose)
    {
        Debug.Log($"Closing {doorsToClose.Count} doors in room '{name}'.");
        foreach (var door in doorsToClose)
        {
            SetDoorOpen(door.transform.localPosition, door.Direction, false);
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
