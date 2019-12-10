using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private List<FigherData> m_Fighters = new List<FigherData>();
    public List<FigherData> Fighters
    {
        get { return m_Fighters; }
    }

    private PlayerController m_Player;
    private float m_PlayerDistance = 0;
    [SerializeField]
    private int m_Odds = 900;

    private KeyValuePair<string, Vector3> m_PlayerOldScenePosition;
    private KeyValuePair<string, Vector3> m_EmptyKeyValue = new KeyValuePair<string, Vector3>();

    #region Player's functions Access
    public void PlayerController(PlayerController aPlayer)
    {
        m_Player = aPlayer;
    }

    private PlayerController Player
    {
        get { return m_Player; }
    }
    #endregion


    public FigherData GetFighterByName(string aName)
    {
        for(int i = 0; i < m_Fighters.Count; i++)
        {
            if(m_Fighters[i].Name.ToLower().Equals(aName.ToLower()))
            {
                return m_Fighters[i];
            }
        }
        
        #if UNITY_EDITOR
        Debug.LogWarning("Fighter name is not matching!");
        #endif

        return new FigherData();
    }

    public Vector3 GetLastPlayerPosition()
    {
        if (!m_PlayerOldScenePosition.Equals(m_EmptyKeyValue) && m_PlayerOldScenePosition.Key == LevelManager.Instance.CurrentScene)
        {
            return m_PlayerOldScenePosition.Value;
        }

        return Vector3.zero;
    }

    public void StartBoss()
    {
        CombatManager.Instance.m_isBoss = true;
        LevelManager.Instance.ChangeLevel("DefaultCombatScene", true, 0.5f);
    }

    public void GetPlayerDistance(float aInt)
    {
        m_PlayerDistance = aInt;
        System.Random rand = new System.Random();
        int tRandom = rand.Next(1, m_Odds);
        if (m_PlayerDistance >= tRandom)
        {
            m_PlayerOldScenePosition = new KeyValuePair<string, Vector3>(LevelManager.Instance.CurrentScene, Player.transform.position);

            LevelManager.Instance.ChangeLevel("DefaultCombatScene", true, 0.5f);
            m_PlayerDistance = 0;
            m_Player.SetDistanceTravelled(0);
        }
    }
}
