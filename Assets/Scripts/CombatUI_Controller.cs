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
    private CombatInventoryPanelUI m_InventoryPanel;
    [SerializeField]
    private CombatSpellPanelUI m_SpellPanel;

    private int m_CurrentAlly = 0;
    private int m_CurrentEnemy = 0;
    private int m_EnemiesCount = 0;
    private int m_AlliesCount = 0;

    private bool m_PlayerTurns = true;
    private BaseAbility m_Ability;

    private int m_SafetyLoop = 0;

    private void Start()
    {
        HUDManager.Instance.combatUI = this;
        ActivatedEnemyButton(false);
        EventSystem.current.SetSelectedGameObject(m_FirstPos.gameObject);
        ClosePanels();
    }

    private void Update()
    {
        if (m_PlayerTurns && (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) )
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
        m_SafetyLoop = 0;
        ActivatedEnemyButton(false);
        m_AllyFighters[m_CurrentAlly].m_Position.SetActive(false);

        if(m_Ability != null)
        {
            CastAbility(enemy);
            m_Ability = null;
        }
        else
        {
            CombatManager.Instance.Attack(m_CurrentAlly, enemy);
        }
    }

    private void CastAbility(int enemy)
    {
        if(m_Ability is AttackAbility)
        {
            AttackAbility aa = (AttackAbility)m_Ability;
            aa.SendAudio();
            CombatManager.Instance.DamagingSpell(enemy, aa.AbilityDamage, aa.AbilityPrefab);
        }
        else if(m_Ability is OffenseAbility)
        {
            OffenseAbility oa = (OffenseAbility)m_Ability;
            CombatManager.Instance.HealingSpell(enemy, oa.AbilityEffect, oa.AbilityPrefab);
        }
    }

    public void UseAbility(BaseAbility aAbility)
    {
        m_SpellPanel.Close();
        ActivatedEnemyButton(true);

        for (int i = 0; i < m_EnemyFighters.Count; i++)
        {
            if (!m_EnemyFighters[i].m_IsDead)
            {
                EventSystem.current.SetSelectedGameObject(m_EnemyFighters[i].m_Position);
                break;
            }
        }

        m_Ability = aAbility;
    }

    public void UsePotion(SimonItemData aItem)
    {
        if(aItem as ConsumableItemData)
        {
            m_PlayerTurns = false;
            m_SafetyLoop = 0;
            ActivatedEnemyButton(false);
            m_AllyFighters[m_CurrentAlly].m_Position.SetActive(false);
            ConsumableItemData cons = (ConsumableItemData)aItem;
            CombatManager.Instance.UsePotion(m_CurrentAlly, cons.EffectAmount);
        }
    }

    public void NextRound()
    {
        m_PlayerTurns = true;
        m_CurrentAlly++;

        if (m_CurrentAlly >= m_AlliesCount)
        {
            m_CurrentAlly = 0;
        }

        if (CombatManager.Instance.IsFriendlyDead(m_CurrentAlly))
        {
            m_SafetyLoop++;

            if(m_SafetyLoop <= m_AlliesCount)
            {
                NextRound();
            }
            return;
        }

        ClosePanels();
        EnableCurrentPlayerIndicator();
        EventSystem.current.SetSelectedGameObject(m_FirstPos.gameObject);
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
        m_SpellPanel.LoadSpell(GameManager.Instance.Fighters[m_CurrentAlly]);
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
