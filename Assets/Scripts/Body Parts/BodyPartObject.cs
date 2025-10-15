using TMPro;
using UnityEngine;

public abstract class BodyPartObject : MonoBehaviour
{
    public Player player = Player.Instance;
    public AbilityType abilityType = AbilityType.NONE;
    public float decayRateMultiplier = 1;
    public int maxHealth = 100;
    private float heightOffset = 0.1f;
    private float hoverFrequency = 1.25f;

    private float startYPosition;

    void Start()
    {
        startYPosition = transform.position.y;
        Debug.Log(startYPosition);
    }

    void FixedUpdate()
    {
        HoverAboveGround(Time.fixedTime);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            HandleCollision(collision);
            Destroy(gameObject);
        }
    }

    public abstract void HandleCollision(Collision2D collision);

    private void HoverAboveGround(float time)
    {
        float yPosition = startYPosition + Mathf.Sin(hoverFrequency * time) * heightOffset;

        transform.position = new(transform.position.x, yPosition);
    }
}