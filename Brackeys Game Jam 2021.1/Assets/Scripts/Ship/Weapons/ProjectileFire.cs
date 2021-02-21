using Level;
using Scriptable_Asset_Definitions;
using Scriptable_Asset_Definitions.Modifiers;
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
        public Bullet bulletPrefab;
        public float bulletLifetime;

        public override void OnFireBehaviour(Transform barrel, Transform owner,
            WeaponModifier currentModifiers)
        {
            Bullet spawnedBullet = ObjectPooler.GetPooledObject(bulletPrefab);
            spawnedBullet.lifetime = bulletLifetime;
            spawnedBullet.velocity =
                currentModifiers ? bulletVelocity + currentModifiers.bonusVelocity : bulletVelocity;
            spawnedBullet.damage =   currentModifiers ? bulletDamage + currentModifiers.bonusDamage : bulletDamage;
            spawnedBullet.ownerID = owner.GetInstanceID();
            var transform = spawnedBullet.transform;
            if (barrel)
            {
                transform.rotation = barrel.rotation;
                transform.position = barrel.position;
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