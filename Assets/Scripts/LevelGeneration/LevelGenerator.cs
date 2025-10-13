using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> Prefabs = new();
    public int NumberOfRooms = 5;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        if (Prefabs == null || Prefabs.Count == 0)
        {
            Debug.LogError("No prefabs assigned to the LevelGenerator.");
            return;
        }

        float originX = 0f;
        for (int i = 0; i < NumberOfRooms; i++)
        {
            GameObject prefab = Prefabs[Random.Range(0, Prefabs.Count)];
            Vector3 position = new Vector3(originX, 0, 0);
            Room room = Instantiate(prefab, position, Quaternion.identity).GetComponent<Room>();
            room.Initialize();
            originX += room.Size.x; // Move origin for next room
        }
    }
}
