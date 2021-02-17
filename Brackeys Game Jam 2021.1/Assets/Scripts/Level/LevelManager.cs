using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ship;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        #region Singleton

        private static LevelManager _ins;


        public static LevelManager PublicAccess
        {
            get
            {
                _ins = _ins == null
                    ? FindObjectOfType<LevelManager>() ??
                      new GameObject("Level Manager").AddComponent<LevelManager>()
                    : _ins;

                return _ins;
            }
        }

        #endregion


        private ShipController m_PlayerShip;


        [Header("Asteroid Information")] public List<Asteroid> asteroidPrefabs;
        public float startingAsteroidAmm, maxAsteroidAmm;
        public float asteroidAmmRate;
        [Range(1, 0.01f)] public float minAsteroidSpawnRate, maxAsteroidSpawnRate;

        [Space] public Vector2 levelSize;


        private void Awake()
        {
            m_PlayerShip = FindObjectOfType<ShipController>();
            StartCoroutine(SpawnAsteroids());
        }


        private void Update()
        {
            KIllPlayerOnExceedingPosLeveLSize();
        }

        private IEnumerator SpawnAsteroids()
        {
            float currentAsteroidAmm = 5f;
            float minDelay = 5f, maxDelay = 10f;
            while (true)
            {
                for (int i = 0; i < currentAsteroidAmm; i++)
                {
                    Asteroid obj = ObjectPooler
                        .GetPooledObject(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count - 1)].gameObject)
                        .GetComponent<Asteroid>();
                    obj.gameObject.SetActive(true);
                    yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
                }

                maxDelay -= Random.Range(1, 3);
                minDelay -= Random.Range(1, 3);
                currentAsteroidAmm += Random.Range(2, 4);
                yield return new WaitForEndOfFrame();
            }

            yield return null;
        }

        private void KIllPlayerOnExceedingPosLeveLSize()
        {
            if (IsFloatWithinLimits(m_PlayerShip.transform.position.x, levelSize.x) ||
                IsFloatWithinLimits(m_PlayerShip.transform.position.z, levelSize.y))
            {
                m_PlayerShip.KIllPlayer();
            }
        }


        public bool IsFloatWithinLimits(float currentFloat, float limit)
        {
            return currentFloat >= limit | currentFloat <= -limit;
        }


        public bool IsObjectInVicinityOfAnotherObject(GameObject objectA, GameObject objectB,
            float minDistanceBetweenObjects)
        {
            float distance = Vector3.Distance(objectA.transform.position, objectB.transform.position);
            return distance >= minDistanceBetweenObjects;
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue - new Color(0, 0, 0, 0.75f);

            Gizmos.DrawCube(Vector3.zero, new Vector3(levelSize.x, 1, levelSize.y));
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