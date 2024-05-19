using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class StatBarUI : MonoBehaviour {
    public static StatBarUI instance;

    public CombatData entity;

    [SerializeField] Slider healthSlider;
    [SerializeField] Slider easeHealthSlider;
    public float currentHealth;

    [SerializeField] Slider energySlider;
    public float currentEnergy;

    private void Awake() {
        instance = this;

        transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(entity.name.Replace("Stats", ""));

        healthSlider.maxValue = entity.health;
        easeHealthSlider.maxValue = healthSlider.maxValue;
        currentHealth = entity.currentHealth;

        energySlider.maxValue = entity.energy;
        currentEnergy = entity.currentEnergy;
    }

    public void UpdateEntityFocusPanel(CombatData newEntity) {
        entity = newEntity;

        transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(entity.name.Replace("Stats", ""));

        healthSlider.maxValue = entity.health;
        easeHealthSlider.maxValue = healthSlider.maxValue;
        currentHealth = entity.currentHealth;

        energySlider.maxValue = entity.energy;
        currentEnergy = entity.currentEnergy;
    }

    private void Update() {
        currentHealth = entity.currentHealth;
        if(healthSlider.value != currentHealth) healthSlider.value = currentHealth;
        if(healthSlider.value != easeHealthSlider.value) easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, currentHealth, 0.05f);

        currentEnergy = entity.currentEnergy;
        if(energySlider.value != currentEnergy)
            energySlider.value = currentEnergy;

    }
}