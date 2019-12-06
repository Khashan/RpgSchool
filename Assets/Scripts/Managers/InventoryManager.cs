using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : Singleton<InventoryManager>
{
    // ***  ALL ITEM DATA is here ***
    [SerializeField]
    private List<ItemData> m_AllItemData = new List<ItemData>();
    public List<ItemData> AllItemData
    {
        get { return m_AllItemData; }
    }
    // *************************************************

    // ===  Player Inventory ===
    [SerializeField]
    private List<Slot> m_CombatSlot = new List<Slot>();
    public List<Slot> CombatSlot
    {
        get { return m_CombatSlot; }
    }
    // =================================================

    // ===  slot ===
    [SerializeField]
    private List<Slot> m_Slot = new List<Slot>();
    public List<Slot> Inventory
    {
        get { return m_Slot; }
    }
    // =================================================
    [SerializeField]
    private TextMeshProUGUI m_PlayerTokenTMP;
    [SerializeField]
    private int m_PlayerToken = 2400;

    protected override void Awake()
    {
        base.Awake();

        TokenRefresh(0);
    }

    private void InitInventory()
    {
        for (int i = 0; i < m_Slot.Count; i++)
        {
            m_Slot[i].InitSlot();
        }
    }

    public void AddItem(ItemData data)
    {
        if (data.Value <= m_PlayerToken)
        {
            int slot = FindEmptySlot();
            if (slot != -1)
            {
                m_Slot[slot].SetSlot(data);
                HUDManager.Instance.combatUI.CombatInventory[slot].SetSlot(data);
                TokenRefresh(-data.Value);
            }
        }
    }

    public void UsedItem(ItemData data)
    {
        for (int i = 0; i < m_Slot.Count; i++)
        {
            if (m_Slot[i] == data)
            {
                HUDManager.Instance.combatUI.CombatInventory[i].ResetSlot();
                m_Slot[i].ResetSlot();
            }
        }
    }

    private int FindEmptySlot()
    {
        for(int i = 0; i < m_Slot.Count; i++)
        {
            if (m_Slot[i].isEmpty)
            {
                return i;
            }
        }
        return -1;
    }

    public void TokenRefresh(int modify)
    {
        m_PlayerToken += modify;
        m_PlayerTokenTMP.text = m_PlayerToken.ToString();
    }
}
