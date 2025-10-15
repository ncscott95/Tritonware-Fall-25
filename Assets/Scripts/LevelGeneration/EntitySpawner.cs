using UnityEngine;

public abstract class EntitySpawner : MonoBehaviour
{
    public Color32 GizmoColor = Color.white;
    [SerializeField] protected GameObject entityPrefab;

    public abstract void SpawnEntity();
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawWireSphere(transform.position, 0.25f);
    }
}
