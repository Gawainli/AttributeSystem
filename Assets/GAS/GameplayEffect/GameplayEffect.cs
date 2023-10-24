using System.Collections.Generic;
using UnityEngine;

namespace GAS
{
    public enum DurationType
    {
        Infinite,
        Instant,
        Duration,
    }

    public enum OverlapType
    {
        Overlap,
        Refresh,
        Ignore,
    }

    [CreateAssetMenu(menuName = "GAS/Buff")]
    public class GameplayEffect : ScriptableObject
    {
        public float duration;
        public float period;
        public bool executeImmediate;
        public int tag;
        public DurationType durationType;
        public OverlapType overlapType;

        public List<EffectModifier> modifiers = new List<EffectModifier>();
        
        public bool IsPeriodic()
        {
            return period > 0.0001f;
        }

        public virtual void OnStart(GameplayEffectHandle handle)
        {
        }
        
        public virtual void OnPeriod(GameplayEffectHandle handle)
        {
        }
        
        public virtual void OnRemove(GameplayEffectHandle handle)
        {
        }
    }
}