using System;
using GAS;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public AttributeComponent attributeComponent;
    public BuffComponent buffComponent;

    private void Awake()
    {
        attributeComponent = GetComponent<AttributeComponent>();
        buffComponent = GetComponent<BuffComponent>();

    }

    private void Start()
    {
        attributeComponent.SetAttributeBaseValue("HP", 1000);
        attributeComponent.SetAttributeBaseValue("Atk", 100);
    }

    private void Update()
    {
        attributeComponent.Tick();
        buffComponent.Tick(Time.deltaTime);
    }
}