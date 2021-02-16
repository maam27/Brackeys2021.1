using System;
using System.Collections.Generic;
using System.Linq;
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
            Debug.Log($"Collided with {other.collider.name}! Attempting to deal {damage} to it!");
            gameObject.SetActive(false);
        }

       
    }


    public static class ObjectPooler
    {
        private static readonly Dictionary<int, List<GameObject>> PoolerDictionary =
            new Dictionary<int, List<GameObject>>();

        public static bool PoolGameObject(GameObject objToPool, int amountToPool = 500, Transform customParent = null)
        {
            if (objToPool == null) return false;
            List<GameObject> pooledObjects = new List<GameObject>();

            customParent = customParent == null
                ? new GameObject(objToPool.name + "'s pooled list").transform
                : customParent;

            for (int i = 0; i < amountToPool; i++)
            {
                GameObject clone = MonoBehaviour.Instantiate(objToPool, customParent);
                clone.SetActive(false);
                pooledObjects.Add(clone);
            }

            int id = objToPool.GetInstanceID();

            if (PoolerDictionary.ContainsKey(id))
            {
                PoolerDictionary[id].AddRange(pooledObjects);
            }
            else
                PoolerDictionary.Add(id, pooledObjects);

            return true;
        }

        public static GameObject GetPooledObject(GameObject objToFind)
        {
            if (objToFind == null) return null;
            int id = objToFind.GetInstanceID();

            if (PoolerDictionary.ContainsKey(id))
                return PoolerDictionary[id].FirstOrDefault(g => !g.activeSelf);
            if (PoolGameObject(objToFind))
            {
                return GetPooledObject(objToFind);
            }

            return null;
        }
    }
}