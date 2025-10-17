
using UnityEngine;

public class TorsoObject : BodyPartObject
{
    public override void HandleCollision(Collision2D collision)
    {
        BodyPart torso = Player.Instance.body.torso;
        InitBodyPartStats(torso);
    }
}