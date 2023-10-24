using System;
using UnityEngine;

namespace GAS
{
    public enum AttributeCompareType
    {
        GreaterThan,
        LessThan,
        EqualTo,
        NotEqualTo,
    }

    [CreateAssetMenu(fileName = "AttributeCompareMagnitude", menuName = "GAS/Magnitude/AttributeCompareMagnitude")]
    public class BaseAttrCompareMagnitude : BaseMagnitude
    {
        public string baseAttributeName;
        public AttributeCompareType compareType;
        public float compareValue;
        public float resultValue;

        public override void Initialise(GameplayEffectHandle handle)
        {
        }

        public override float CalculateMagnitude(GameplayEffectHandle handle)
        {
            var attr = handle.parent.attributeComponent.GetAttribute(baseAttributeName);
            if (attr == null)
            {
                return 0;
            }

            var compareResult = false;
            switch (compareType)
            {
                case AttributeCompareType.EqualTo:
                    compareResult = Math.Abs(attr.currentValue - compareValue) < 0.0001f;
                    break;
                case AttributeCompareType.NotEqualTo:
                    compareResult = Math.Abs(attr.currentValue - compareValue) > 0.0001f;
                    break;
                case AttributeCompareType.GreaterThan:
                    compareResult = attr.currentValue > compareValue;
                    break;
                case AttributeCompareType.LessThan:
                    compareResult = attr.currentValue < compareValue;
                    break; 
            }

            return compareResult ? resultValue : 0;
        }
    }
}