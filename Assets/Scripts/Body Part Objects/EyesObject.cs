using UnityEngine;

public class EyesObject : BodyPartObject
{
    public override void HandleCollision(Collision2D collision)
    {
        BodyPart eyes = Player.Instance.body.eyes;
        InitBodyPartStats(eyes);
    }
}