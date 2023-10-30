using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace GAS
{
    public class AttributeComponent : MonoBehaviour
    {
        [SerializeField] private List<AttributeDefine> _attributeDefines;
        [SerializeField] private Attribute[] _attributes;
        private Dictionary<string, Attribute> _attributeMap = new Dictionary<string, Attribute>();

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            foreach (var attr in _attributeDefines)
            {
                _attributeMap.Add(attr.name, new Attribute()
                {
                    attributeDefine = attr,
                    name = attr.name,
                    modifier = new AttributeModifier()
                    {
                        add = 0,
                        multiply = 0,
                        overwrite = 0,
                    }
                });
            }
#if UNITY_EDITOR
            _attributes = _attributeMap.Values.ToArray();
#endif
        }

        public void Tick()
        {
            foreach (var attr in _attributeMap.Values)
            {
                attr.attributeDefine.CalcCurrentValue(attr, _attributeMap);
            }
        }
        
        public void ResetAllAttributeModifier()
        {
            foreach (var attr in _attributeMap.Values)
            {
                attr.modifier.Reset();
            }
        }
        
        public void AddAttribute(Attribute attribute)
        {
            _attributeMap.Add(attribute.name, attribute);
        }
        
        public void RemoveAttribute(string name)
        {
            _attributeMap.Remove(name);
        }
        
        public Attribute GetAttribute(string name)
        {
            if (_attributeMap.TryGetValue(name, out var attribute))
            {
                attribute.attributeDefine.CalcCurrentValue(attribute, _attributeMap);
                return attribute;
            }
            return null;
        }
        
        public bool SetAttributeBaseValue(string name, float value)
        {
            if (_attributeMap.TryGetValue(name, out var attribute))
            {
                attribute.baseValue = value;
                return true;
            }
            return false;
        }
        
        public bool SetAttributeModifier(string name, AttributeModifier modifier)
        {
            if (_attributeMap.TryGetValue(name, out var attribute))
            {
                attribute.modifier.Combine(modifier);
                return true;
            }
            return false;
        }
    }
}