public class HealingEffect : BaseAbilityEffect
{
    public override void TurnStarts()
    {
        m_Target.ReceiveHeal(m_EffectPower);
    }
}
