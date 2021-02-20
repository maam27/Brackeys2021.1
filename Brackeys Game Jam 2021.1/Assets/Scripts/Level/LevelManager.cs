using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interactivity;
using Level.Enemies;
using Ship;
using UnityEngine;
using UnityEngine.UIElements;
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


        [Header("Asteroid Information")] public List<Asteroid> asteroidPrefabs;
        [Expose] public AsteroidSettings asteroidSettings;
        [Header("Level Information")] public Vector2 levelSize;
        public Vector2 levelOffset;
        public Vector3 spawnPositionOffset;

        [Header("Enemy Spawn Information")] [Expose]
        public List<EnemySpawnProfile> enemySpawnProfiles = new List<EnemySpawnProfile>();

        public bool selectEnemySpawnProfileRandomly;
        public int targetESProfile;
        public bool spawnEnemies;

        [Header("Debug Stuff - Asteroids")] public bool spawnAsteroids = true;
        public bool manuallyControlAsteroidSpawnRate;
        public float spawnRate;
        public int asteroidSpawnAmount;

        [Header("Game Goals")] [Expose]
        public List<TargetWaypointListProfile> targetWaypointProfiles = new List<TargetWaypointListProfile>();

        public bool selectTargetWaypointListProfileRandomly;
        public int targetTWProfile;
        public float proximityDistanceOnTargetWaypoint = 0.1f;
        public GameObject trackerPrefab;


        public bool IsCurrentlyPlaying { private get; set; } = true;
        public ShipController GetPlayerReference { get; private set; }
        public event Action ONGameOver, ONGameComplete;


        private ShipController m_PlayerShip;
        private List<Asteroid> m_RegistedAsteroids = new List<Asteroid>();
        private TargetWaypointListProfile m_SelectedProfile;
        [Space] [SerializeField] private int currentTargetFocus = 0;
        private GameObject m_RegistedTracker;
        private Coroutine m_AsteroidCoroutine, m_EnemyCoroutine;
        

        public void StartGame()
        {
            m_SelectedProfile =
                targetWaypointProfiles[
                    selectTargetWaypointListProfileRandomly
                        ? Random.Range(0, targetWaypointProfiles.Count)
                        : targetTWProfile];


            m_PlayerShip = m_PlayerShip ? m_PlayerShip : FindObjectOfType<ShipController>();
            m_PlayerShip.GetComponent<DamageableComponent>().ONDeathCallback += ONGameOver;
            GetPlayerReference = m_PlayerShip;
            if (spawnAsteroids)
                m_AsteroidCoroutine = StartCoroutine(SpawnAsteroids());

            if (spawnEnemies)
                m_EnemyCoroutine = StartCoroutine(SpawnEnemies());
        }


        private void Update()
        {
            KIllPlayerOnExceedingPosLeveLSize();
            DetectIfPlayerHasReachedCurrentTarget();
            TrackTargetPosition();
        }

        private void TrackTargetPosition()
        {
            if (!m_RegistedTracker)
            {
                m_RegistedTracker = Instantiate(trackerPrefab);
            }
            else if (!m_RegistedTracker.activeSelf)
                m_RegistedTracker.SetActive(true);

            if (currentTargetFocus < m_SelectedProfile.targetWaypointList.Count)
            {
                var position = m_PlayerShip.transform.position;
                Vector3 dir = m_SelectedProfile.targetWaypointList[currentTargetFocus] - position;
                m_RegistedTracker.transform.localRotation = Quaternion.LookRotation(
                    dir,
                    Vector3.up);
                Debug.DrawRay(m_PlayerShip.transform.position, dir);
                m_RegistedTracker.transform.position = position;
            }

            else
            {
                m_RegistedTracker.SetActive(false);
            }
        }

        private void DetectIfPlayerHasReachedCurrentTarget()
        {
            if (!m_PlayerShip) return;
            if (currentTargetFocus < m_SelectedProfile.targetWaypointList.Count && IsObjectInVicinityOfAnotherObject(
                m_PlayerShip.transform.position,
                m_SelectedProfile.targetWaypointList[currentTargetFocus], proximityDistanceOnTargetWaypoint))
            {
                currentTargetFocus++;
                Debug.Log("Reached Goal! Proceeding to the next goal!");
            }

            if (currentTargetFocus >= m_SelectedProfile.targetWaypointList.Count)
            {
                Debug.Log("Game completed");
                OnGameComplete();
                currentTargetFocus = 0;
            }
        }

        private void OnGameComplete()
        {
            if (m_AsteroidCoroutine != null)
                StopCoroutine(m_AsteroidCoroutine);

            if (m_EnemyCoroutine != null)
                StopCoroutine(m_EnemyCoroutine);
            
            
            ONGameComplete?.Invoke();
            m_PlayerShip.GetComponent<DamageableComponent>().ONDeathCallback -= ONGameOver;
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue - new Color(0, 0, 0, 0.75f);

            Gizmos.DrawCube(new Vector3(levelOffset.x, 0, levelOffset.y), new Vector3(levelSize.x, 1, levelSize.y));


            if (asteroidSettings)
            {
                Gizmos.color = Color.green - new Color(0, 0, 0, 0.5f);
                Gizmos.DrawSphere(m_PlayerShip ? m_PlayerShip.transform.position : Vector3.zero,
                    asteroidSettings.spawnRadiusNearPlayer);
            }

            Color pointColor = Color.yellow;
            Color lineColor = Color.cyan;
            Color detectionColor = Color.green;
            for (int p = 0; p < targetWaypointProfiles.Count; p++)
            {
                List<Vector3> targetWaypointList = targetWaypointProfiles[p].targetWaypointList;
                if (targetWaypointList.Count != 0)
                {
                    Vector3 waypointA, waypointB = Vector3.zero;
                    for (var i = 0; i < targetWaypointList.Count; i++)
                    {
                        waypointA = targetWaypointList[i];
                        var isWithinList = i + 1 < targetWaypointList.Count;
                        if (isWithinList)
                            waypointB = targetWaypointList[i + 1];


                        Gizmos.color = pointColor;
                        if (i == 0 || i == targetWaypointList.Count - 1)
                            Gizmos.DrawSphere(waypointA, 1f);
                        else
                            Gizmos.DrawCube(waypointA, Vector3.one / 2f);
                        Gizmos.DrawCube(waypointB, Vector3.one / 2f);
                        Gizmos.color = lineColor;
                        if (waypointB != Vector3.zero)
                            Gizmos.DrawLine(waypointA, waypointB);

                        Gizmos.color = detectionColor - new Color(0, 0, 0, 0.5f);
                        Gizmos.DrawSphere(waypointA, proximityDistanceOnTargetWaypoint);
                    }
                }

                //Debug.Log($"{p} is {(p + 1 >= targetWaypointProfiles.Count ? $"higher than" : "lower than")} {targetWaypointProfiles.Count}!");
                int nextPIndex = p + 1 >= targetWaypointProfiles.Count ? 0 : p + 1;
                pointColor = (targetWaypointProfiles[nextPIndex].pointColor == pointColor
                    ? targetWaypointProfiles[nextPIndex].pointColor = Random.ColorHSV(0, 1, 0, 1, 0, 1, 1, 1)
                    : targetWaypointProfiles[nextPIndex].pointColor);
                lineColor = (targetWaypointProfiles[nextPIndex].lineColor == lineColor
                    ? targetWaypointProfiles[nextPIndex].lineColor = Random.ColorHSV(0, 1, 0, 1, 0, 1, 1, 1)
                    : targetWaypointProfiles[nextPIndex].lineColor);
                detectionColor = (targetWaypointProfiles[nextPIndex].detectionColor == detectionColor
                    ? targetWaypointProfiles[nextPIndex].detectionColor = Random.ColorHSV(0, 1, 0, 1, 0, 1, 1, 1)
                    : targetWaypointProfiles[nextPIndex].detectionColor);
            }
        }


        private BoxCollider m_ChildCollider;

        private void OnValidate()
        {
            m_ChildCollider = m_ChildCollider ? m_ChildCollider : transform.GetChild(0).GetComponent<BoxCollider>();

            m_ChildCollider.size = new Vector3(levelSize.x, 1, levelSize.y);
        }

        #region Asteroid Implementation

        private IEnumerator SpawnAsteroids()
        {
            #region Get Settings

            bool doSettingsExist = asteroidSettings;
            int currentAsteroidAmm = 5;
            float spawnRange = (doSettingsExist ? asteroidSettings.spawnRadiusNearPlayer : 3f);

            StartCoroutine(RemoveExcessAsteroids(spawnRange));

            #endregion

            while (true)
            {
                AsteroidField asteroidField = asteroidSettings.GetARandomAsteroidFieldConfiguration();
                yield return new WaitForSeconds(asteroidField.initialSpawnDelayOnSpawningAsteroidField);
                currentAsteroidAmm = manuallyControlAsteroidSpawnRate ? asteroidSpawnAmount : asteroidField.spawnAmount;
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
                    yield return new WaitForSeconds(manuallyControlAsteroidSpawnRate
                        ? spawnRate
                        : asteroidField.spawnRate);
                }

                yield return new WaitForSeconds(asteroidField.delayToSpawnNextAsteroidField);

                if (!IsCurrentlyPlaying) break;
            }
        }

        private IEnumerator SpawnEnemies()
        {
            while (true)
            {
                EnemySpawnProfile profile = selectEnemySpawnProfileRandomly
                    ? enemySpawnProfiles.GetRandomElement()
                    : enemySpawnProfiles[targetESProfile];
                yield return new WaitForSeconds(profile.initialDelayUntilEnemySpawn);
                for (int i = 0; i < profile.enemySpawnAmm; i++)
                {
                    if (!profile.spawnEnemies) break;
                    BaseEnemy enemy = ObjectPooler.GetPooledObject(profile.enemiesToSpawn.GetRandomElement());
                    Vector3 randomPos;
                    enemy.transform.position =
                        new Vector3((randomPos = Random.onUnitSphere).x, 0, randomPos.z) * profile.enemySpawnRange +
                        m_PlayerShip.transform.position;

                    enemy.gameObject.SetActive(true);

                    yield return new WaitForSeconds(profile.enemySpawnRate);
                }

                yield return new WaitForSeconds(profile.delayToNextEnemySpawnProfile);

                if (!IsCurrentlyPlaying) break;
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
                if (!IsCurrentlyPlaying) break;
            }
        }

        #endregion

        #region Helper Methods

        private void KIllPlayerOnExceedingPosLeveLSize()
        {
            if (!IsPositionWithinZone(m_PlayerShip.transform.position))
            {
                m_PlayerShip.KIllPlayer();
            }
        }


        public bool IsPositionWithinZone(Vector3 currentPos)
        {
            float halfLevelSizeX = levelSize.x / 2f;
            float halfLevelSizeY = levelSize.y / 2f;

            float dist = Vector3.Distance(currentPos, levelOffset);

            return dist < halfLevelSizeX && dist > -halfLevelSizeX || dist < halfLevelSizeY && dist > -halfLevelSizeY;
        }


        public bool IsObjectInVicinityOfAnotherObject(GameObject objectA, GameObject objectB,
            float minDistanceBetweenObjects)
        {
            float distance = Vector3.Distance(objectA.transform.position, objectB.transform.position);
            return distance < minDistanceBetweenObjects;
        }

        public bool IsObjectInVicinityOfAnotherObject(Vector3 objectA, Vector3 objectB,
            float minDistanceBetweenObjects)
        {
            float distance = Vector3.Distance(objectA, objectB);
            return distance <= minDistanceBetweenObjects;
        }

        public Vector3 GetDirectionToPlayer(Transform transform1)
        {
            return (m_PlayerShip.transform.position - transform1.position).normalized;
        }

        #endregion
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

        public static TGameObject GetPooledObject<TGameObject>(TGameObject objToFind, int objectToPoolAmm = 500)
            where TGameObject : MonoBehaviour
        {
            return GetPooledObject(objToFind.gameObject, objectToPoolAmm)?.GetComponent<TGameObject>();
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

    public static class Extensions
    {
        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count - 1)];
        }
    }
}