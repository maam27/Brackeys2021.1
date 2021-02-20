using Level;
using Scriptable_Asset_Definitions;
using UnityEngine;
using Utility.Attributes;

namespace Ship.Weapons
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Create Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        public float fireRate;
        private float m_CurrentTime;

        [Expose] public BaseFireBehaviour fireBehaivourCallback;

        public void Fire(bool fireButton, Transform barrel = null, Transform owner = null,
            WeaponModifier currentModifiers = default)
        {
            m_CurrentTime += Time.deltaTime;
            float trueFireRate = currentModifiers ?  fireRate - currentModifiers.bonusFireRate : fireRate;
            Mathf.Clamp(trueFireRate, 0.05f, float.MaxValue);
            if (fireButton && m_CurrentTime >= trueFireRate)
            {
                if (fireBehaivourCallback)
                    fireBehaivourCallback.OnFireBehaviour(barrel, owner, currentModifiers);
                m_CurrentTime = 0;
            }
        }
    }
}