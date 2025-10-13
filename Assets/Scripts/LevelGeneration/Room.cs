using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public enum TileState
    {
        EMPTY = 0,
        FILLED = 1,
        DOOR = 2,
    }

    public class TilemapData
    {
        public Vector2Int Origin;
        public Vector2Int Size;
        public int[,] Grid;
    }

    public Vector2Int Size { get; private set; }
    public Vector2Int Origin { get; private set; }

    [SerializeField] private TilemapReader wallReader;
    [SerializeField] private TilemapReader doorsReader;

    private TilemapData wallData;
    private TilemapData doorsData;

    public void Initialize()
    {
        if (wallReader != null)
        {
            wallData = wallReader.SaveTilemapData();
        }
        else
        {
            Debug.LogError("Wall Reader is not assigned.");
            return;
        }

        if (doorsReader != null)
        {
            doorsData = doorsReader.SaveTilemapData();
            doorsReader.gameObject.GetComponent<TilemapRenderer>().enabled = false;
        }
        else
        {
            Debug.LogError("Doors Reader is not assigned.");
            return;
        }

        if (wallData == null || doorsData == null)
        {
            Debug.LogError("Failed to retrieve tilemap data.");
            return;
        }

        Size = new
        (
            Mathf.Max(wallData.Size.x, doorsData.Size.x),
            Mathf.Max(wallData.Size.y, doorsData.Size.y)
        );
    }
}
