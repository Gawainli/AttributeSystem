using System;
using GAS;
using UnityEngine;
using UnityEngine.Serialization;

public class Entity : MonoBehaviour
{
    public AttributeComponent attributeComponent;
    [FormerlySerializedAs("buffComponent")] public AbilityComponent abilityComponent;

    private void Awake()
    {
        attributeComponent = GetComponent<AttributeComponent>();
        abilityComponent = GetComponent<AbilityComponent>();

    }

    private void Start()
    {
        attributeComponent.SetAttributeBaseValue("HP", 1000);
        attributeComponent.SetAttributeBaseValue("Atk", 100);
    }

    private void Update()
    {
        attributeComponent.Tick();
        abilityComponent.Tick(Time.deltaTime);
    }
}