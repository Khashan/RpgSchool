using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatUI_Controller : MonoBehaviour
{

    [SerializeField]
    private Button m_FirstPos;
    [SerializeField]
    private Button m_FirstItemButton;
    [SerializeField]
    private Button m_FirstSpellButton;

    [SerializeField]
    private List<GameObject> m_ActivatedAllieTarget = new List<GameObject>();
    [SerializeField]
    private List<GameObject> m_ActivatedEnemyButton = new List<GameObject>();

    [SerializeField]
    private List<Slot> m_CombatInventory = new List<Slot>();
    public List<Slot> CombatInventory
    {
        get { return m_CombatInventory; }
    }

    [SerializeField]
    private List<GameObject> m_SpellButton = new List<GameObject>();
    public List<GameObject> ListSpellButton
    {
        get { return m_SpellButton; }
    }

    [SerializeField]
    private List<GameObject> m_ItemButton = new List<GameObject>();
    public List<GameObject> ListItemButton
    {
        get { return m_ItemButton; }
    }

    private int m_Allie = 0;
    private int m_AllieCount = 0;
    private int m_Enemy = 0;
    private int m_EnemyCount = 0;

    private void Start()
    {
        HUDManager.Instance.combatUI = this;
        ActivatedEnemyButton(false);
        ShowItem(false);
        ShowSpell(false);
        EventSystem.current.SetSelectedGameObject(m_FirstPos.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EventSystem.current.SetSelectedGameObject(m_FirstPos.gameObject);
            ShowItem(false);
            ShowSpell(false);
        }
    }

    private void ActiveAllie(bool desableAll)
    {
        if (!desableAll)
        {
            Debug.Log("InActiveAllie Function:" + m_AllieCount);
            for (int i = 0; i < m_ActivatedAllieTarget.Count; i++)
            {
                if (i == m_AllieCount)
                {
                    m_ActivatedAllieTarget[i].SetActive(true);
                }
                else
                {
                    m_ActivatedAllieTarget[i].SetActive(false);
                }
            }

            EventSystem.current.SetSelectedGameObject(m_FirstPos.gameObject);      
        }
        else
        {
            for (int i = 0; i < m_ActivatedAllieTarget.Count; i++)
            {
                m_ActivatedAllieTarget[i].SetActive(false);
            }
        }

    }

    private void ActivatedEnemyButton(bool activated)
    {
        for (int i = 0; i < m_ActivatedEnemyButton.Count; i++)
        {
            if (activated)
            {
                m_ActivatedEnemyButton[i].SetActive(true);
            }
            else
            {
                m_ActivatedEnemyButton[i].SetActive(false);
            }
        }
    }

    public void InitialiseCharacter(int allie, int ennemy)
    {
        m_Allie = allie;
        m_Enemy = ennemy;
        m_AllieCount = 0;
       // Debug.Log(m_AllieCount);
        ActiveAllie(false);
    }

    public void Fight(int enemy)
    {
        ActiveAllie(true);
        int allie = m_AllieCount + 1;
        if(allie > 3)
        {
            allie = 1;
        }
        CombatManager.Instance.Attack(allie, enemy);
        ActivatedEnemyButton(false);
    }

    public void NextRound()
    {
        if (m_AllieCount < m_Allie)
        {
            m_AllieCount++;
        }
        else
        {
            m_AllieCount = 0;
        }

        ActiveAllie(false);
    }

    public void FightButton()
    {
        for (int i = 0; i < m_ActivatedEnemyButton.Count; i++)
        {
            if (i < m_Enemy)
            {
                m_ActivatedEnemyButton[i].SetActive(true);
            }
            else
            {
                m_ActivatedEnemyButton[i].SetActive(false);
            }
        }

        EventSystem.current.SetSelectedGameObject(m_ActivatedEnemyButton[0]);
    }

    public void ItemButton()
    {
        ShowItem(true);
        EventSystem.current.SetSelectedGameObject(m_FirstItemButton.gameObject);
    }
    public void SpellButton()
    {
        ShowSpell(true);
        EventSystem.current.SetSelectedGameObject(m_FirstSpellButton.gameObject);
    }
    public void FleeButton()
    {
        string lastScene = LevelManager.Instance.LastScene;
        LevelManager.Instance.ChangeLevel(lastScene, true, 1);
        ExitScene();
    }

    private void ExitScene()
    {
        m_Allie = 0;
        m_AllieCount = 0;
        m_Enemy = 0;
        m_EnemyCount = 0;
    }

    private void ShowSpell(bool active)
    {
        for (int i = 0; i < m_SpellButton.Count; i++)
        {
            if (active)
            {
                m_SpellButton[i].SetActive(true);
            }
            else
            {
                m_SpellButton[i].SetActive(false);
            }
        }
    }
    private void ShowItem(bool active)
    {
        for (int i = 0; i < m_ItemButton.Count; i++)
        {
            if (active)
            {
                m_ItemButton[i].SetActive(true);
            }
            else
            {
                m_ItemButton[i].SetActive(false);
            }
        }
    }
}
