using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "New Asteroid Settings", menuName = "Level Manager/Settings/Asteroid", order = 0)]
    public class AsteroidSettings : ScriptableObject
    {
        [Header("Asteroid Spawn Amount information")]
        public int startingAsteroidAmm;
        public int maxAsteroidAmm;
        public Vector2Int asteroidIncrementRange;
        
        [Header("Minimum Spawn Rate")]
        public float minSpawnDelay;
        public Vector2 minSpawnDelayDecrementRange;
        
        [Header("Maximum Spawn Rate")]
        public float maxSpawnDelay;
        public Vector2 maxSpawnDelayDecrementRange;
        
        [Header("Other")]
        public float spawnRadiusNearPlayer;
    }
}