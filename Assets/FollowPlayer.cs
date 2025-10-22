using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private float smoothSpeed = 0.5f;
    private Vector3 maxCameraVelocity = new Vector3(1, 1, 0);
    private Vector3 targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Transform playerTransform = Player.Instance.transform;
        targetPosition = new(playerTransform.position.x, playerTransform.position.y, -10);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref maxCameraVelocity, smoothSpeed);
    }
}
