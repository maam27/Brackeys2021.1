using System;
using System.Collections.Generic;
using System.Linq;
using Interactivity;
using UnityEngine;

namespace Ship.Weapons.Weapon_Fire
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        private Rigidbody m_Rb;
        private float m_CurrentLifetime;

        internal float lifetime, damage, velocity;

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            ResetTransform();
            ResetRigidbody();
            m_CurrentLifetime = 0;
        }

        private void ResetTransform()
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }


        private void ResetRigidbody()
        {
            m_Rb = m_Rb == null ? GetComponent<Rigidbody>() : m_Rb;

            m_Rb.useGravity = false;
            m_Rb.velocity = Vector3.zero;
            m_Rb.angularVelocity = Vector3.zero;
            m_Rb.ResetInertiaTensor();
            m_Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }


        private void FixedUpdate()
        {
            m_Rb.velocity = transform.forward * velocity;
        }

        private void Update()
        {
            m_CurrentLifetime += Time.deltaTime;
            if (m_CurrentLifetime >= lifetime)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.GetComponent<DamageableComponent>() is { } damageableComponent &&
                damageableComponent != null && other.gameObject.GetComponent<ShipController>() == null)
            {
                damageableComponent.TakeDamage(damage);
            }

            gameObject.SetActive(false);
        }
    }
}