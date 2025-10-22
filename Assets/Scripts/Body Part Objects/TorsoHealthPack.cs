
using UnityEngine;

public class TorsoHealthPack : BodyPartObject
{
    [SerializeField]
    private float healPercentage = 0.25f;

    public override void HandleCollision(Collision2D collision)
    {
        Body playerBody = Player.Instance.body;
        BodyPart eyes = playerBody.eyes;
        BodyPart torso = playerBody.torso;
        BodyPart leftArm = playerBody.leftArm;
        BodyPart rightArm = playerBody.rightArm;
        BodyPart legs = playerBody.legs;
        eyes.healthComponent.HealByPercentage(healPercentage);
        torso.healthComponent.HealByPercentage(healPercentage);
        leftArm.healthComponent.HealByPercentage(healPercentage);
        rightArm.healthComponent.HealByPercentage(healPercentage);
        legs.healthComponent.HealByPercentage(healPercentage);
    }
}