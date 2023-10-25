using System.Collections;
using System.Collections.Generic;
using GAS;
using TMPro;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    public Entity unitEntity;
    
    public TMP_Text hpText;
    public TMP_Text atkText;
    
    public GameplayEffect[] testBuffs;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = unitEntity.attributeComponent.GetAttribute("HP")?.currentValue.ToString();
        atkText.text = unitEntity.attributeComponent.GetAttribute("Atk")?.currentValue.ToString();
    }

    public void TestAddBuff(int idx)
    {
        var handle = unitEntity.abilityComponent.MakeGameplayEffectHandle(testBuffs[idx]);
        unitEntity.abilityComponent.ApplyGameplayEffectToSelf(handle);
    }
}
