using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserBeam : MonoBehaviour
{
    [SerializeField]
    private Attack attackComponent;
    public float beamWidth;
    public bool playerCollidedWithBeam;
    [SerializeField]
    private AudioSource blasterAudio;

    void Start()
    {
        beamWidth = gameObject.transform.lossyScale.x;
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("player collided with beam");
            playerCollidedWithBeam = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerCollidedWithBeam = false;
        }
    }

    public void SetLaserBeamActive(bool enabled)
    {
        gameObject.SetActive(enabled);
        if (enabled)
        {
            blasterAudio.Play();
        }
        else
        {
            blasterAudio.Stop();
        }
    }
}
