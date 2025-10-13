using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapReader : MonoBehaviour
{
    [SerializeField] private TileBase doorTile;
    private Tilemap tilemap;

    public Room.TilemapData SaveTilemapData()
    {
        tilemap = GetComponent<Tilemap>();

        if (tilemap == null)
        {
            Debug.LogError("Tilemap component is missing.");
            return null;
        }

        if (doorTile == null)
        {
            Debug.LogError("Door TileBase is not assigned in the inspector.");
            return null;
        }

        Room.TilemapData roomData = GenerateRoomFromTilemap();
        Debug.Log(TileDataToString(roomData.Grid, roomData.Size.x, roomData.Size.y));
        return roomData;
    }

    Room.TilemapData GenerateRoomFromTilemap()
    {
        BoundsInt bounds = CalculateMapDimensions();
        Room.TilemapData data = new()
        {
            Origin = new Vector2Int(bounds.xMin, bounds.yMin),
            Size = new Vector2Int(bounds.size.x, bounds.size.y),
            Grid = GetGridData(bounds)
        };
        return data;
    }

    BoundsInt CalculateMapDimensions()
    {
        tilemap.CompressBounds();
        BoundsInt bounds = tilemap.cellBounds;
        return bounds;
    }

    int[,] GetGridData(BoundsInt bounds)
    {
        int width = bounds.size.x;
        int height = bounds.size.y;
        int[,] grid = new int[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3Int cellPosition = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);
                TileBase tile = tilemap.GetTile(cellPosition);

                if (tile == null)
                {
                    grid[x, y] = (int)Room.TileState.EMPTY;
                }
                else
                {
                    // Comparing the TileBase asset directly is more robust than comparing names.
                    if (tile == doorTile)
                    {
                        grid[x, y] = (int)Room.TileState.DOOR;
                    }
                    else
                    {
                        grid[x, y] = (int)Room.TileState.FILLED;
                    }
                }
            }
        }

        return grid;
    }

    string TileDataToString(int[,] grid, int width, int height)
    {
        string tileData = "";

        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                tileData += grid[x, y].ToString();
            }
            tileData += "\n";
        }

        return tileData;
    }
}
