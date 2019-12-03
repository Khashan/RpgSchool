using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI_Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Cursor;
    [SerializeField]
    private GameObject m_Target;
    [SerializeField]
    private Image m_TargetImage;

    [SerializeField]
    private List<Transform> m_CursorPosition;
    [SerializeField]
    private int m_IndexPos = 0;
    private int m_LastIndexPos = 0;

    [SerializeField]
    private List<Transform> m_TargetFriendPosition = new List<Transform>();
    [SerializeField]
    private Transform m_Alli01;
    [SerializeField]
    private Transform m_Alli02;
    [SerializeField]
    private Transform m_Alli03;
    [SerializeField]
    private int m_TargetFriendIndexPos = 0;
    private int m_LastTargetFriendIndexPos = 0;

    [SerializeField]
    private List<Transform> m_TargetEnemyPosition = new List<Transform>();
    [SerializeField]
    private Transform m_Enemy01;
    [SerializeField]
    private Transform m_Enemy02;
    [SerializeField]
    private Transform m_Enemy03;

    private int m_RoundCount = 0;
    private bool m_EnemySelector = false;



    private void Start()
    {
        HUDManager.Instance.combatUI = this;

        m_TargetImage = m_Target.GetComponent<Image>();

        m_Target.SetActive(true);
        m_Cursor.SetActive(false);



        //InitialiseCharacter(1, 1);
    }

    private void Update()
    {
        if (!m_EnemySelector && m_IndexPos < m_CursorPosition.Count-1 && 
            (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
        {
            m_IndexPos++;
            CursorMove(m_IndexPos, false);      
        }
        else if(m_EnemySelector && m_IndexPos < m_TargetEnemyPosition.Count - 1 && 
                (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
        {
            m_IndexPos++;
            CursorMove(m_IndexPos, true);
        }
        else if(m_IndexPos > 0 &&  (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            m_IndexPos--;

            if (!m_EnemySelector)
            {
                CursorMove(m_IndexPos, false);
            }
            else
            {
                CursorMove(m_IndexPos, true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (m_EnemySelector)
            {
                StartFight(m_TargetFriendIndexPos, m_IndexPos);
            }
            else
            {
                m_LastIndexPos = m_IndexPos;
                InputCursorAction(m_IndexPos);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            CursorMove(m_LastIndexPos, false);
        }
    }

    public void InitialiseCharacter(int allie, int ennemy)
    {
        //Debug.Log(allie + " : " + ennemy);
        switch(allie)
        {
            case 1:
                m_TargetFriendPosition.Add(m_Alli01);
                break;
            case 2:
                m_TargetFriendPosition.Add(m_Alli01);
                m_TargetFriendPosition.Add(m_Alli02);
                break;
            case 3:
                m_TargetFriendPosition.Add(m_Alli01);
                m_TargetFriendPosition.Add(m_Alli02);
                m_TargetFriendPosition.Add(m_Alli03);
                break;
        }

        switch (ennemy)
        {
            case 1:
                m_TargetEnemyPosition.Add(m_Enemy01);
                break;
            case 2:
                m_TargetEnemyPosition.Add(m_Enemy01);
                m_TargetEnemyPosition.Add(m_Enemy02);
                break;
            case 3:
                m_TargetEnemyPosition.Add(m_Enemy01);
                m_TargetEnemyPosition.Add(m_Enemy02);
                m_TargetEnemyPosition.Add(m_Enemy03);
                break;
        }


        StartRound();
    }

    public void StartRound()
    {
        m_TargetFriendIndexPos += m_RoundCount;
        m_Target.transform.position = m_TargetFriendPosition[m_TargetFriendIndexPos].position;
        m_Target.SetActive(true);
        m_Cursor.transform.position = m_CursorPosition[m_IndexPos].position;
        m_Cursor.SetActive(true);

        m_IndexPos = 0;
        m_LastIndexPos = 0;

        m_TargetFriendIndexPos = 0;
        m_LastTargetFriendIndexPos = 0;

        //m_TargetEnemyIndexPos = 0;
        //m_LastTargetEnemyIndexPos = 0;
}

    public void StartFight(int allie, int enemy)
    {
        Debug.Log("Alli :" + allie + " attack -> " + enemy);

        // les alli et enemy sont de 0 a 2
        CombatManager.Instance.Attack(allie + 1, enemy + 1);

        if (m_RoundCount +1 < m_TargetFriendPosition.Count)
        {
            m_RoundCount++;
        }
        else
        {
            m_RoundCount = 0;
        }

        StartRound();
    }

    private void CursorMove(int indexPos, bool enemySelector)
    {
        if (!enemySelector)
        {
            m_Cursor.transform.position = m_CursorPosition[indexPos].position;
        }
        else
        {
            m_Target.transform.position = m_TargetEnemyPosition[indexPos].position;
        }
    }

    public void EndCombat()
    {
        m_IndexPos = 0;
    }

    private void InputCursorAction(int index)
    {
        switch(index) // FIGHT
        {
            case 0:
                if (m_EnemySelector)
                {
                    StartFight(0, 0);
                }
                else
                {
                    ActiveEnemySelector();
                }
                break;
            case 1: // SPELL
                break;
            case 2: // ITEM
                break;
            case 3: // FLEE
                string lastScene = LevelManager.Instance.LastScene;
                LevelManager.Instance.ChangeLevel(lastScene, true, 1);
                Debug.Log("FLEE.........");
                break;
        }
    }

    private void ActiveEnemySelector()
    {
        m_IndexPos = 0;
 
        m_EnemySelector = true;
        m_Target.transform.position = m_TargetEnemyPosition[0].position;
        m_TargetImage.color = Color.red;
    }

    private void ActiveTarget(int index)
    {

    }
}
