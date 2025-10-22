using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Health healthComponent;
    [SerializeField]
    private Slider healthBarSlider;

    // Update is called once per frame
    void Update()
    {
        healthBarSlider.maxValue = healthComponent.maxHealth;
        healthBarSlider.value = healthComponent.health;
    }
}
