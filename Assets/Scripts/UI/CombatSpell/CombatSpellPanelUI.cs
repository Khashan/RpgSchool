using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSpellPanelUI: CustomScrollerUI
{
    [SerializeField]
    private CombatSpellUI m_SpellUIPrefab;

    public void LoadSpell(FighterData aFighter)
    {
        ClearContainer();

        for(int i = 0; i < aFighter.Abilities.Count; i++)
        {
            CombatSpellUI spell = Instantiate(m_SpellUIPrefab, m_Contrainer);
            spell.InitSpellUI(i, aFighter.Abilities[i], aFighter);
        }
    }

    private void ClearContainer()
    {
        for(int i = 0; i < m_Contrainer.childCount; i++)
        {
            Destroy(m_Contrainer.GetChild(i).gameObject);
        }
    }
}
