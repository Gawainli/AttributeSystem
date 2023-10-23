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
    
    public class Buff : ScriptableObject
    {
        public float duration;
        public float period;
        public bool executeImmediate;
        public int tag;
        public DurationType durationType;
        public OverlapType overlapType;
        
        public BuffModifier[] modifiers;
    }
}