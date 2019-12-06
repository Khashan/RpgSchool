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
    [SerializeField]
    private HealthBarController m_HealthBarController;

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

    public void SetHealthBarController(HealthBarController aController)
    {
        m_HealthBarController = aController;
    }

    public int GetEnnemyTeamSize()
    {
        return m_EnnemyTeam.Count;
    }

    public void ChangeLifeValue(NPCController aNPCDATA, int aPosition)
    {
        m_HealthBarController.ChangeLifeValues(aNPCDATA, aPosition);
    }

    public void InitEnnemyTeam()
    {
        //adding 3 random ennemies from the possible ennemies list
        //this will get called at the start of the combat scene
        int ennemycountrand = (int)UnityEngine.Random.Range(1, 4);
        for(int i = 0; i < ennemycountrand; i++)
        {
            int rand = (int)UnityEngine.Random.Range(0, m_PossibleEnnemyList.Count);
            m_EnnemyTeam.Add(m_PossibleEnnemyList[rand]);
        }
    }

    public List<int> GetAliveEnnemies()
    {
        return m_CombatController.GetAliveEnnemies();
    }

    public void CombatSetup()
    {
        Debug.Log("Setting Combat");
        m_CombatController.SetupCombat(m_PlayerTeam, m_EnnemyTeam);

        HUDManager.Instance.combatUI.InitialiseCharacter(m_PlayerTeam.Count, m_EnnemyTeam.Count);
    }

    public void Attack(int aAttackingPosition, int aAttackedPosition)
    {
        m_CombatController.FriendlyAttack(aAttackingPosition, aAttackedPosition);
    }
    
}
