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
    [SerializeField] private AudioSource deathAudio;
    [SerializeField] private float immunityTime = 1f;
    private float elapsed;

    void Update()
    {
        elapsed += Time.deltaTime;
        HandlePlayerDeath();
    }

    void HandlePlayerDeath()
    {
        if (torso.healthComponent.isDead)
        {
            Destroy(Player.Instance.gameObject);
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
        Debug.Log($"{bodyPart} took {attackComponent.damage} damage");
    }

    private BodyPart GetRandomBodyPart()
    {
        damageableBodyParts.Clear();

        damageableBodyParts.Add(torso);
        if (eyes.healthComponent.isUpgraded) damageableBodyParts.Add(eyes);
        if (rightArm.healthComponent.isUpgraded) damageableBodyParts.Add(rightArm);
        if (legs.healthComponent.isUpgraded) damageableBodyParts.Add(legs);

        int randomBodyPartChance = Random.Range(1, 100);
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