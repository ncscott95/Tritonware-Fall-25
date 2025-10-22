using UnityEngine;
using UnityEditor;
using System.Linq;

public class RoomLayoutEditorWindow : EditorWindow
{
    private RoomLayoutData currentLayout;
    private GUIStyle nodeStyle;
    private GUIStyle connectingNodeStyle;
    private GUIStyle labelStyle;

    private const float NodeSize = 50f;
    private const float GridSize = 50f; // Must be greater than NodeSize
    private const float ConnectionLineThickness = 4f;

    private RoomLayoutNode draggingNode = null; 
    private RoomLayoutNode connectingNode = null; // The first node selected for a new connection
    private Vector2 dragOffset;


    [MenuItem("Level Generation/Room Layout Editor")]
    public static void ShowWindow()
    {
        RoomLayoutEditorWindow window = GetWindow<RoomLayoutEditorWindow>();
        window.titleContent = new GUIContent("Room Layout Graph");
        window.minSize = new Vector2(400, 300);
    }

    private void OnEnable()
    {
        // Visual style for the standard nodes
        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);
        nodeStyle.alignment = TextAnchor.MiddleCenter;
        nodeStyle.fontStyle = FontStyle.Bold;
        nodeStyle.fontSize = 10; // Smaller font size
        nodeStyle.normal.textColor = Color.white;
        
        // Visual style for the node being connected from (highlight)
        connectingNodeStyle = new GUIStyle();
        connectingNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node3.png") as Texture2D; 
        connectingNodeStyle.border = new RectOffset(12, 12, 12, 12);
        connectingNodeStyle.alignment = TextAnchor.MiddleCenter;
        connectingNodeStyle.fontStyle = FontStyle.Bold;
        connectingNodeStyle.fontSize = 10; // Smaller font size
        connectingNodeStyle.normal.textColor = Color.white;

        // Style for the rotated labels (transparent background)
        labelStyle = new GUIStyle(nodeStyle); // Copy properties from nodeStyle
        labelStyle.normal.background = null; // Make background transparent
        labelStyle.hover.background = null;
        labelStyle.active.background = null;
    }

    private void OnGUI()
    {
        // Selection area
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Layout Asset:", EditorStyles.boldLabel, GUILayout.Width(80));
        
        currentLayout = (RoomLayoutData)EditorGUILayout.ObjectField(
            currentLayout, 
            typeof(RoomLayoutData), 
            false
        );
        EditorGUILayout.EndHorizontal();

        if (currentLayout == null)
        {
            EditorGUILayout.HelpBox("Drag a 'Room Layout Data' asset here to start editing.", MessageType.Info);
            return;
        }

        // Main drawing area
        DrawGrid();
        DrawConnections();
        DrawNodes();

        // Handle mouse input 
        ProcessEvents();

        if (GUI.changed || connectingNode != null)
        {
            Repaint();
        }
    }

    private void DrawGrid()
    {
        int widthDivs = Mathf.CeilToInt(position.width / GridSize);
        int heightDivs = Mathf.CeilToInt(position.height / GridSize);
        
        Handles.BeginGUI();
        Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(GridSize * i, -GridSize), new Vector3(GridSize * i, position.height));
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-GridSize, GridSize * j), new Vector3(position.width, GridSize * j));
        }
        
        Handles.EndGUI();
    }

    private void DrawNodes()
    {
        if (currentLayout == null) return;

        GUILayout.BeginArea(new Rect(Vector2.zero, position.size));

        foreach (var node in currentLayout.Nodes)
        {
            Rect nodeRect = new Rect(node.EditorPosition, new Vector2(NodeSize, NodeSize));
            GUIStyle styleToUse = (node == connectingNode) ? connectingNodeStyle : nodeStyle;

            // Draw the node background without text
            GUI.Box(nodeRect, "", styleToUse);

            // Save the current GUI matrix
            Matrix4x4 matrix = GUI.matrix;

            // Rotate the GUI around the center of the node
            GUIUtility.RotateAroundPivot(330f, nodeRect.center);

            // Draw the rotated text
            GUI.Label(nodeRect, node.Type.ToString(), labelStyle);

            // Restore the original matrix
            GUI.matrix = matrix;

            if (GUI.changed)
            {
                EditorUtility.SetDirty(currentLayout);
            }
        }

        GUILayout.EndArea();
    }

    private void DrawConnections()
    {
        if (currentLayout == null || currentLayout.Nodes.Count < 1) return;

        // Draw saved connections
        Handles.color = new Color(0.6f, 0.8f, 1f, 0.8f);
        foreach (var connection in currentLayout.Connections)
        {
            RoomLayoutNode startNode = currentLayout.Nodes.FirstOrDefault(n => n.RoomID == connection.StartRoomID);
            RoomLayoutNode endNode = currentLayout.Nodes.FirstOrDefault(n => n.RoomID == connection.EndRoomID);

            if (startNode != null && endNode != null)
            {
                Vector2 startCenter = startNode.EditorPosition + new Vector2(NodeSize / 2, (NodeSize * 0.7f) / 2);
                Vector2 endCenter = endNode.EditorPosition + new Vector2(NodeSize / 2, (NodeSize * 0.7f) / 2);

                Vector2 dir = (endCenter - startCenter).normalized;
                Vector2 startTangent = startCenter + dir * 50;
                Vector2 endTangent = endCenter - dir * 50;

                Handles.DrawBezier(
                    startCenter,
                    endCenter,
                    startTangent,
                    endTangent,
                    Handles.color,
                    null,
                    ConnectionLineThickness
                );
            }
        }

        // Temporary connection line while connecting
        if (connectingNode != null)
        {
            Vector2 startCenter = connectingNode.EditorPosition + new Vector2(NodeSize / 2, (NodeSize * 0.7f) / 2);
            Vector2 mousePosition = Event.current.mousePosition;

            Handles.color = Color.yellow;
            Handles.DrawLine(startCenter, mousePosition, 2f);
        }

        Handles.color = Color.white;
    }

    private void ProcessEvents()
    {
        if (currentLayout == null) return;
        Event current = Event.current;
        Vector2 mousePosition = current.mousePosition;

        switch (current.type)
        {
            case EventType.MouseDown:
                if (current.button == 0) // Left click
                {
                    RoomLayoutNode clickedNode = GetNodeAtPosition(mousePosition);

                    if (connectingNode != null)
                    {
                        // Have start node, trying to select the end node
                        if (clickedNode != null && clickedNode != connectingNode)
                        {
                            // Success: Check Orthogonal constraint before creating connection
                            CreateConnection(connectingNode, clickedNode);
                            connectingNode = null;
                            GUI.changed = true;
                            current.Use();
                            return;
                        }
                        else
                        {
                            // Fail: Clicked background or the same node, cancel connection
                            connectingNode = null;
                            GUI.changed = true;
                            current.Use();
                            return;
                        }
                    }

                    // No node selected for connection, check for Drag or Connection Start
                    if (clickedNode != null)
                    {
                        if (current.shift) // Shift + Click: Start connection workflow
                        {
                            connectingNode = clickedNode;
                            draggingNode = null; // Stop dragging mode
                            GUI.changed = true;
                            current.Use();
                        }
                        else // Normal click: Start dragging
                        {
                            draggingNode = clickedNode;
                            dragOffset = draggingNode.EditorPosition - mousePosition;
                            GUI.changed = true; 
                            current.Use();
                        }
                    }
                    else
                    {
                        draggingNode = null;
                        connectingNode = null;
                    }
                }
                else if (current.button == 1) // Right click for context menu
                {
                    if (connectingNode != null)
                    {
                        connectingNode = null;
                        GUI.changed = true;
                    }
                    ShowContextMenu(mousePosition);
                }
                break;
            case EventType.MouseDrag: // Dragging
                if (draggingNode != null && current.button == 0)
                {
                    Vector2 newCenter = mousePosition + dragOffset + new Vector2(NodeSize / 2, (NodeSize * 0.7f) / 2);

                    newCenter.x = Mathf.Round(newCenter.x / GridSize) * GridSize;
                    newCenter.y = Mathf.Round(newCenter.y / GridSize) * GridSize;

                    draggingNode.EditorPosition = newCenter - new Vector2(NodeSize / 2, (NodeSize * 0.7f) / 2);
                    
                    GUI.changed = true;
                    EditorUtility.SetDirty(currentLayout); // Mark dirty to save position changes
                    current.Use();
                }
                break;
            case EventType.MouseUp: // Stop dragging
                draggingNode = null;
                dragOffset = Vector2.zero;
                break;
        }
    }

    private RoomLayoutNode GetNodeAtPosition(Vector2 position)
    {
        return currentLayout.Nodes.FirstOrDefault(node => 
        {
            Rect nodeRect = new Rect(node.EditorPosition, new Vector2(NodeSize, NodeSize * 0.7f));
            return nodeRect.Contains(position);
        });
    }
    
    private void CreateConnection(RoomLayoutNode startNode, RoomLayoutNode endNode)
    {
        Vector2 diff = startNode.EditorPosition - endNode.EditorPosition;
        float tolerance = 1f; // Small tolerance for float comparison
        
        // Check if nodes are horizontally or vertically aligned AND exactly one grid unit apart
        bool isOrthogonalAndAdjacent = 
            (Mathf.Abs(diff.x) < tolerance && Mathf.Abs(diff.y - GridSize) < tolerance) || // Vertical Down
            (Mathf.Abs(diff.x) < tolerance && Mathf.Abs(diff.y + GridSize) < tolerance) || // Vertical Up
            (Mathf.Abs(diff.x - GridSize) < tolerance && Mathf.Abs(diff.y) < tolerance) || // Horizontal Right
            (Mathf.Abs(diff.x + GridSize) < tolerance && Mathf.Abs(diff.y) < tolerance);   // Horizontal Left

        if (!isOrthogonalAndAdjacent)
        {
            Debug.LogWarning("Connection Failed: Nodes must be orthogonally adjacent (1 grid unit apart) to connect.");
            return;
        }

        // Check for duplicate connection (A -> B or B -> A)
        bool alreadyExists = currentLayout.Connections.Any(c => 
            (c.StartRoomID == startNode.RoomID && c.EndRoomID == endNode.RoomID) ||
            (c.StartRoomID == endNode.RoomID && c.EndRoomID == startNode.RoomID));

        if (alreadyExists)
        {
            Debug.LogWarning("Connection already exists.");
            return;
        }

        RoomLayoutConnection newConnection = new RoomLayoutConnection
        {
            StartRoomID = startNode.RoomID,
            EndRoomID = endNode.RoomID,
        };

        Undo.RecordObject(currentLayout, "Add Room Connection");
        currentLayout.Connections.Add(newConnection);
        EditorUtility.SetDirty(currentLayout);
    }

    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();
        RoomLayoutNode clickedNode = GetNodeAtPosition(mousePosition);

        if (clickedNode != null)
        {
            menu.AddItem(new GUIContent("Delete Room Node"), false, () => DeleteNode(clickedNode));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Start Connection From Here"), false, () => 
            {
                connectingNode = clickedNode;
                GUI.changed = true;
            });
        }

        menu.AddItem(new GUIContent("Create New Room Node (Snaps to Grid)"), false, () => CreateNewNode(mousePosition));
        menu.ShowAsContext();
    }

    private void CreateNewNode(Vector2 position)
    {
        Vector2 snappedCenter = position;
        snappedCenter.x = Mathf.Round(snappedCenter.x / GridSize) * GridSize;
        snappedCenter.y = Mathf.Round(snappedCenter.y / GridSize) * GridSize;

        RoomLayoutNode newNode = new RoomLayoutNode
        {
            EditorPosition = snappedCenter - new Vector2(NodeSize / 2, (NodeSize * 0.7f) / 2) 
        };

        Undo.RecordObject(currentLayout, "Add Room Node");
        currentLayout.Nodes.Add(newNode);
        EditorUtility.SetDirty(currentLayout);
    }

    private void DeleteNode(RoomLayoutNode nodeToDelete)
    {
        Undo.RecordObject(currentLayout, "Delete Room Node");
        currentLayout.Nodes.Remove(nodeToDelete);

        currentLayout.Connections.RemoveAll(c => c.StartRoomID == nodeToDelete.RoomID || c.EndRoomID == nodeToDelete.RoomID);

        EditorUtility.SetDirty(currentLayout);
    }
}
