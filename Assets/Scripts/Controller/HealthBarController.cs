using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField]
    private Image m_Bar1;
    [SerializeField]
    private Image m_Bar2;
    [SerializeField]
    private Image m_Bar3;
    [SerializeField]
    private Image m_Bar4;
    [SerializeField]
    private Image m_Bar5;
    [SerializeField]
    private Image m_Bar6;

    [SerializeField]
    private GameObject m_Ennemy1UI;

    [SerializeField]
    private GameObject m_Ennemy2UI;

    [SerializeField]
    private GameObject m_Ennemy3UI;

    private int m_EnnemyCount;

    private void Start()
    {
        CombatManager.Instance.SetHealthBarController(this);
        m_EnnemyCount = CombatManager.Instance.GetEnnemyTeamSize();
        switch(m_EnnemyCount)
        {
            case 4:
            {
                m_Ennemy1UI.SetActive(true);
                m_Ennemy2UI.SetActive(false);
                m_Ennemy3UI.SetActive(false);
                break;
            }
            case 5:
            {
                m_Ennemy1UI.SetActive(true);
                m_Ennemy2UI.SetActive(true);
                m_Ennemy3UI.SetActive(false);
                break;
            }
            case 6:
            {
                m_Ennemy1UI.SetActive(true);
                m_Ennemy2UI.SetActive(true);
                m_Ennemy3UI.SetActive(true);
                break;
            }
        }
    }

    public void ChangeLifeValues(NPCController aCurrentNPC, int aPos)
    {
        switch(aPos)
        {
            case 1:
            {
                m_Bar1.fillAmount = (float)aCurrentNPC.CurrentHP / (float)aCurrentNPC.MaxHP;
                break;
            }
            case 2:
            {
                m_Bar2.fillAmount = (float)aCurrentNPC.CurrentHP / (float)aCurrentNPC.MaxHP;
                break;
            }
            case 3:
            {
                m_Bar3.fillAmount = (float)aCurrentNPC.CurrentHP / (float)aCurrentNPC.MaxHP;
                break;
            }
            case 4:
            {
                m_Bar4.fillAmount = (float)aCurrentNPC.CurrentHP / (float)aCurrentNPC.MaxHP;
                break;
            }
            case 5:
            {
                m_Bar5.fillAmount = (float)aCurrentNPC.CurrentHP / (float)aCurrentNPC.MaxHP;
                break;
            }
            case 6:
            {
                m_Bar6.fillAmount = (float)aCurrentNPC.CurrentHP / (float)aCurrentNPC.MaxHP;
                break;
            }
        }
    }

   

}
