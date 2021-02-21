using System;
using Interactivity;
using Ship;
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
            targetHealthToDisplay = targetHealthToDisplay
                ? targetHealthToDisplay
                : transform.parent.parent.GetComponentInChildren<DamageableComponent>();
        }


        private void Update()
        {
            if (targetHealthToDisplay)
                m_HealthBar.value = targetHealthToDisplay.CurrentHealth / targetHealthToDisplay.maxHealth;
        }
    }
}