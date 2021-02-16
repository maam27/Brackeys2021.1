using System;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ship
{
    [RequireComponent(typeof(ShipInputHandler))]
    public class WeaponSystems : MonoBehaviour
    {
        private ShipInputHandler m_InputHandler;
        public Weapon currentWeapon;

        // Start is called before the first frame update

        private void Awake()
        {
            m_InputHandler = GetComponent<ShipInputHandler>();
        }


        // Update is called once per frame
        void Update()
        {
            currentWeapon.Fire(m_InputHandler.GetFireButton);
        }
    }


    [Serializable]
    public class Weapon
    {
        public float bulletDamage;
        public float fireRate;
        private float m_CurrentTime;

        public float bulletVelocity;


        internal event Action<float, float> ONFireCallback;

        public Weapon()
        {
            ONFireCallback += (a,b) => OnFireCallback();
        }


        public void Fire(bool fireButton)
        {
            m_CurrentTime += Time.deltaTime;
            if (fireButton && m_CurrentTime >= fireRate)
            {
                ONFireCallback?.Invoke(bulletDamage, bulletVelocity);
                m_CurrentTime = 0;
            }
        }

        protected virtual void OnFireCallback()
        {
            Debug.Log("Weapon fired!");
        }
    }
}