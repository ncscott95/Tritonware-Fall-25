using UnityEngine;

public class ItemHover : MonoBehaviour
{
    private float startYPosition;
    public float hoverFrequency = 1.25f;
    public float heightOffset = 0.1f;

    void Start()
    {
        startYPosition = transform.position.y;
    }

    void FixedUpdate()
    {
        HoverAboveGround(Time.fixedTime);
        Debug.Log(transform.position);
    }

    private void HoverAboveGround(float time)
    {
        float yPosition = startYPosition + Mathf.Sin(hoverFrequency * time) * heightOffset;

        transform.position = new(transform.position.x, yPosition);
    }
}