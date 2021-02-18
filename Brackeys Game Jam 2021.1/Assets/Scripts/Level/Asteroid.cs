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
        internal Vector3 InitDirection;

        [Range(1, 10)] public float maxVelocity, minVelocity;

        [Range(1, 10)] public float maxSize, minSize;

        private void Awake()
        {
            m_Rb = GetComponent<Rigidbody>();
            m_DamageableComponent = GetComponent<DamageableComponent>();

         

            Transform transform1;
            (transform1 = transform).localScale = (Vector3.one * Random.Range(minSize, maxSize)) / 2f;


            m_DamageableComponent.maxHealth = 10 + (transform1.localScale.magnitude * 2f);
        }


        private void FixedUpdate()
        {
            m_Rb.velocity = InitDirection * Time.fixedDeltaTime;
        }


        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.GetComponent<DamageableComponent>() is { } damageableComponent &&
                damageableComponent != null && other.collider.GetComponent<Asteroid>() == null)
                damageableComponent.TakeDamage(transform.localScale.magnitude);
        }
    }
}