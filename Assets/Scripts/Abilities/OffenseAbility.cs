using UnityEngine;

[CreateAssetMenu(menuName = MENU_PATH + "Offense")]
public class OffenseAbility : BaseAbility
{
    public enum OffenseType
    {
        HEAL,
        ARMOR,
        CLEANSE
    }

    public enum OffenseSelection
    {
        TARGET,
        AOE
    }

    public enum OffenseTarget
    {
        SELF_CAST,
        ALLY_CAST,
        SELF_ALLY_CAST,
    }

    [Header("Offense Setting")]
    [SerializeField]
    private OffenseSelection m_Selection;
    [SerializeField]
    private OffenseTarget m_AvailableTargets;
    [SerializeField]
    private OffenseType m_OffenseType;

    [SerializeField]
    private int m_AbilityFirstEffect = 0;


    public override void CastAbilityTo(IFighter[] aTargets)
    {
        base.CastAbilityTo(aTargets);
        ApplyEffect(aTargets);
    }

    private void ApplyEffect(IFighter[] aTargets)
    {
        for(int i = 0; i < aTargets.Length; i++)
        {
            IFighter currFighter = aTargets[i];
            
            switch(m_OffenseType)
            {
                case OffenseType.HEAL:
                    currFighter.ReceiveHeal(m_AbilityFirstEffect);
                break;
                
                case OffenseType.CLEANSE:
                    currFighter.Cleanse();
                break;

                case OffenseType.ARMOR:
                    currFighter.AddArmor(m_AbilityFirstEffect);
                break;
            }
        }
    }
}
