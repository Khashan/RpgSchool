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




    private List<CharacterData> m_PlayerTeam = new List<CharacterData>();

    private List<CharacterData> m_EnnemyTeam = new List<CharacterData>();

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

    public void CombatSetup(List<CharacterData> aFriendlyList, List<CharacterData> aEnnemyList)
    {
        //this sets the combatcontroller's list to the current playerteam and ennemy team
        //this will get called right after we initialize the ennemies using InitEnnemyTeam()
        aFriendlyList = m_PlayerTeam;
        aEnnemyList = m_EnnemyTeam;
        m_EnnemyTeam.Clear();
    }
    
}
