
using UnityEngine;

namespace GAS
{
    public abstract class BaseMagnitude : ScriptableObject
    {
        public abstract void Initialise(GameplayEffectHandle handle);
        public abstract float CalculateMagnitude(GameplayEffectHandle handle);
    }
}