using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CursorController : MonoBehaviour
{

    

    [SerializeField]
    private List<Transform> m_CursorMoveMenu;   
    private int m_IndexMenu = 0;

    private List<Transform> m_TargetChoice; // 0
    private int m_IndexTarget = 0;
    private List<Transform> m_SpellChoice;  // 1
    private int m_IndexSpell = 0;
    private List<Transform> m_PotionChoice; // 2
    private int m_IndexPotion = 0;
    private List<Transform> m_ItemChoice;   // 3
    private int m_IndexItem = 0;

    //[SerializeField]
    //private Transform m_CurrentPos;

    private void Start()
    {
        HUDManager.Instance.ResetLayout();// WARNING...
        transform.position = m_CursorMoveMenu[m_IndexMenu].position;        
    }

    public void EnterSelection()
    {
        switch(m_IndexMenu)
        {
            case 0:
                // Target choice UI
                break;
            case 1:
                // Spell choice UI
                break;
            case 2:
                // Potion choice UI
                break;
            case 3:
                // Item choice UI
                break;
        }          
    }

    public void BackSelection()
    {
        switch (m_IndexMenu)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }

    // +++ MENU  SELECTOR +++
    public void CombatMenuSelector(int upDown)
    {
        // Up
        if (upDown == -1 && m_IndexMenu + 1 < m_CursorMoveMenu.Count)
        {
            m_IndexMenu++;
            transform.position = m_CursorMoveMenu[m_IndexMenu].position;
        }

        // Down
        else if (upDown == 1 && m_IndexMenu - 1 >= 0)
        {
            m_IndexMenu--;
            transform.position = m_CursorMoveMenu[m_IndexMenu].position;
        }
    }

    // --- TARGET SELECTOR ---
    public void TargetSelector(int upDown)
    {
        // Up
        if (upDown == -1 && m_IndexTarget + 1 < m_TargetChoice.Count)
        {
            m_IndexTarget++;
            transform.position = m_TargetChoice[m_IndexTarget].position;
        }

        // Down
        else if (upDown == 1 && m_IndexTarget - 1 >= 0)
        {
            m_IndexTarget--;
            transform.position = m_TargetChoice[m_IndexTarget].position;
        }
    }

    // --- SPELL SELECTOR ---
    public void SpellSelector(int upDown)
    {
        // Up
        if (upDown == -1 && m_IndexSpell + 1 < m_SpellChoice.Count)
        {
            m_IndexSpell++;
            transform.position = m_SpellChoice[m_IndexSpell].position;
        }

        // Down
        else if (upDown == 1 && m_IndexSpell - 1 >= 0)
        {
            m_IndexSpell--;
            transform.position = m_SpellChoice[m_IndexSpell].position;
        }
    }

    // --- POTION SELECTOR ---
    public void PotionSelector(int upDown)
    {
        // Up
        if (upDown == -1 && m_IndexPotion + 1 < m_PotionChoice.Count)
        {
            m_IndexPotion++;
            transform.position = m_PotionChoice[m_IndexPotion].position;
        }

        // Down
        else if (upDown == 1 && m_IndexPotion - 1 >= 0)
        {
            m_IndexPotion--;
            transform.position = m_PotionChoice[m_IndexPotion].position;
        }
    }

    // --- ITEM SELECTOR ---
    public void ItemSelector(int upDown)
    {
        // Up
        if (upDown == -1 && m_IndexItem + 1 < m_ItemChoice.Count)
        {
            m_IndexItem++;
            transform.position = m_ItemChoice[m_IndexItem].position;
        }

        // Down
        else if (upDown == 1 && m_IndexItem - 1 >= 0)
        {
            m_IndexItem--;
            transform.position = m_ItemChoice[m_IndexItem].position;
        }
    }
}
