using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GAS
{
    public class AbilityComponent : MonoBehaviour
    {
        public AttributeComponent attributeComponent;

        private List<GameplayEffectHandle> _appliedGameplayEffectHandles = new List<GameplayEffectHandle>();

        private Dictionary<GameplayEffect, GameplayEffectHandle> _gameplayEffectHandleMap =
            new Dictionary<GameplayEffect, GameplayEffectHandle>();

        private Dictionary<string, AbstractAbilityHandle> _addedAbilityHandles =
            new Dictionary<string, AbstractAbilityHandle>();

        private List<AbstractAbilityHandle> _activeAbilityHandles = new List<AbstractAbilityHandle>();

        public void Tick(float deltaTime)
        {
            attributeComponent.ResetAllAttributeModifier();
            TickActiveAbility(deltaTime);
            TickGameplayEffectHandles(deltaTime);
            CleanGameplayEffectHandles();
        }

        #region Ability
        
        public AbstractAbilityHandle AddAbility(AbilityDefine abilityDefine)
        {
            if (abilityDefine == null)
            {
                return null;
            }

            var abilityHandle = abilityDefine.CreateAbilityHandle(this);
            AddAbility(abilityHandle);
            return abilityHandle;
        }

        public void AddAbility(AbstractAbilityHandle abilityHandle)
        {
            if (abilityHandle == null)
            {
                return;
            }

            abilityHandle.owner = this;
            abilityHandle.Awake();
            _addedAbilityHandles.Add(abilityHandle.abilityDefine.name, abilityHandle);
        }

        public AbstractAbilityHandle GetAbilityHandle(string abilityName)
        {
            if (_addedAbilityHandles.TryGetValue(abilityName, out var abilityHandle))
            {
                return abilityHandle;
            }

            return null;
        }

        public AbstractAbilityHandle GetAbilityHandle(AbilityDefine ability)
        {
            if (_addedAbilityHandles.TryGetValue(ability.name, out var abilityHandle))
            {
                return abilityHandle;
            }

            return null;
        }

        public bool ActiveAbility(AbstractAbilityHandle abilityHandle)
        {
            if (abilityHandle == null)
            {
                return false;
            }

            if (!_addedAbilityHandles.ContainsValue(abilityHandle))
            {
                return false;
            }

            if (abilityHandle.CanActivate())
            {
                abilityHandle.Active();
                _activeAbilityHandles.Add(abilityHandle);
                return true;
            }
            return false;
        }

        public bool CancelAbility(AbstractAbilityHandle abilityHandle)
        {
            if (abilityHandle == null || !_activeAbilityHandles.Contains(abilityHandle))
            {
                return false;
            }
            abilityHandle.End();
            _activeAbilityHandles.Remove(abilityHandle);
            return true;
        }

        public bool CancelAbility(AbilityDefine ability)
        {
            var abilityHandle = GetAbilityHandle(ability);
            if (abilityHandle == null)
            {
                return false;
            }

            return CancelAbility(abilityHandle);
        }


        private void TickActiveAbility(float deltaTime)
        {
            for (int i = 0; i < _activeAbilityHandles.Count; i++)
            {
                var abHandle = _activeAbilityHandles[i];
                if (abHandle == null)
                {
                    continue;
                }

                abHandle.Tick(deltaTime);
            }
        }

        #endregion

        #region GameplayEffect

        private void TickGameplayEffectHandles(float deltaTime)
        {
            for (int i = 0; i < _appliedGameplayEffectHandles.Count; i++)
            {
                var handle = _appliedGameplayEffectHandles[i];
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
                        ApplyGameplayEffectToBaseValue(handle);
                    }
                }
                else
                {
                    ApplyGameplayEffectToAttrModifier(handle);
                }
            }
        }

        private void CleanGameplayEffectHandles()
        {
            for (var i = _appliedGameplayEffectHandles.Count - 1; i >= 0; i--)
            {
                var handle = _appliedGameplayEffectHandles[i];
                if (handle.gameplayEffect.durationType == DurationType.Infinite)
                {
                    continue;
                }

                if (handle.durationRemaining <= 0)
                {
                    handle.gameplayEffect.OnRemove(handle);
                    _appliedGameplayEffectHandles.RemoveAt(i);
                    _gameplayEffectHandleMap.Remove(handle.gameplayEffect);
                }
            }
        }

        public GameplayEffectHandle MakeGameplayEffectHandle(GameplayEffect gameplayEffect, float level = 1)
        {
            return GameplayEffectHandle.Create(gameplayEffect, this, level);
        }

        public bool ApplyGameplayEffectToSelf(GameplayEffectHandle handle)
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
            if (_gameplayEffectHandleMap.TryGetValue(handle.gameplayEffect, out var exist))
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
            ApplyGameplayEffectGrantedAbility(handle);
            if (handle.gameplayEffect.durationType == DurationType.Instant)
            {
                ApplyGameplayEffectToBaseValue(handle);
            }

            _appliedGameplayEffectHandles.Add(handle);
            _gameplayEffectHandleMap.TryAdd(handle.gameplayEffect, handle);
            return true;
        }

        private void ApplyGameplayEffectGrantedAbility(GameplayEffectHandle handle)
        {
            foreach (var ad in handle.gameplayEffect.grantedAbilityDefines)
            {
                var adHandle = ad.CreateAbilityHandle(this);
                AddAbility(adHandle);
            }
        }

        private void ApplyGameplayEffectToAttrModifier(GameplayEffectHandle handle)
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

        private void ApplyGameplayEffectToBaseValue(GameplayEffectHandle handle)
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

        #endregion
    }
}