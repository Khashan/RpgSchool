using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCombatUI : MonoBehaviour
{
    [SerializeField]
    private Image m_Icon;
    [SerializeField]
    private TextMeshProUGUI m_TextName;
    [SerializeField]
    private Button m_Btn;

    private SimonInventoryManager.Item m_Item;
    private FighterData m_User;

    private void Start()
    {
        m_Btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        HUDManager.Instance.combatUI.UsePotion(m_Item.m_ItemData);
        SimonInventoryManager.Instance.UseItem(m_Item);
    }

    public void InitItemUI(SimonInventoryManager.Item aItem, FighterData aFighter)
    {
        m_Icon.sprite = aItem.m_ItemData.Icon;
        m_TextName.text = aItem.m_ItemData.ItemName + " x" + aItem.m_Quantity;
        m_Item = aItem;
        m_User = aFighter;
    }
}
