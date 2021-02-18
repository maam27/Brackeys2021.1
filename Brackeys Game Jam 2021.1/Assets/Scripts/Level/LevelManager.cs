using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ship;
using UnityEngine;
using Utility.Attributes;
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
        private List<Asteroid> m_RegistedAsteroids = new List<Asteroid>();


        [Header("Asteroid Information")] public List<Asteroid> asteroidPrefabs;
        [Expose] public AsteroidSettings asteroidSettings;

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
            #region Get Settings

            bool doSettingsExist = asteroidSettings;
            int currentAsteroidAmm = 5;
            float minDelay = doSettingsExist ? asteroidSettings.minSpawnDelay : 5f,
                maxDelay = doSettingsExist ? asteroidSettings.maxSpawnDelay : 10f;
            float spawnRange = (doSettingsExist ? asteroidSettings.spawnRadiusNearPlayer : 3f);

            StartCoroutine(RemoveExcessAsteroids(spawnRange));

            Vector2 minDelayDecrement =
                doSettingsExist ? asteroidSettings.minSpawnDelayDecrementRange : new Vector2(1, 3);
            Vector2 maxDelayDecrement =
                doSettingsExist ? asteroidSettings.maxSpawnDelayDecrementRange : new Vector2(1, 3);
            Vector2Int asteroidIncrementRange =
                doSettingsExist ? asteroidSettings.asteroidIncrementRange : new Vector2Int(2, 4);

            #endregion

            while (true)
            {
                for (int i = 0; i < currentAsteroidAmm; i++)
                {
                    Asteroid obj = ObjectPooler
                        .GetPooledObject(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count - 1)].gameObject, 1000)?
                        .GetComponent<Asteroid>();
                    if (!obj)
                    {
                        yield return new WaitForEndOfFrame();
                        continue;
                    }

                    obj.gameObject.SetActive(true);
                    Vector3 randomPos;
                    var transform1 = obj.transform;
                    transform1.position =
                        new Vector3((randomPos = Random.onUnitSphere).x, 0, randomPos.z) * spawnRange +
                        m_PlayerShip.transform.position;
                    obj.transform.rotation = Quaternion.LookRotation(GetDirectionToPlayer(transform1), Vector3.up);

                    obj.InitDirection = transform1.forward * (Random.Range(obj.minVelocity, obj.maxVelocity) * 100f);
                    obj.InitDirection = new Vector3(obj.InitDirection.x, 0, obj.InitDirection.z);

                    m_RegistedAsteroids.Add(obj);
                    yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
                }


                maxDelay -= Random.Range(minDelayDecrement.x, minDelayDecrement.y);
                minDelay -= Random.Range(maxDelayDecrement.x, maxDelayDecrement.y);
                currentAsteroidAmm += Random.Range(asteroidIncrementRange.x, asteroidIncrementRange.y);
            }

            yield return null;
        }

        private IEnumerator RemoveExcessAsteroids(float spawnRange)
        {
            while (true)
            {
                List<Asteroid> foundAsteroinds = m_RegistedAsteroids.FindAll(a =>
                    Vector3.Distance(a.transform.position, m_PlayerShip.transform.position) > spawnRange);

                foreach (var asteroid in foundAsteroinds)
                {
                    asteroid.gameObject.SetActive(false);
                    m_RegistedAsteroids.Remove(asteroid);
                    yield return new WaitForEndOfFrame();
                }

                yield return null;
            }
        }

        private void KIllPlayerOnExceedingPosLeveLSize()
        {
            if (IsFloatNotWithinLimits(m_PlayerShip.transform.position.x, levelSize.x) &&
                IsFloatNotWithinLimits(m_PlayerShip.transform.position.z, levelSize.y))
            {
                m_PlayerShip.KIllPlayer();
            }
        }


        public bool IsFloatNotWithinLimits(float currentFloat, float limit)
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


            if (!asteroidSettings || !m_PlayerShip) return;

            Gizmos.color = Color.green - new Color(0, 0, 0, 0.5f);
            Gizmos.DrawSphere(m_PlayerShip.transform.position, asteroidSettings.spawnRadiusNearPlayer);
        }

        public Vector3 GetDirectionToPlayer(Transform transform1)
        {
            return (m_PlayerShip.transform.position - transform1.position).normalized;
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

        public static GameObject GetPooledObject(GameObject objToFind, int objectToPoolAmm = 500)
        {
            if (objToFind == null) return null;
            int id = objToFind.GetInstanceID();

            if (PoolerDictionary.ContainsKey(id))
                return PoolerDictionary[id].FirstOrDefault(g => !g.activeSelf);
            if (PoolGameObject(objToFind, objectToPoolAmm))
            {
                return GetPooledObject(objToFind);
            }

            return null;
        }
    }
}