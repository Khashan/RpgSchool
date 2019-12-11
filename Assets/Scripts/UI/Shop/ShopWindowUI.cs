using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWindowUI : CustomScrollerUI
{
    [SerializeField]
    private ItemShopUI m_ItemShopPrefab;

    [SerializeField]
    private List<SimonItemData> m_Items = new List<SimonItemData>();

    private void Awake()
    {
        for(int i = 0; i < m_Items.Count; i++)
        {
            ItemShopUI item = Instantiate(m_ItemShopPrefab, m_Container);
            item.InitItemShopUI(m_Items[i]);
        }
    }
}
