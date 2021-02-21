using UnityEngine;

namespace Scriptable_Asset_Definitions.Modifiers
{
    [CreateAssetMenu(fileName = "New Weapon Modifier", menuName = "Weapons/Create a Modifier", order = 0)]
    public class WeaponModifier : ModifierDef
    {
        public float bonusDamage, bonusFireRate, bonusVelocity;


        public override void Add<TModifier>(TModifier modifier)
        {
            if (modifier is WeaponModifier weaponModifiers)
            {
                bonusDamage += weaponModifiers.bonusDamage;
                bonusFireRate += weaponModifiers.bonusFireRate;
                bonusVelocity += weaponModifiers.bonusVelocity;
            }
        }
    }
}