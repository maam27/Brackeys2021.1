using System;
using UnityEngine;

namespace Interactivity
{
    public class DamageableComponent : MonoBehaviour
    {
        public float maxHealth;
        public float currentHealth;


        private void OnEnable()
        {
            currentHealth = maxHealth;
        }
        

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            gameObject.SetActive(false);
        }
    }
}