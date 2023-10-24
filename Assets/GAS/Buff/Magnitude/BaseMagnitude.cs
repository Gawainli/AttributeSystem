
using UnityEngine;

namespace GAS
{
    public abstract class BaseMagnitude : ScriptableObject
    {
        public abstract void Initialise(BuffHandle handle);
        public abstract float CalculateMagnitude(BuffHandle handle);
    }
}