using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GAS
{
    [CreateAssetMenu (menuName = "GAS/Attribute")]
    public class Attribute : ScriptableObject
    {
        public string name;
        public float baseValue;
        
        private List<AttributeModifier> modifiers = new List<AttributeModifier>();
        private bool dirty = true;
        private float _currentValue;
        
        public Attribute(string name, float baseValue)
        {
            this.name = name;
            this.baseValue = baseValue;
        }
        
        public float CurrentValue
        {
            get
            {
                if (dirty)
                {
                    CalcCurrentValue();
                }

                return _currentValue;
            }
        }
        
        public void AddModifier(AttributeModifier modifier)
        {
            modifiers.Add(modifier);
            dirty = true;
        }

        private void CalcCurrentValue()
        {
            _currentValue = baseValue;
            foreach (var modifier in modifiers)
            {
                _currentValue += modifier.add;
                _currentValue *= modifier.multiply;
                _currentValue = modifier.overwrite;
            }
            dirty = false;
        }
    }
}