
using UnityEngine;

public class LegsObject : BodyPartObject
{
    public float jumpStrengthMultiplier = 1f;
    public float moveSpeedMultiplier = 1f;
    public override void HandleCollision(Collision2D collision)
    {
        BodyPart legs = Player.Instance.body.legs;
        Player.Instance.SwapLegVisuals(isUpgraded);
        InitBodyPartStats(legs);
        Player.Instance.movementComponent.jumpStrengthMultiplier = jumpStrengthMultiplier;
        Player.Instance.movementComponent.moveSpeedMultiplier = moveSpeedMultiplier;
    }
}