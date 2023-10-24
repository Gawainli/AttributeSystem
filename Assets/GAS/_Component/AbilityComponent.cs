using System.Collections.Generic;
using UnityEngine;

namespace GAS
{
    public class AbilityComponent : MonoBehaviour
    {
        public AttributeComponent attributeComponent;

        private List<GameplayEffectHandle> _appliedBuffHandles = new List<GameplayEffectHandle>();
        private Dictionary<GameplayEffect, GameplayEffectHandle> _buffHandleMap = new Dictionary<GameplayEffect, GameplayEffectHandle>();

        public void Tick(float deltaTime)
        {
            attributeComponent.ResetAllAttributeModifier();
            TickBuffHandles(deltaTime);
            CleanBuffHandles();
        }


        private void TickBuffHandles(float deltaTime)
        {
            for (int i = 0; i < _appliedBuffHandles.Count; i++)
            {
                var handle = _appliedBuffHandles[i];
                if (handle.gameplayEffect.durationType == DurationType.Instant)
                {
                    continue;
                }

                handle.TickDuration(deltaTime);
                
                if (handle.gameplayEffect.IsPeriodic())
                {
                    if (handle.TickPeriod(deltaTime))
                    {
                        handle.gameplayEffect.OnPeriod(handle);
                        ApplyBuffToBaseValue(handle);
                    }
                }
                else
                {
                    ApplyBuffToAttrModifier(handle);
                }
            }
        }

        private void CleanBuffHandles()
        {
            for (var i = _appliedBuffHandles.Count - 1; i >= 0; i--)
            {
                var handle = _appliedBuffHandles[i];
                if (handle.gameplayEffect.durationType == DurationType.Infinite)
                {
                    continue;
                }

                if (handle.durationRemaining <= 0)
                {
                    handle.gameplayEffect.OnRemove(handle);
                    _appliedBuffHandles.RemoveAt(i);
                }
            }
        }

        public GameplayEffectHandle MakeBuffHandle(GameplayEffect gameplayEffect, float level = 1)
        {
            return GameplayEffectHandle.Create(gameplayEffect, this, level);
        }

        public bool ApplyBuffToSelf(GameplayEffectHandle handle)
        {
            if (handle == null)
            {
                return false;
            }

            if (!CheckTagOk(handle))
            {
                return false;
            }

            handle.gameplayEffect.OnStart(handle);
            if (_buffHandleMap.TryGetValue(handle.gameplayEffect, out var exist))
            {
                switch (exist.gameplayEffect.overlapType)
                {
                    case OverlapType.Overlap:
                        break;
                    case OverlapType.Refresh:
                        exist.durationRemaining = exist.totalDuration;
                        break;
                    case OverlapType.Ignore:
                        return false;
                }
            }

            handle.parent = this;
            if (handle.gameplayEffect.durationType == DurationType.Instant)
            {
                ApplyBuffToBaseValue(handle);
            }

            _appliedBuffHandles.Add(handle);
            return true;
        }

        private void ApplyBuffToAttrModifier(GameplayEffectHandle handle)
        {
            var attrModifier = new AttributeModifier();
            foreach (var bm in handle.gameplayEffect.modifiers)
            {
                var attr = attributeComponent.GetAttribute(bm.attributeName);
                if (attr == null)
                {
                    continue;
                }

                var value = bm.magnitude.CalculateMagnitude(handle);
                switch (bm.opType)
                {
                    case OpType.Add:
                        attrModifier.add += value;
                        break;
                    case OpType.Multiply:
                        attrModifier.multiply *= value;
                        break;
                    case OpType.Overwrite:
                        attrModifier.overwrite = value;
                        break;
                }

                attributeComponent.SetAttributeModifier(attr.name, attrModifier);
            }
        }

        private void ApplyBuffToBaseValue(GameplayEffectHandle handle)
        {
            foreach (var bm in handle.gameplayEffect.modifiers)
            {
                var attr = attributeComponent.GetAttribute(bm.attributeName);
                if (attr == null)
                {
                    continue;
                }

                var value = bm.magnitude.CalculateMagnitude(handle);
                switch (bm.opType)
                {
                    case OpType.Add:
                        attr.baseValue += value;
                        break;
                    case OpType.Multiply:
                        attr.baseValue *= 1 + value;
                        break;
                    case OpType.Overwrite:
                        attr.baseValue = value;
                        break;
                }

                attributeComponent.SetAttributeBaseValue(attr.name, attr.baseValue);
            }
        }

        private bool CheckTagOk(GameplayEffectHandle handle)
        {
            if (handle.gameplayEffect.tag == 0)
            {
                return true;
            }

            return true;
        }
    }
}