using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level
{
    [CreateAssetMenu(fileName = "New Asteroid Settings", menuName = "Level Manager/Settings/Asteroid", order = 0)]
    public class AsteroidSettings : ScriptableObject
    {
        [Header("Asteroid Spawn Amount information")]
        public int startingAsteroidAmm;
        public int maxAsteroidAmm;


        [Header("Different Asteroid Field Configurations")]
        public List<AsteroidField> asteroidFields = new List<AsteroidField>();
        
        [Header("Other")]
        public float spawnRadiusNearPlayer;


        public AsteroidField GetARandomAsteroidFieldConfiguration()
        {
            return asteroidFields[Random.Range(0, asteroidFields.Count - 1)];
        }
    }

    [Serializable]
    public struct AsteroidField
    {
        public float spawnRate;
        public int spawnAmount;
        public float delayToSpawnNextAsteroidField;
    }
}