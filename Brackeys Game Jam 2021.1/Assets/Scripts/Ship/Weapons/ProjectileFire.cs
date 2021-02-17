using Level;
using Ship.Weapons.Weapon_Fire;
using UnityEngine;

namespace Ship.Weapons
{
    [CreateAssetMenu(fileName = "New Projectile Fire Behaviour",
        menuName = "Weapons/Weapons/Define Weapon Fire/Projectile Fire", order = 0)]
    public class ProjectileFire : BaseFireBehaviour
    {
        public float bulletDamage;
        public float bulletVelocity;
        public GameObject bulletPrefab;
        public float bulletLifetime;

        public override void OnFireBehaviour(Transform owner)
        {
            Bullet spawnedBullet = ObjectPooler.GetPooledObject(bulletPrefab).GetComponent<Bullet>();
            spawnedBullet.lifetime = bulletLifetime;
            spawnedBullet.velocity = bulletVelocity;
            spawnedBullet.damage = bulletDamage;
            var transform = spawnedBullet.transform;
            if (owner)
            {
                transform.rotation = owner.rotation;
                transform.position = owner.position;
            }
            else
            {
                transform.rotation = Quaternion.identity;
                transform.position = Vector3.zero;
            }
            spawnedBullet.gameObject.SetActive(true);
        }
    }
}