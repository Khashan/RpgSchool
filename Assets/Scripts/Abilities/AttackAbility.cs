using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MENU_PATH + "AttackAbility")]
public class AttackAbility : BaseAbility
{
    public enum AttackType
    {
        INSTANT,
        EFFECT
    }

    public enum AttackTarget
    {
        ONE_TARGET,
        ALL_TARGET
    }

    [Header("Attack Settings")]
    [SerializeField]
    private AttackType m_AttackType;
    public AttackType Type
    {
        get { return m_AttackType; }
    }

    [SerializeField]
    private AttackTarget m_AttackTarget;
    public AttackTarget Target
    {
        get { return m_AttackTarget; }
    }

    [SerializeField]
    private int m_AbilityFirstDamage = 0;

    public override void CastAbilityTo(IFighter[] aTargets)
    {
        base.CastAbilityTo(aTargets);

        if (m_AttackType == AttackType.INSTANT)
        {
            for (int i = 0; i < aTargets.Length; i++)
            {
                aTargets[i].ReceiveDamage(m_AbilityFirstDamage);
            }
        }
    }
}
