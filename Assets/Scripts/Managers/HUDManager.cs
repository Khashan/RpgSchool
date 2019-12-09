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
    private GamePlayUI m_MainUI;
    public GamePlayUI MainUI
    {
        get {return m_MainUI;}
    }
    
    [SerializeField]
    private GameObject m_CaveUI;

    [SerializeField]
    private CombatUI_Controller m_CombatUI_Controller;
    public CombatUI_Controller combatUI
    {
        set { m_CombatUI_Controller = value; }
        get { return m_CombatUI_Controller; }
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
            case "Level01Scene":
                m_CaveUI.SetActive(false);
                m_CombatUI_Controller.gameObject.SetActive(false);
                m_MainUI.gameObject.SetActive(true);
                break;
            case "CaveScene":
                m_CombatUI_Controller.gameObject.SetActive(false);
                m_MainUI.gameObject.SetActive(false);
                m_CaveUI.SetActive(true);
                break;
            case "DefaultCombatScene":
                m_CaveUI.SetActive(false);
                m_MainUI.gameObject.SetActive(false);
                m_CombatUI_Controller.gameObject.SetActive(true);
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
}
