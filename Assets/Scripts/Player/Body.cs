using UnityEngine;

public class Body : MonoBehaviour
{
    public BodyPart eyes;
    public BodyPart torso;
    public BodyPart leftArm;
    public BodyPart rightArm;
    public BodyPart legs;

    void Update()
    {
        HandlePlayerDeath();
    }

    void HandlePlayerDeath()
    {
        if (torso.healthComponent.isDead)
        {
            Debug.Log("Torso Health depleted! Player died!");
            Destroy(Player.Instance.gameObject);
        }
    }

    public BodyPart GetRandomBodyPart()
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