using System;
using Interactivity;
using Ship;
using Ship.Weapons;
using UnityEngine;
using UnityEngine.AI;

namespace Level.Enemies
{
    [RequireComponent(typeof(DamageableComponent), typeof(Weapon))]
    public abstract class BaseEnemy : MonoBehaviour
    {
        protected ShipController PlayerShip;
        protected NavMeshAgent Agent;
        protected Rigidbody Physics;
        protected Weapon CurrentWeapon;

        protected Vector3 DirectionToPlayer =>
            PlayerShip ? (PlayerShip.transform.position - transform.position).normalized : Vector3.zero;

        protected float DistanceBetweenPlayerAndThis =>
            PlayerShip ? (PlayerShip.transform.position - transform.position).magnitude : 0;

        public bool useNavMesh = true;
        public float speed = 5f;

        protected void Start()
        {
            PlayerShip = LevelManager.PublicAccess.GetPlayerReference;
            Physics = GetComponent<Rigidbody>();
            Agent = GetComponent<NavMeshAgent>();
        }


        protected void OnFalseConditionMoveTowardsPlayer(bool condition, Action onTrueConditionCallback = null)
        {
            if (!condition)
                if (useNavMesh && Agent)
                {
                    Agent.SetDestination(PlayerShip.transform.position);
                }
                else if (Physics)
                {
                    Physics.velocity = DirectionToPlayer * (speed * 100f * Time.fixedDeltaTime);
                }
            else
                {
                    onTrueConditionCallback?.Invoke();
                }
        }
    }
}