using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Attack attackComponent;
    public Health healthComponent;
    public Hitbox hitboxComponent;
    protected Transform target;
    protected bool isAggro;
    public GameObject dropOnDeath;

    protected void DamagePlayer()
    {
        Player.Instance.body.DamagePlayerBody(attackComponent);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            DamagePlayer();
        }
    }

    public void SetAggro(bool isAggro, Transform target = null)
    {
        this.isAggro = isAggro;
        this.target = target;
        OnSetAggro(isAggro);
    }

    protected virtual void OnSetAggro(bool isAggro) { }

    void OnDestroy()
    {
        // This prevents instantiation when the scene is being unloaded or the application is quitting.
        // `gameObject.scene.isLoaded` will be false when stopping play mode in the editor.
        if (dropOnDeath != null && gameObject.scene.isLoaded)
        {
            Instantiate(dropOnDeath, transform.position, Quaternion.identity);
        }
    }
}