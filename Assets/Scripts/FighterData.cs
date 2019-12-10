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
    private List<BaseAbility> m_Abilities;
    public List<BaseAbility> Abilities
    {
        get { return m_Abilities; }
    }
    
    [SerializeField]
    private List<object> m_Equipments;
    public List<object> Equipments
    {
        get { return m_Equipments;}
    }

    public void Heal(int aAmount)
    {
        m_CurrentHealth += aAmount;

        if(m_CurrentHealth > m_MaxHealth)
        {
            m_CurrentHealth = m_MaxHealth;
        }
    }
}