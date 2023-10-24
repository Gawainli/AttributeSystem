using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GAS
{
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