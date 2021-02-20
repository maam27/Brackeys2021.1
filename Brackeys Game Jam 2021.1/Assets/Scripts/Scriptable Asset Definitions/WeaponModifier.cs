using UnityEngine;

namespace Scriptable_Asset_Definitions
{
    [CreateAssetMenu(fileName = "New Weapon Modifier", menuName = "Weapons/Create a Modifier", order = 0)]
    public class WeaponModifier : ScriptableObject
    {
        public float bonusDamage, bonusFireRate, bonusVelocity;

        public void Add(WeaponModifier weaponModifiers)
        {
            bonusDamage += weaponModifiers.bonusDamage;
            bonusFireRate += weaponModifiers.bonusFireRate;
            bonusVelocity += weaponModifiers.bonusVelocity;
        }
    }
}