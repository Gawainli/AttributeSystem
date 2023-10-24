using UnityEngine;

namespace GAS
{
    [CreateAssetMenu(fileName = "SimpleFloatMagnitude", menuName = "GAS/Magnitude/SimpleFloatMagnitude")]
    public class SimpleFloatMagnitude : BaseMagnitude
    {
        public float value;
        public override void Initialise(GameplayEffectHandle handle)
        {
        }

        public override float CalculateMagnitude(GameplayEffectHandle handle)
        {
            return value;
        }
    }
}