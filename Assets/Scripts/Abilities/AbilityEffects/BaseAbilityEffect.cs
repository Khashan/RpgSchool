using UnityEngine;

public abstract class BaseAbilityEffect : MonoBehaviour
{
    protected int m_ActiveTurns;
    protected int m_EffectPower;
    protected IFighter m_Target;

    [SerializeField]
    private Sprite m_Icon;
    public Sprite Icon
    {
        get { return m_Icon; }
    }

    //Removes Me later plz
    private void Awake()
    {

        //TODO -> Listen to CombatSystem Actions.

        //Test..
        Test_SkipTurns.Instance.m_TurnStarts += TurnStarts;
        Test_SkipTurns.Instance.m_TurnEnds += TurnEnds;
    }


    public void InitEffect(IFighter aTarget, int aActiveTurns, int aEffectPower)
    {
        m_Target = aTarget;
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
