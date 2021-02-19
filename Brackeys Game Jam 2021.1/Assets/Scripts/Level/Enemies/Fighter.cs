using Ship.Weapons.Weapon_Fire;
using UnityEngine;

namespace Level.Enemies
{
    public class Fighter : BaseEnemy
    {
        public float attackRange;
        public float attackRate;
        public float attackDamage;
        [Space] public Bullet bulletPrefab;

        private float p_Timer;
        protected override void EnemyBehaivour()
        {
            bool isWithinRange = DistanceBetweenPlayerAndThis <= attackRange;
            OnFalseConditionMoveTowardsPlayer(isWithinRange);

            if (isWithinRange)
            {
                transform.rotation = Quaternion.LookRotation(DirectionToPlayer, Vector3.up);
                AttackPlayer();
            }
            
        }

        private void AttackPlayer()
        {
            p_Timer += Time.deltaTime;

            if (p_Timer >= attackRate)
            {
                Bullet bullet = ObjectPooler.GetPooledObject(bulletPrefab);
                bullet.damage = attackDamage;
                bullet.lifetime = 3f;
                bullet.velocity = 50f;
                bullet.ownerID = GetInstanceID();
                bullet.transform.position = transform.position + transform.forward.normalized * 2f;
                bullet.transform.rotation = transform.rotation;
                
                bullet.gameObject.SetActive(true);


                p_Timer = 0;
            }
        }
    }
}