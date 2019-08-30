using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private object m_PlayerController;
    
    //Setter since we don't need all the player controller.
    private object PlayerController
    {
        set { throw new NotImplementedException("Require Player Controller!"); m_PlayerController = value; }
    }

    #region Player's functions Access
    private object[] GetPlayerTeams()
    {
        throw new NotImplementedException("Require Player Controller!");
    }

    
    #endregion
}
