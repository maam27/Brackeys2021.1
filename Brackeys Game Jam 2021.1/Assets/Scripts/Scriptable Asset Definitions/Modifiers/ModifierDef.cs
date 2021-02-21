using UnityEngine;

namespace Scriptable_Asset_Definitions
{
   
    public abstract class ModifierDef : ScriptableObject
    {
        public abstract void Add<TModifier>(TModifier modifier) where TModifier : ModifierDef;
    }
}