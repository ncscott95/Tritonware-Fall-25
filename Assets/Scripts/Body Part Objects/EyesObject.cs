using UnityEngine;

public class EyesObject : BodyPartObject
{
    public Sprite sprite;

    public override void HandleCollision(Collision2D collision)
    {
        BodyPart eyes = Player.Instance.body.eyes;
        Player.Instance.SwapEyeVisuals(sprite);
        InitBodyPartStats(eyes);
    }
}