using System;
using Interactivity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        public DamageableComponent targetHealthToDisplay;

        private Slider m_HealthBar;

        private void Awake()
        {
            m_HealthBar = GetComponent<Slider>();
        }


        private void Update()
        {
            m_HealthBar.value = targetHealthToDisplay.CurrentHealth / targetHealthToDisplay.maxHealth;
        }
    }
}