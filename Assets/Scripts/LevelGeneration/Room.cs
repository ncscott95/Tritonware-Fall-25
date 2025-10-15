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
}
