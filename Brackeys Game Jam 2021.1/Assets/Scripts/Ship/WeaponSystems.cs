using System;
using Input;
using Ship.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility.Attributes;

namespace Ship
{
    [RequireComponent(typeof(ShipInputHandler))]
    public class WeaponSystems : MonoBehaviour
    {
        private ShipInputHandler m_InputHandler;
        [Expose] public Weapon currentWeapon;
        public Transform weaponBarrel;

        // Start is called before the first frame update

        private void Awake()
        {
            m_InputHandler = GetComponent<ShipInputHandler>();
        }


        // Update is called once per frame
        void Update()
        {
            currentWeapon.Fire(m_InputHandler.GetFireButton, weaponBarrel, transform);
        }
    }
}