using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : ScriptableObject
{
    protected const string MENU_PATH = "ScriptableObject/Ability/";

    [Header("Base Setting")]
    [SerializeField]
    private int m_ManaCost;
    [SerializeField]
    private int m_TurnCoolDown;
    [SerializeField]
    private GameObject m_EffectPrefab;

    [Header("Effects")]
    [MonoScript]
    private string m_AbilityScript;

    [SerializeField]
    private int m_TurnLifespan = 1;
    [SerializeField]
    [Tooltip("Example: If its a healing spell and the power is 5 = +5 to health")]
    private int m_EffectPower;

    public virtual void CastAbilityTo(IFighter[] aTargets)
    {
        if (!string.IsNullOrEmpty(m_AbilityScript))
        {
            BaseAbilityEffect ae = (BaseAbilityEffect)aTargets[0].GetFighterGameObject().AddComponent(System.Type.GetType(m_AbilityScript));
            ae.InitEffect(m_TurnLifespan, m_EffectPower);
        }

        for (int i = 0; i < aTargets.Length; i++)
        {
            IFighter fighter = aTargets[i];

            GameObject go = Instantiate(m_EffectPrefab);
            go.transform.position = fighter.GetFighterGameObject().transform.position;
        }
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