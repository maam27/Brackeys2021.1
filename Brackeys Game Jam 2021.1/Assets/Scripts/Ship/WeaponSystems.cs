using System;
using Input;
using Level;
using Scriptable_Asset_Definitions;
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
        [Expose] public WeaponModifier currentModifiers;
        public Transform weaponBarrel;


        // Start is called before the first frame update

        private void Awake()
        {
            m_InputHandler = GetComponent<ShipInputHandler>();
            currentModifiers = ScriptableObject.CreateInstance<WeaponModifier>();
        }


        public void ResetToDefault()
        {
            currentWeapon = Resources.Load<Weapon>("Weapons/StartingWeapon");
        }


        // Update is called once per frame
        void Update()
        {
            currentWeapon.Fire(m_InputHandler.GetFireButton, weaponBarrel, transform, currentModifiers);
        }
    }
}