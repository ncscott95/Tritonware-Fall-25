using UnityEngine;

public class ArmObject : BodyPartObject
{
    public BodyPartType armType = BodyPartType.LEFT_ARM;

    public override void HandleCollision(Collision2D collision)
    {
        BodyPart arm;
        if (armType == BodyPartType.LEFT_ARM)
        {
            arm = Player.Instance.body.leftArm;
        }
        else if (armType == BodyPartType.RIGHT_ARM)
        {
            arm = Player.Instance.body.rightArm;
        }
        else
        {
            return;
        }
        InitBodyPartStats(arm);
    }
}