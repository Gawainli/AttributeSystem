using System;

namespace GAS
{
    [Serializable]
    public struct AttributeModifier
    {
        public float add;
        public float multiply;
        public float overwrite;
        
        public void Combine(AttributeModifier modifier)
        {
            add += modifier.add;
            multiply += modifier.multiply;
            overwrite = modifier.overwrite;
        }
        
        public void Reset()
        {
            add = 0;
            multiply = 0;
            overwrite = 0;
        }
    }
}