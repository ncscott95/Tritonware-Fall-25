using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRoomLayout", menuName = "Level Generation/Room Layout")]
public class RoomLayoutData : ScriptableObject
{
    public List<RoomLayoutNode> Nodes = new();
    public List<RoomLayoutConnection> Connections = new();
}

[System.Serializable]
public class RoomLayoutConnection
{
    public string StartRoomID;
    public string EndRoomID;
}

[System.Serializable]
public class RoomLayoutNode
{
    public string RoomID = System.Guid.NewGuid().ToString();
    public Room.RoomType Type;

    // Position used only in the custom EditorWindow for visualization (not world position)
    public Vector2 EditorPosition;
}
