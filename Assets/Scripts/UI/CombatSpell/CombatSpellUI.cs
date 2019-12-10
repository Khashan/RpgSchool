using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatSpellUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_Name;
    [SerializeField]
    private Button m_Btn;

    private BaseAbility m_Ability;
    private FighterData m_Caster;

    private void Start()
    {
        m_Btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        HUDManager.Instance.combatUI.UseAbility(m_Ability);
    }

    public void InitSpellUI(int aId, BaseAbility aAbility, FighterData aFighter)
    {
        m_Name.name = aAbility.name;
        m_Ability = aAbility;
        m_Caster = aFighter;
    }
}
