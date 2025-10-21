
using UnityEngine;

public class TorsoHealthPack : BodyPartObject
{
    [SerializeField]
    private float healPercentage = 0.25f;

    public override void HandleCollision(Collision2D collision)
    {
        BodyPart torso = Player.Instance.body.torso;
        HealTorso(torso);
    }

    private void HealTorso(BodyPart torso)
    {
        Health torsoHealth = torso.healthComponent;
        torsoHealth.HealByPercentage(healPercentage);
    }
}