using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CursorController : MonoBehaviour
{



    [SerializeField]
    private List<Transform> m_MainCursorTransform;
    private int m_IndexVertical = 0;
    private int m_IndexHorizontal = 0;

    private int m_subMenu = 0;
    // --- Fight
    [SerializeField]
    private GameObject m_FightMenu;
    [SerializeField]
    private List<Transform> m_FightCursorTransform;
    [SerializeField]
    private List<GameObject> m_FightGameObject;

    // --- Spell
    [SerializeField]
    private GameObject m_SpellMenu;
    [SerializeField]
    private List<Transform> m_SpellCursorTransform;

    // --- Item
    [SerializeField]
    private GameObject m_ItemMenu;
    [SerializeField]
    private List<Transform> m_ItemCursorTransform;




    private void Start()
    {
        HUDManager.Instance.ResetLayout();// WARNING...
        transform.position = m_MainCursorTransform[0].position;
        Activator(4);
    }

    public void CusorSelctor(int vertical, bool clicEnter)
    {

        if (m_subMenu == 0 && (m_IndexVertical + vertical) >= 0 && (m_IndexVertical + vertical) < 4)
        {
            m_IndexVertical += vertical;

            switch (m_IndexVertical)
            {
                case 0: // FIGHT -----------------------------------------------------------
                    transform.position = m_MainCursorTransform[m_IndexVertical].position;
                    Activator(m_IndexVertical);
                    Activator(4);
                    if (clicEnter)
                    {
                        m_subMenu = 1;
                        transform.position = m_FightCursorTransform[0].position;
                        Debug.Log("FIGHT ***");
                    }
                    break;

                case 1: // SPELL -----------------------------------------------------------
                    transform.position = m_MainCursorTransform[m_IndexVertical].position;
                    Activator(m_IndexVertical);
                    Activator(5);
                    if (clicEnter)
                    {
                        transform.position = m_SpellCursorTransform[0].position;
                        Debug.Log("Spell selection");
                    }
                    break;

                case 2: // POTION -----------------------------------------------------------
                    transform.position = m_MainCursorTransform[m_IndexVertical].position;
                    Activator(m_IndexVertical);
                    Activator(5);
                    if (clicEnter)
                    {
                        transform.position = m_ItemCursorTransform[0].position;
                        Debug.Log("Item selection");
                    }
                    break;

                case 3: // FLEE -----------------------------------------------------------
                    transform.position = m_MainCursorTransform[m_IndexVertical].position;
                    Activator(m_IndexVertical);
                    Activator(5);
                    if (clicEnter)
                    {
                        string lastScene = LevelManager.Instance.LastScene;
                        LevelManager.Instance.ChangeLevel(lastScene, true, 1);
                        Debug.Log("FLEE.........");
                    }
                    break;
            }
        }
        else if (m_subMenu == 1 && (m_IndexVertical + vertical) >= 0 && (m_IndexVertical + vertical) < 6)
        {
            switch (m_IndexVertical)
            {
                case 0: // frien 1 -----------------------------------------------------------  
                    transform.position = m_FightCursorTransform[m_IndexVertical].position;
                    break;
                case 1: // frien 1 -----------------------------------------------------------  
                    transform.position = m_FightCursorTransform[m_IndexVertical].position;
                    break;
                case 2: // frien 1 -----------------------------------------------------------  
                    transform.position = m_FightCursorTransform[m_IndexVertical].position;
                    break;
                case 3: // frien 1 -----------------------------------------------------------  
                    transform.position = m_FightCursorTransform[m_IndexVertical].position;
                    break;
                case 4: // frien 1 -----------------------------------------------------------  
                    transform.position = m_FightCursorTransform[m_IndexVertical].position;
                    break;
                case 5: // frien 1 -----------------------------------------------------------  
                    transform.position = m_FightCursorTransform[m_IndexVertical].position;
                    break;
            }
        }


    }

    public void Activator(int menu)
    {
        switch (menu)
        {
            case 0: // FIGHT
                m_FightMenu.SetActive(true);
                m_SpellMenu.SetActive(false);
                m_ItemMenu.SetActive(false);
                break;
            case 1: // SPELL
                m_FightMenu.SetActive(false);
                m_SpellMenu.SetActive(true);
                m_ItemMenu.SetActive(false);
                break;
            case 2: // POTION
                m_FightMenu.SetActive(false);
                m_SpellMenu.SetActive(false);
                m_ItemMenu.SetActive(true);
                break;
            case 3: // ITEM
                m_FightMenu.SetActive(false);
                m_SpellMenu.SetActive(false);
                m_ItemMenu.SetActive(false);
                break;
            case 4: // ITEM
                for (int i = 0; i < m_FightGameObject.Count; i++)
                {
                    m_FightGameObject[i].SetActive(true);
                }
                break;
            case 5: // ITEM
                for (int i = 0; i < m_FightGameObject.Count; i++)
                {
                    m_FightGameObject[i].SetActive(false);
                }
                break;
        }
    }

}
