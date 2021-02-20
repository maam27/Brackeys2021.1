using System.Collections.Generic;
using Level.Enemies;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "New Enemy Spawn Profile", menuName = "Level Manager/Enemy/Create Profile for spawning enemies", order = 0)]
    public class EnemySpawnProfile : ScriptableObject
    {
        public List<BaseEnemy> enemiesToSpawn;
        public int enemySpawnAmm;
        public float enemySpawnRate;
        public float initialDelayUntilEnemySpawn;
        public float delayToNextEnemySpawnProfile;
        public float enemySpawnRange;
        public bool spawnEnemies = true;
    }
}