using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FighterData
{
    public void UpdateData(int aHealth)
    {
        m_CurrentHealth = aHealth;
    }

    [SerializeField]
    private string m_FighterName;
    public string Name
    {
        get { return m_FighterName; }
    }

    [SerializeField]
    private int m_CurrentHealth;
    public int Health
    {
        get { return m_CurrentHealth; }
    }
    
    [SerializeField]
    private int m_MaxHealth;
    public int MaxHealth
    {
        get { return m_MaxHealth; }
    }

    [SerializeField]
    private int m_DamagePower;
    public int DamagePower
    {
        get { return m_DamagePower; }
    }

    [SerializeField]
    private List<BaseAbility> m_Abilities;
    public List<BaseAbility> Abilities
    {
        get { return m_Abilities; }
    }
    
    [SerializeField]
    private List<EquippableItemData> m_Equipments;

    public void Heal(int aAmount)
    {
        m_CurrentHealth += aAmount;

        if(m_CurrentHealth > m_MaxHealth)
        {
            m_CurrentHealth = m_MaxHealth;
        }
    }

    public void Equipe(EquippableItemData aEquip)
    {
        m_Equipments.Add(aEquip);
        m_MaxHealth += aEquip.BonusHealth;
        m_DamagePower += aEquip.BonusDamage;
    }

    public void UnEquipe(EquippableItemData aEquip)
    {
        if(m_Equipments.Contains(aEquip))
        {
            m_Equipments.Remove(aEquip);
            m_MaxHealth -= aEquip.BonusHealth;
            m_DamagePower -= aEquip.BonusDamage;
        }
    }

    public bool HasEquipment(EquippableItemData aEquip)
    {
        return m_Equipments.Contains(aEquip);
    }
}