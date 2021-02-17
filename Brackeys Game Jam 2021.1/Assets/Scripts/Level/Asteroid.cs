using System;
using Interactivity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level
{
    [RequireComponent(typeof(Rigidbody), typeof(DamageableComponent))]
    public class Asteroid : MonoBehaviour
    {
        private Rigidbody m_Rb;
        private DamageableComponent m_DamageableComponent;
        private Vector3 m_InitDirection;

        [Range(1, 10)] public float maxVelocity, minVelocity;

        [Range(1, 10)] public float maxSize, minSize;

        private void Awake()
        {
            m_Rb = GetComponent<Rigidbody>();
            m_DamageableComponent = GetComponent<DamageableComponent>();

            m_InitDirection = Random.insideUnitSphere.normalized * Random.Range(maxVelocity, minVelocity) * 100f;
            m_InitDirection = new Vector3(m_InitDirection.x, 0, m_InitDirection.z);

            Transform transform1;
            (transform1 = transform).localScale = Vector3.one * Random.Range(maxSize, minSize);


            m_DamageableComponent.maxHealth = 10 + (transform1.localScale.magnitude * 2f);
        }


        private void FixedUpdate()
        {
            m_Rb.velocity = m_InitDirection * Time.fixedDeltaTime;
        }


        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.GetComponent<DamageableComponent>() is { } damageableComponent &&
                damageableComponent != null)
                damageableComponent.TakeDamage(transform.localScale.magnitude);
            gameObject.SetActive(false);
        }
    }
}