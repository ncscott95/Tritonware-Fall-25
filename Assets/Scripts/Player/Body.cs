using System.Collections;
using UnityEngine;

public class Body : MonoBehaviour
{
    public BodyPart eyes;
    public BodyPart torso;
    public BodyPart leftArm;
    public BodyPart rightArm;
    public BodyPart legs;
    [SerializeField] private AudioSource deathAudio;
    [SerializeField] private AudioSource damagedAudio;
    [SerializeField] private float immunityTime = 1f;
    private float elapsed;
    private bool startedDeathSequence = false;

    void Update()
    {
        elapsed += Time.deltaTime;
        HandlePlayerDeath();
    }

    void HandlePlayerDeath()
    {
        if (torso.healthComponent.isDead && !startedDeathSequence)
        {
            startedDeathSequence = true;
            deathAudio.Play();
            Destroy(Player.Instance.transform.Find("Visuals").gameObject);
            Destroy(Player.Instance.gameObject, deathAudio.clip.length);
            UIManager.Instance.deathScreen.ShowScreen();
        }
    }

    public void DamagePlayerBody(Attack attackComponent)
    {
        if (elapsed < immunityTime) return;
        elapsed %= immunityTime;
        BodyPart bodyPart = GetRandomBodyPart();
        Health bodyPartHealth = bodyPart.healthComponent;
        bodyPartHealth.TakeDamage(attackComponent.damage);
        damagedAudio.Play();
    }

    private BodyPart GetRandomBodyPart()
    {
        int randomBodyPartChance = Random.Range(1, 100);
        BodyPart bodyPart;
        switch (randomBodyPartChance)
        {
            case >= 1 and <= 40:
                bodyPart = torso;
                break;
            case >= 41 and <= 55:
                bodyPart = eyes;
                break;
            case >= 56 and <= 70:
                bodyPart = leftArm;
                break;
            case >= 71 and <= 85:
                bodyPart = rightArm;
                break;
            case >= 86 and <= 100:
                bodyPart = legs;
                break;
            default:
                bodyPart = torso;
                break;
        }

        return bodyPart;
    }
}