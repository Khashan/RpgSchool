﻿public class PoisonEffect : BaseAbilityEffect
{
    public override void TurnStarts()
    {
        m_Target.ReceiveDamage(m_EffectPower);
    }
}