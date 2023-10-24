using UnityEngine;

namespace GAS
{
    [CreateAssetMenu(fileName = "SimpleFloatMagnitude", menuName = "GAS/Magnitude/SimpleFloatMagnitude")]
    public class SimpleFloatMagnitude : BaseMagnitude
    {
        public float value;
        public override void Initialise(BuffHandle handle)
        {
        }

        public override float CalculateMagnitude(BuffHandle handle)
        {
            return value;
        }
    }
}