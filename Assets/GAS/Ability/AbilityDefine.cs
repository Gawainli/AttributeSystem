using UnityEngine;

namespace GAS
{
    public abstract class AbilityDefine : ScriptableObject
    {
        [SerializeField] private string name;
        [SerializeField] public GameplayEffect cost;
        [SerializeField] public GameplayEffect cd;
        
    }
}