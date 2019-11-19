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
    private List<Transform> m_TargetFriendPosition;
    [SerializeField]
    private int m_TargetFriendIndexPos = 0;
    private int m_LastTargetFriendIndexPos = 0;

    [SerializeField]
    private List<Transform> m_TargetEnemyPosition;
    [SerializeField]
    //private int m_TargetEnemyIndexPos = 0;
    //private int m_LastTargetEnemyIndexPos = 0;


    private int m_MaxAllie = 0;
    private int m_MaxEnemy = 0;

    private bool m_EnemySelector = false;



    private void Start()
    {
        m_TargetImage = m_Target.GetComponent<Image>();

        m_Target.SetActive(false);
        m_Cursor.SetActive(false);

        InitialiseCharacter(3, 3);
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
        m_MaxAllie += allie;
        m_MaxEnemy += ennemy;
        StartRound();
    }

    public void StartRound()
    {
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
        m_MaxAllie = 0;
        m_MaxEnemy = 0;
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
