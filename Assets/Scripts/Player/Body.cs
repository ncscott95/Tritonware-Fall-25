using UnityEngine;

public class Body : MonoBehaviour
{
    public Torso torso;
    public Arm arm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleAbilities();
    }

    private void HandleAbilities()
    {
        if (torso != null)
            torso.ActivateAbility();
        if (arm != null)
            arm.ActivateAbility();
    }

    public void SwapTorso(Torso torso)
    {
        this.torso = torso;
    }

    public void SwapArm(Arm arm)
    {
        this.arm = arm;
    }
}
