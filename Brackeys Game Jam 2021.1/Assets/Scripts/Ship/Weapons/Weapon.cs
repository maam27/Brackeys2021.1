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

        public void Fire(bool fireButton, Transform holder = null)
        {
            m_CurrentTime += Time.deltaTime;
            if (fireButton && m_CurrentTime >= fireRate)
            {
                if (fireBehaivourCallback)
                    fireBehaivourCallback.OnFireBehaviour(holder);
                m_CurrentTime = 0;
            }
        }
    }
}