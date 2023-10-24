using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GAS
{
    // [CreateAssetMenu(menuName = "GAS/Attribute")]
    [Serializable]
    public class Attribute
    {
        public AttributeDefine attributeDefine;
        public string name;
        public float baseValue;
        public float currentValue;

        public AttributeModifier modifier;
    }
}