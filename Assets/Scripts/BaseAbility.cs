using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Ability")]
public class BaseAbility : ScriptableObject
{
    [SerializeField]
    private int m_ManaCost;
    [SerializeField]
    private int m_TurnCoolDown;
    [SerializeField]
    private bool m_AllyTargetable;
    [SerializeField]
    private int m_DamageDone;
    [SerializeField]
    private int m_HealingDone;
    [SerializeField]
    private GameObject m_EffectPrefab;

    public void CastAbilityTo(Vector3 aPosition)
    {
        GameObject go = Instantiate(m_EffectPrefab);
        go.transform.position = aPosition;
    }

    public int ManaCost
    {
        get { return m_ManaCost; }
    }

    public int TurnCoolDown
    {
        get { return m_TurnCoolDown; }
    }

    public bool CanTargetAllies
    {
        get { return m_AllyTargetable; }
    }

    public int AbilityDamage
    {
        get { return m_DamageDone; }
    }

    public int AbilityHealing
    {
        get { return m_HealingDone; }
    }
}
