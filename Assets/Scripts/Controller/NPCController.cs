using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{

    [SerializeField]
    private GameObject m_SelfPrefab;

    [SerializeField]
    private CharacterData m_NPCData;

    private int m_Damage;
    private int m_MaxHP;
    private int m_CurrentHP;
    private bool m_IsDead;


    private void Start()
    {
        m_Damage = m_NPCData.m_AttackDamage;
        m_MaxHP = m_NPCData.m_MaxHealth;
        m_CurrentHP = m_NPCData.m_CurrentHealth;
        m_IsDead = m_NPCData.m_IsDead;
    }



    public int Damage
    {
        get {return m_Damage;}
    }

    public int MaxHP
    {
        get {return m_MaxHP;}
    }

    public int CurrentHP
    {
        get { return m_CurrentHP;}
        set 
        {
            m_CurrentHP = value;
            
            if(m_CurrentHP <= 0)
            {
                m_IsDead = true;
            }
            else
            {
                m_IsDead = false;
            }
        }
    }

    public bool isDead
    {
        get {return m_IsDead;}
    }

}
