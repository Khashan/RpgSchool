using System.Collections.Generic;
using Anderson.CustomWindows;
using UnityEngine;

public class GamePlayUI : MonoBehaviour
{
    public enum MenuId
    {
        INVENTORY
    }

    [SerializeField]
    private CustomWindow m_ShopWindow;

    [Header("Navigations")]
    [SerializeField]
    private CustomWindow m_NavigationMenu;
    [SerializeField]
    private CustomWindow m_Inventory;

    List<CustomWindow> m_NavigationOpenMenus = new List<CustomWindow>();


    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (m_NavigationOpenMenus.Count != 0)
            {
                BackMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    private void BackMenu()
    {
        CustomWindow currOpen = m_NavigationOpenMenus[m_NavigationOpenMenus.Count - 1];
        currOpen.Close();
        m_NavigationOpenMenus.Remove(currOpen);

        if (m_NavigationOpenMenus.Count != 0)
        {
            m_NavigationOpenMenus[m_NavigationOpenMenus.Count - 1].Open();
        }
    }

    private void OpenMenu()
    {
        m_NavigationMenu.Open();
        m_NavigationOpenMenus.Add(m_NavigationMenu);
    }

    public void CloseMenu()
    {
        m_NavigationMenu.Close();
        m_NavigationOpenMenus.Remove(m_NavigationMenu);
    }

    public void OpenSubMenu(int aId)
    {
        switch ((MenuId)aId)
        {
            case MenuId.INVENTORY:
                m_Inventory.Open();
                m_NavigationOpenMenus.Add(m_Inventory);
                break;
        }
    }

    public void SetShopWindowVisibility(bool aSetActive)
    {
        if (aSetActive)
        {
            m_ShopWindow.Open();
        }
        else
        {
            m_ShopWindow.Close();
        }
    }
}
