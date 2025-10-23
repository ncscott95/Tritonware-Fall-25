using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ExitDoor triggered by: " + collision.name);
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player reached the exit door!");
            UIManager.Instance.winScreen.ShowScreen();
            Destroy(collision.gameObject);
        }
    }
}
