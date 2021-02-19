using System;
using Interactivity;
using Ship;
using UnityEngine;
using UnityEngine.AI;

namespace Level.Enemies
{
    [RequireComponent(typeof(NavMeshAgent), typeof(DamageableComponent))]
    public abstract class BaseEnemy : MonoBehaviour
    {
        protected ShipController PlayerShip;
        protected NavMeshAgent Agent;
        protected Vector3 DirectionToPlayer => (PlayerShip.transform.position - transform.position).normalized;
        protected float DistanceBetweenPlayerAndThis => (PlayerShip.transform.position - transform.position).magnitude;
        public bool useNavMesh = true;

        protected virtual void Awake()
        {
            PlayerShip = LevelManager.PublicAccess.GetPlayerReference;
            Agent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Update()
        {
            EnemyBehaivour();
        }
        
        
        
        
        protected abstract void EnemyBehaivour();

        protected void OnFalseConditionMoveTowardsPlayer(bool condition)
        {
            if (useNavMesh)
            {
                if (!condition)
                    Agent.SetDestination(PlayerShip.transform.position);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}