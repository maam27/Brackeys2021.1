using UnityEngine;

namespace Ship.Weapons
{
    
    public abstract class BaseFireBehaviour : ScriptableObject
    {
        public abstract void OnFireBehaviour(Transform owner);
    }
}