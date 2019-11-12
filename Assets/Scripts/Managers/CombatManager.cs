using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatManager : Singleton<CombatManager>
{
    [SerializeField]
    private List<GameObject> m_ListBackground = new List<GameObject>();
    [SerializeField]
    private List<CharacterData> m_PossibleEnnemyList = new List<CharacterData>();

    //[SerializeField]
    //private List<>



    [SerializeField]
    private List<CharacterData> m_PlayerTeam = new List<CharacterData>();

    //[SerializeField]
    private List<CharacterData> m_EnnemyTeam = new List<CharacterData>();

    private CombatController m_CombatController;
    public CombatController CombatController
    {
        set{ m_CombatController = value;}
    }

    public void InitEnnemyTeam()
    {
        //adding 3 random ennemies from the possible ennemies list
        //this will get called at the start of the combat scene
        for(int i = 0; i < 3; i++)
        {
            int rand = (int)UnityEngine.Random.Range(0, m_PossibleEnnemyList.Count);
            m_EnnemyTeam.Add(m_PossibleEnnemyList[rand]);
        }
    }

    public void CombatSetup()
    {
        Debug.Log("Setting Combat");
        m_CombatController.SetupCombat(m_PlayerTeam, m_EnnemyTeam);
    }

    public void Attack(int aAttackingPosition, int aAttackedPosition)
    {
        m_CombatController.FriendlyAttack(aAttackingPosition, aAttackedPosition);
    }
    
}
