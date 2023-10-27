using System;
using UnityEngine.Serialization;

namespace GAS.GameplayTag
{
    [Serializable]
    public class IndexTag
    {
        public const int MaxTagLength = 4;
        public int id;
        public byte[] tagBytes;
    }
}