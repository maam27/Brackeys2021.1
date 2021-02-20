using System;
using UnityEngine;

namespace Interactivity
{
    public class DamageableComponent : MonoBehaviour
    {
        public float maxHealth;
        private float m_CurrentHealth;
        public event Action ONDeathCallback;
        public float CurrentHealth => m_CurrentHealth;

        private void OnEnable()
        {
            m_CurrentHealth = maxHealth;
        }
        

        public void TakeDamage(float damage)
        {
            m_CurrentHealth -= damage;
            if (m_CurrentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            gameObject.SetActive(false);
            ONDeathCallback?.Invoke();
        }
    }
}