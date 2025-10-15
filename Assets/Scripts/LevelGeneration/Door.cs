using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorDirection
    {
        LEFT,  // Player enters room from left side
        RIGHT, // Player enters room from right side
        UP,    // Player enters room from top side
        DOWN   // Player enters room from bottom side
    }

    public DoorDirection Direction;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 0));

        Vector3 center = transform.position;
        float arrowSize = 0.25f;
        Vector3 arrowEnd;

        switch (Direction)
        {
            case DoorDirection.UP:
                arrowEnd = center + new Vector3(0, 0.5f, 0);
                Gizmos.DrawLine(center, arrowEnd);
                Gizmos.DrawLine(arrowEnd, arrowEnd + new Vector3(-arrowSize, -arrowSize, 0));
                Gizmos.DrawLine(arrowEnd, arrowEnd + new Vector3(arrowSize, -arrowSize, 0));
                break;
            case DoorDirection.DOWN:
                arrowEnd = center + new Vector3(0, -0.5f, 0);
                Gizmos.DrawLine(center, arrowEnd);
                Gizmos.DrawLine(arrowEnd, arrowEnd + new Vector3(-arrowSize, arrowSize, 0));
                Gizmos.DrawLine(arrowEnd, arrowEnd + new Vector3(arrowSize, arrowSize, 0));
                break;
            case DoorDirection.LEFT:
                arrowEnd = center + new Vector3(-0.5f, 0, 0);
                Gizmos.DrawLine(center, arrowEnd);
                Gizmos.DrawLine(arrowEnd, arrowEnd + new Vector3(arrowSize, arrowSize, 0));
                Gizmos.DrawLine(arrowEnd, arrowEnd + new Vector3(arrowSize, -arrowSize, 0));
                break;
            case DoorDirection.RIGHT:
                arrowEnd = center + new Vector3(0.5f, 0, 0);
                Gizmos.DrawLine(center, arrowEnd);
                Gizmos.DrawLine(arrowEnd, arrowEnd + new Vector3(-arrowSize, arrowSize, 0));
                Gizmos.DrawLine(arrowEnd, arrowEnd + new Vector3(-arrowSize, -arrowSize, 0));
                break;
        }
    }
}
