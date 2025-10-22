using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomPrefabs
{
    public Room.RoomType Type;
    public List<GameObject> Prefabs = new();
}

[CreateAssetMenu(fileName = "NewRoomPrefabLists", menuName = "Level Generation/Room Prefab Lists")]
public class RoomPrefabLists : ScriptableObject
{
    public List<RoomPrefabs> RoomPrefabs = new();
}
