using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private PlayerController m_Player;
    private int m_PlayerDistance = 0;
    [SerializeField]
    private int m_Odds = 125;

    //Setter since we don't need all the player controller.
    public void PlayerController(PlayerController aPlayer)
    {
        m_Player = aPlayer;
    }

    #region Player's functions Access
    private PlayerController[] GetPlayerTeams()
    {
        throw new NotImplementedException("Require Player Controller!");
    }
    #endregion


    public void GetPlayerDistance(int aInt)
    {
        m_PlayerDistance = aInt;
        System.Random rand = new System.Random();
        int tRandom = rand.Next(1, m_Odds);
        if(m_PlayerDistance >= tRandom)
        {
            LevelManager.Instance.ChangeLevel("DefaultCombatScene", true, 0.5f);
            m_PlayerDistance = 0;
            m_Player.SetDistanceTravelled(0);
        }
    }
}
