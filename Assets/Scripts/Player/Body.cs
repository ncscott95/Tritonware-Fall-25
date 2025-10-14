using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Body : MonoBehaviour
{
    public BodyPart torso;
    public BodyPart leftArm;
    public BodyPart rightArm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        HandleAbilities();
    }

    private void HandleAbilities()
    {
        torso.ActivateAbility();
        leftArm.ActivateAbility();
    }
}