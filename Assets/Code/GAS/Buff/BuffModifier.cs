using System;
using GAS.Magnitude;

namespace GAS
{
    public enum OpType
    {
        Add,
        Multiply,
        Overwrite,
    }
    
    
    [Serializable]
    public class BuffModifier
    {
        public Attribute attr;
        public OpType opType;
        public BaseMagnitude magnitude;
    }
}