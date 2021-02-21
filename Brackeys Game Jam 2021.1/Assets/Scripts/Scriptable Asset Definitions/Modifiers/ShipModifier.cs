using UnityEngine;

namespace Scriptable_Asset_Definitions.Modifiers
{
    [CreateAssetMenu(fileName = "New Ship Modifier", menuName = "Ship/Create a Modifier", order = 0)]
    public class ShipModifier : ModifierDef
    {
        public float bonusSpeed;
        public float bonusSpeedBoostAmm;
        [Space] public float bonusHealth;
        public override void Add<TModifier>(TModifier modifier)
        {
            if (modifier is ShipModifier shipModifier)
            {
                bonusSpeed += shipModifier.bonusSpeed;
                bonusSpeedBoostAmm += shipModifier.bonusSpeedBoostAmm;
                bonusHealth += bonusHealth;
            }
        }
    }
}