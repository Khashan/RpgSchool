using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{

    [SerializeField]
    private GameObject m_SelfPrefab;

    [SerializeField]
    private CharacterData m_NPCData;

    public int Damage
    {
        get {return m_NPCData.m_AttackDamage;}
    }

    public int CurrentHP
    {
        get { return m_NPCData.m_CurrentHealth;}
        set { m_NPCData.m_CurrentHealth = value;}
    }
}
