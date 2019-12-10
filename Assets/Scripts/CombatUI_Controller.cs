using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Anderson.CustomWindows;

public class CombatUI_Controller : MonoBehaviour
{
    [System.Serializable]
    private struct FigtherData
    {
        public GameObject m_Position;
        [HideInInspector]
        public bool m_IsDead;
    }

    [SerializeField]
    private Button m_FirstPos;
    [SerializeField]
    private Button m_FirstItemButton;
    [SerializeField]
    private Button m_FirstSpellButton;

    [SerializeField]
    private List<FigtherData> m_AllyFighters = new List<FigtherData>();
    [SerializeField]
    private List<FigtherData> m_EnemyFighters = new List<FigtherData>();

    [SerializeField]
    private List<Slot> m_CombatInventory = new List<Slot>();
    public List<Slot> CombatInventory
    {
        get { return m_CombatInventory; }
    }

    [SerializeField]
    private CustomWindow m_InventoryPanel;
    [SerializeField]
    private CustomWindow m_SpellPanel;

    private int m_CurrentAlly = 0;
    private int m_CurrentEnemy = 0;
    private int m_EnemiesCount = 0;
    private int m_AlliesCount = 0;

    private bool m_PlayerTurns = true;

    private void Start()
    {
        HUDManager.Instance.combatUI = this;
        ActivatedEnemyButton(false);
        EventSystem.current.SetSelectedGameObject(m_FirstPos.gameObject);
        ClosePanels();
    }

    private void Update()
    {
        if (m_PlayerTurns && (Input.GetKeyDown(KeyCode.Escape)))
        {
            EventSystem.current.SetSelectedGameObject(m_FirstPos.gameObject);
            ClosePanels();
        }
    }

    private void ClosePanels()
    {
        m_InventoryPanel.Close();
        m_SpellPanel.Close();
    }

    private void EnableCurrentPlayerIndicator()
    {
        for (int i = 0; i < m_AllyFighters.Count; i++)
        {
            m_AllyFighters[i].m_Position.SetActive(i == m_CurrentAlly);
        }
    }

    private void ActivatedEnemyButton(bool activated)
    {
        for (int i = 0; i < m_EnemyFighters.Count; i++)
        {
            if (m_EnemyFighters[i].m_IsDead)
            {
                m_EnemyFighters[i].m_Position.SetActive(false);
            }
            else
            {
                m_EnemyFighters[i].m_Position.SetActive(activated);
            }
        }
    }

    public void InitialiseCharacter(int allie, int ennemy)
    {
        m_AlliesCount = allie;
        m_EnemiesCount = ennemy;
        InitializeFightersData();
        EnableCurrentPlayerIndicator();
        EventSystem.current.SetSelectedGameObject(m_FirstPos.gameObject);
    }

    private void InitializeFightersData()
    {
        for (int i = 0; i < m_AllyFighters.Count; i++)
        {
            m_AllyFighters[i] = new FigtherData { m_Position = m_AllyFighters[i].m_Position, m_IsDead = false };
        }
        for (int i = 0; i < m_EnemyFighters.Count; i++)
        {
            m_EnemyFighters[i] = new FigtherData { m_Position = m_EnemyFighters[i].m_Position, m_IsDead = false };
        }
    }

    public void Fight(int enemy)
    {
        m_PlayerTurns = false;
        ActivatedEnemyButton(false);
        m_AllyFighters[m_CurrentAlly].m_Position.SetActive(false);
        CombatManager.Instance.Attack(m_CurrentAlly, enemy);
    }

    public void NextRound()
    {
        m_PlayerTurns = true;
        m_CurrentAlly++;

        if (m_CurrentAlly >= m_AlliesCount)
        {
            m_CurrentAlly = 0;
        }

        if (m_AllyFighters[m_CurrentAlly].m_IsDead)
        {
            NextRound();
            return;
        }

        ClosePanels();
        EnableCurrentPlayerIndicator();
        EventSystem.current.SetSelectedGameObject(m_FirstPos.gameObject);
    }

    public void FriendlyDead(int aAllyIndex)
    {
        if (aAllyIndex < m_AlliesCount)
        {
            m_AllyFighters[aAllyIndex] = new FigtherData { m_Position = m_AllyFighters[aAllyIndex].m_Position, m_IsDead = true };
        }
    }


    public void EnemyDead(int aIndexEnemy)
    {
        if (aIndexEnemy < m_EnemiesCount)
        {
            m_EnemyFighters[aIndexEnemy] = new FigtherData { m_Position = m_EnemyFighters[aIndexEnemy].m_Position, m_IsDead = true };
        }
    }

    public void FightButton()
    {
        ActivatedEnemyButton(true);

        for (int i = 0; i < m_EnemyFighters.Count; i++)
        {
            if (!m_EnemyFighters[i].m_IsDead)
            {
                EventSystem.current.SetSelectedGameObject(m_EnemyFighters[i].m_Position);
                break;
            }
        }
    }

    public void ItemButton()
    {
        m_InventoryPanel.Open();
    }

    public void SpellButton()
    {
        m_SpellPanel.Open();
    }
    public void FleeButton()
    {
        string lastScene = LevelManager.Instance.LastScene;
        LevelManager.Instance.ChangeLevel(lastScene, true, 1);
        ExitScene();
    }

    private void ExitScene()
    {
        m_PlayerTurns = true;
        m_CurrentAlly = 0;
        m_AlliesCount = 0;
        m_EnemiesCount = 0;
    }
}
