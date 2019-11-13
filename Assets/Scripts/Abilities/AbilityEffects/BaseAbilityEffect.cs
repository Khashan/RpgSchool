using UnityEngine;

public abstract class BaseAbilityEffect : MonoBehaviour
{
    protected int m_ActiveTurns;
    protected int m_EffectPower;

    public void InitEffect(int aActiveTurns, int aEffectPower)
    {
        m_ActiveTurns = aActiveTurns;
        m_EffectPower = aEffectPower;
    }

    public abstract void TurnStarts();

    public virtual void TurnEnds()
    {
        m_ActiveTurns--;

        if (m_ActiveTurns == 0)
        {
            Destroy(this);
        }
    }
}
