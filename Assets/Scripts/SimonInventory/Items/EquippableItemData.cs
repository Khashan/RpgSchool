using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = PATH_NAME + "Equippable")]
public class EquippableItemData : SimonItemData
{
    [SerializeField]
    private int m_BonusHealth;
    public int BonusHealth
    {
        get { return m_BonusHealth; }
    }

    [SerializeField]
    private int m_BonusDamage;
    public int BonusDamage
    {
        get { return m_BonusDamage; }
    }

    public override void Use(FighterData aFighter)
    {
        aFighter.Equipe(this);
        GameManager.Instance.OverriderFigther(aFighter);
    }
}
