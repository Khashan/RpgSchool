using UnityEngine;

[CreateAssetMenu(menuName = MENU_PATH + "Defense")]
public class DefenseAbility : BaseAbility
{
    public enum DefenseType
    {
        HEAL,
        ARMOR,
        CLEANSE
    }

    public enum DefenseTarget
    {
        SELF_CAST,
        ALLY_CAST,
        SELF_ALLY_CAST,
    }

    [Header("Defense Setting")]
    [SerializeField]
    private DefenseTarget m_OnlyOnSelf;
    [SerializeField]
    private DefenseType m_DefenseType;

}
