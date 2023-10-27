using System;
using UnityEngine;

namespace GAS.GameplayTag
{
    public class TestIndexTag : MonoBehaviour
    {
        public IndexTagAsset tagAsset;

        private void Start()
        {
            tagAsset.AddIndexTag("A.B.C.D");
        }
    }
}