using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopUI : MonoBehaviour
{
    [SerializeField]
    private Image m_Icon;
    [SerializeField]
    private TextMeshProUGUI m_TextName;
    [SerializeField]
    private TextMeshProUGUI m_TextCost;
    [SerializeField]
    private Button m_Btn;

    private SimonItemData m_Item;

    private void Start()
    {
        m_Btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        SimonInventoryManager.Instance.Buy(m_Item);
    }

    public void InitItemShopUI(SimonItemData aItem)
    {
        m_TextName.text = aItem.ItemName;
        m_Icon.sprite = aItem.Icon;
        m_TextCost.text = aItem.Cost + "";
        m_Item = aItem;
    }
}
