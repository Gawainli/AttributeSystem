using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GAS
{
    // [CreateAssetMenu(menuName = "GAS/Attribute")]
    [Serializable]
    public class Attribute
    {
        public string name;
        public float baseValue;
        public float currentValue;

        public AttributeModifier modifier;

        public Attribute(string name, float baseValue)
        {
            this.name = name;
            this.baseValue = baseValue;
            modifier = new AttributeModifier();
        }

        public void ResetModifier()
        {
            modifier.Reset();
        }

        public void CalcCurrentValue()
        {
            currentValue = baseValue;
            currentValue += modifier.add;
            currentValue *= 1 + modifier.multiply;
            if (modifier.overwrite != 0)
            {
                currentValue = modifier.overwrite;
            }
        }
        
        
    }
}