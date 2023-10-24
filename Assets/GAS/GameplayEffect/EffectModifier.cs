using System;

namespace GAS
{
    public enum OpType
    {
        Add,
        Multiply,
        Overwrite,
    }
    
    
    [Serializable]
    public class EffectModifier
    {
        public string attributeName;
        public OpType opType;
        public BaseMagnitude magnitude;
    }
}