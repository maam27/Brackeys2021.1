using Level;
using Scriptable_Asset_Definitions;
using Scriptable_Asset_Definitions.Modifiers;
using UnityEngine;

namespace Ship.Weapons
{
    
    public abstract class BaseFireBehaviour : ScriptableObject
    {
        public abstract void OnFireBehaviour(Transform barrel, Transform owner,
            WeaponModifier weaponModifier);
    }
}