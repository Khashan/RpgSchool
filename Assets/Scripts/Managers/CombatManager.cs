using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    [SerializeField]
    private List<GameObject> m_ListBackground = new List<GameObject>();

    //[SerializeField]
    //private List<>

    private List<object> m_PlayerTeam = new List<object>();

    private List<object> m_EnnemyTeam = new List<object>();

    public void CombatSetup()
    {

    }
    
}
