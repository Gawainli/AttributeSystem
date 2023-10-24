using System.Collections.Generic;
using UnityEngine;

namespace GAS
{
    public class BuffComponent : MonoBehaviour
    {
        public AttributeComponent attributeComponent;

        private List<BuffHandle> _appliedBuffHandles = new List<BuffHandle>();
        private Dictionary<Buff, BuffHandle> _buffHandleMap = new Dictionary<Buff, BuffHandle>();

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
                if (handle.buff.durationType == DurationType.Instant)
                {
                    continue;
                }

                handle.TickDuration(deltaTime);
                
                if (handle.buff.IsPeriodic())
                {
                    if (handle.TickPeriod(deltaTime))
                    {
                        handle.buff.OnPeriod(handle);
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
                if (handle.buff.durationType == DurationType.Infinite)
                {
                    continue;
                }

                if (handle.durationRemaining <= 0)
                {
                    handle.buff.OnRemove(handle);
                    _appliedBuffHandles.RemoveAt(i);
                }
            }
        }

        public BuffHandle MakeBuffHandle(Buff buff, float level = 1)
        {
            return BuffHandle.Create(buff, this, level);
        }

        public bool ApplyBuffToSelf(BuffHandle handle)
        {
            if (handle == null)
            {
                return false;
            }

            if (!CheckTagOk(handle))
            {
                return false;
            }

            handle.buff.OnStart(handle);
            if (_buffHandleMap.TryGetValue(handle.buff, out var exist))
            {
                switch (exist.buff.overlapType)
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
            if (handle.buff.durationType == DurationType.Instant)
            {
                ApplyBuffToBaseValue(handle);
            }

            _appliedBuffHandles.Add(handle);
            return true;
        }

        private void ApplyBuffToAttrModifier(BuffHandle handle)
        {
            var attrModifier = new AttributeModifier();
            foreach (var bm in handle.buff.modifiers)
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

        private void ApplyBuffToBaseValue(BuffHandle handle)
        {
            foreach (var bm in handle.buff.modifiers)
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

        private bool CheckTagOk(BuffHandle handle)
        {
            if (handle.buff.tag == 0)
            {
                return true;
            }

            return true;
        }
    }
}