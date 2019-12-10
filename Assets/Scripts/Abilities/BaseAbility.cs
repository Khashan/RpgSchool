using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : ScriptableObject
{
    protected const string MENU_PATH = "ScriptableObject/Ability/";

    [Header("Base Setting")]
    [SerializeField]
    private string m_SpellName;
    public string SpellName
    {
        get { return SpellName;}
    }

    [SerializeField]
    private int m_ManaCost;
    [SerializeField]
    private int m_TurnCoolDown;
    [SerializeField]
    private GameObject m_AbilityPrefab;
    public GameObject AbilityPrefab
    {
        get { return m_AbilityPrefab; }
    }

    [SerializeField]
    private AudioClip m_SFX;

    [Header("Effects")]
    [MonoScript]
    [SerializeField]
    private string m_EffectScript;

    [SerializeField]
    private int m_EffectLifeSpan = 1;
    [SerializeField]
    [Tooltip("Example: If its a healing spell and the power is 5 = +5 to health")]
    private int m_EffectPower;

    public virtual void SendAudio()
    {
        if(m_SFX != null)
        AudioManager.Instance.PlaySFX(m_SFX, Vector3.zero);
    }

    public virtual void CastAbilityTo(IFighter[] aTargets)
    {
        bool hasEffect = !string.IsNullOrEmpty(m_EffectScript);

        for (int i = 0; i < aTargets.Length; i++)
        {
            IFighter fighter = aTargets[i];
            
            if(hasEffect)
            {
                AddEffect(fighter);
            }

            if(m_AbilityPrefab != null)
            {
                CreateAbilityFX(fighter.GetFighterGameObject().transform.position);
            }
        }
    }

    private void AddEffect(IFighter aTarget)
    {
        BaseAbilityEffect abilityEffect = (BaseAbilityEffect)aTarget.GetFighterGameObject().AddComponent(System.Type.GetType(m_EffectScript));
        abilityEffect.InitEffect(aTarget, m_EffectLifeSpan, m_EffectPower);
    }

    private void CreateAbilityFX(Vector3 aPostion)
    {
        Instantiate(m_AbilityPrefab, aPostion, Quaternion.identity);
    }

    public int ManaCost
    {
        get { return m_ManaCost; }
    }

    public int TurnCoolDown
    {
        get { return m_TurnCoolDown; }
    }
}