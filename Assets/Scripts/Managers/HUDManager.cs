using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : Singleton<HUDManager>
{
    [SerializeField]
    private RectTransform m_Layout;// pêtit souci de sequence de layout WARNING
    [SerializeField]
    private GameObject m_GameOver;
    [SerializeField]
    private GameObject m_Win;
    [SerializeField]
    private GameObject m_MainUI;
    [SerializeField]
    private GameObject m_CaveUI;
    [SerializeField]
    private GameObject m_CombatUI;
    [SerializeField]
    private GameObject m_TradeUI;
    [SerializeField]
    private CombatUI_Controller m_CombatUI_Controller;
    public CombatUI_Controller combatUI
    {
        set { m_CombatUI_Controller = value; }
        get { return m_CombatUI_Controller; }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (m_TradeUI.activeSelf)
            {
                ActiveTrade(false);
            }
            else
            {
                ActiveTrade(true);
            }
        }
    }

    // pêtit souci de sequence de layout WARNING dont touch
    public void ResetLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(m_Layout);
    }

    public void ActiveScenUI(string aScene)
    {
        switch(aScene)
        {
            case "ThomasScene":
                m_CaveUI.SetActive(false);
                m_CombatUI.SetActive(false);
                m_TradeUI.SetActive(false);
                m_MainUI.SetActive(true);
                break;
            case "CaveScene":
                m_CombatUI.SetActive(false);
                m_MainUI.SetActive(false);
                m_TradeUI.SetActive(false);
                m_CaveUI.SetActive(true);
                break;
            case "DefaultCombatScene":
                m_CaveUI.SetActive(false);
                m_MainUI.SetActive(false);
                m_TradeUI.SetActive(false);
                m_CombatUI.SetActive(true);
                break;
        }
    }

    public void ActivateWinHUD(bool aBool)
    {
        m_Win.SetActive(aBool);
    }

    public void ActivateGameOverHUD(bool aBool)
    {
        m_GameOver.SetActive(aBool);
    }

    public void ActiveTrade(bool active)
    {
        if (active)
        {
            m_TradeUI.SetActive(true);
        }
        else
        {
            m_TradeUI.SetActive(false);
        }
    }
}
