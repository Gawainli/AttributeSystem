using System.Collections.Generic;
using UnityEngine;

namespace GAS
{
    [CreateAssetMenu(fileName = "AttributeDefine", menuName = "GAS/AttributeDefine")]
    public class AttributeDefine : ScriptableObject
    {
        public string name;
        public void CalcCurrentValue(Attribute attribute, Dictionary<string, Attribute> attributeMap)
        {
            attribute.currentValue = attribute.baseValue;
            attribute.currentValue += attribute.modifier.add;
            attribute.currentValue *= 1f + attribute.modifier.multiply;
            if (attribute.modifier.overwrite != 0)
            {
                attribute.currentValue = attribute.modifier.overwrite;
            }
        }
    }
}