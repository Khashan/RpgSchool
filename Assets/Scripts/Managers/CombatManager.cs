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

    [SerializeField]
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
        for(int i = 0; i <= 3; i++)
        {
            int rand = (int)UnityEngine.Random.Range(0, m_PossibleEnnemyList.Count);
            m_EnnemyTeam.Add(m_PossibleEnnemyList[rand]);
        }
    }

    public void CombatSetup(/*List<GameObject> aFriendlyList, List<GameObject> aEnnemyList*/)
    {
        Debug.Log("Setting Combat");
        m_CombatController.SetupCombat(m_PlayerTeam, m_EnnemyTeam);


        //this sets the combatcontroller's list to the current playerteam and ennemy team
        //this will get called right after we initialize the ennemies using InitEnnemyTeam()
        /*for(int i = 0; i < 3; i++)
        {
            NPCController FController = aFriendlyList[i].AddComponent<NPCController>();
            if(FController != null)
            {
                FController.m_NPCData = m_PlayerTeam[i];
            }

            NPCController EController = aEnnemyList[i].AddComponent<NPCController>();
            if(EController != null)
            {
                EController.m_NPCData = m_EnnemyTeam[i];
            }
        }
        m_EnnemyTeam.Clear();*/
    }

    public void Attack(int aAttackingPosition, int aAttackedPosition)
    {
        m_CombatController.FriendlyAttack(aAttackingPosition, aAttackedPosition);
    }
    
}
