using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Health health;

    private void Update()
    {
        healthBarSlider.value = (float) health.CurrentHealth / health.MaxHealth;
    }
}
