using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public BodyPart eyes;
    public BodyPart torso;
    public BodyPart leftArm;
    public BodyPart rightArm;
    public BodyPart legs;
    private List<BodyPart> damageableBodyParts = new();

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

            // TODO: show death screen
        }
    }

    public BodyPart GetRandomBodyPart()
    {
        damageableBodyParts.Clear();

        damageableBodyParts.Add(torso);
        if (eyes.healthComponent.isUpgraded) damageableBodyParts.Add(eyes);
        if (rightArm.healthComponent.isUpgraded) damageableBodyParts.Add(rightArm);
        if (legs.healthComponent.isUpgraded) damageableBodyParts.Add(legs);

        int randomBodyPartChance = Random.Range(1, 100);
        Debug.Log($"Random Body Part Chance: {randomBodyPartChance}, Damageable Body Parts Count: {damageableBodyParts.Count}");
        BodyPart bodyPart;

        if (randomBodyPartChance <= 50 || damageableBodyParts.Count == 1)
        {
            bodyPart = torso;
        }
        else
        {
            int randomIndex = Random.Range(1, damageableBodyParts.Count);
            bodyPart = damageableBodyParts[randomIndex];
        }

        return bodyPart;
    }
}