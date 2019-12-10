using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_TextName;
    [SerializeField]
    private Sprite m_Icon;
    [SerializeField]
    private Button m_Btn;
    [SerializeField]

    private SimonItemData m_Item;
    private FighterData m_Caster;

    private void Start()
    {
        m_Btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        SimonInventoryManager.Instance.Buy(m_Item);
    }

    public void InitSpellUI(int aId, SimonItemData aItem, FighterData aFighter)
    {
        m_TextName.name = aItem.ItemName;
        m_Item = aItem;
        m_Caster = aFighter;
    }
}
