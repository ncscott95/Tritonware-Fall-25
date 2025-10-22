
using UnityEngine;

public class TorsoObject : BodyPartObject
{
    public Sprite sprite;

    public override void HandleCollision(Collision2D collision)
    {
        BodyPart torso = Player.Instance.body.torso;
        Player.Instance.SwapTorsoVisuals(sprite);
        InitBodyPartStats(torso);
    }
}