using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace GAS
{
    public class AttributeComponent : MonoBehaviour
    {
        [SerializeField] private Attribute[] _attributes;
        private Dictionary<string, Attribute> _attributeMap = new Dictionary<string, Attribute>();

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            foreach (var attr in _attributes)
            {
                _attributeMap.Add(attr.name, attr);
            }
        }
        
        public void AddAttribute(Attribute attribute)
        {
            _attributeMap.Add(attribute.name, attribute);
        }
        
        public void AddAttribute(string name, float baseValue)
        {
            var attribute = new Attribute(name, baseValue);
            _attributeMap.Add(name, attribute);
        }
        
        public void RemoveAttribute(string name)
        {
            _attributeMap.Remove(name);
        }
        
        public Attribute GetAttribute(string name)
        {
            if (_attributeMap.TryGetValue(name, out var attribute))
            {
                return attribute;
            }
            return null;
        }
        
        
    }
}