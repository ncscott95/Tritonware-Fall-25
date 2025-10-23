using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public PlayerMovement movementComponent;
    public Body body;
    public Controls controls;
    public AbilityManager abilityManager;

    [Header("Visuals")]
    public Animator animator;
    public GameObject visuals;
    public SpriteRenderer eyesSR;
    public SpriteRenderer torsoSR;
    public SpriteRenderer leftArmSR;
    public SpriteRenderer rightArmSR;

    // 0 = left upper, 1 = right upper, 2 = left lower, 3 = right lower
    public List<SpriteRenderer> legsSpriteRenderers;
    public Transform legsObject;

    [Header("Base Body Parts")]
    public EyesObject baseEyes;
    public TorsoObject baseTorso;
    public ArmObject baseRightArm;
    public LegsObject baseLegs;

    [Header("Torso Sprites")]
    public Sprite baseTorsoSprite;
    public Sprite inactiveJetpackSprite;
    public Sprite activeJetpackSprite;

    [Header("Leg Sprites")]
    public List<Sprite> baseLegsSprites;
    public List<Sprite> enemyLegsSprites;

    [Header("Eye Lights")]
    public GameObject baseEyeLight;
    public GameObject enemyEyeLight;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SwapEyeVisuals(Sprite newEyesSprite = null, bool usingEnemyEyes = false)
    {
        eyesSR.sprite = newEyesSprite;
        baseEyeLight.SetActive(!usingEnemyEyes);
        enemyEyeLight.SetActive(usingEnemyEyes);
    }

    public void SwapTorsoVisuals(Sprite newTorsoSprite = null)
    {
        torsoSR.sprite = newTorsoSprite;
    }

    public void SwapRightArmVisuals(Sprite newRightArmSprite = null)
    {
        rightArmSR.sprite = newRightArmSprite;
    }

    public void SwapLegVisuals(bool usingEnemyLegs)
    {
        List<Sprite> selectedSprites = usingEnemyLegs ? enemyLegsSprites : baseLegsSprites;
        for (int i = 0; i < legsSpriteRenderers.Count; i++)
        {
            legsSpriteRenderers[i].sprite = selectedSprites[i];
        }
        legsObject.localScale = new Vector3(usingEnemyLegs ? -1 : 1, 1, 1);
        animator.SetBool("usingEnemyLegs", usingEnemyLegs);
        animator.SetTrigger("resetCycle");
    }

    public void SetJetpackActiveVisuals(bool isJetpackEquipped, bool isActive)
    {
        if (!isJetpackEquipped)
        {
            torsoSR.sprite = baseTorsoSprite;
            return;
        }
        torsoSR.sprite = isActive ? activeJetpackSprite : inactiveJetpackSprite;
    }
}
