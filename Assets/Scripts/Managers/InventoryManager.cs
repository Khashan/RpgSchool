using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    // ***  ALL ITEM DATA is here ***
    private Dictionary<string, ItemData> m_AllItemData = new Dictionary<string, ItemData>();
    public Dictionary<string, ItemData> AllItemData
    {
        get { return AllItemData; }
    }
    // *************************************************

    // ===  Player Inventory ===
    private Dictionary<string, ItemData> m_PlayerInventory = new Dictionary<string, ItemData>();
    public Dictionary<string, ItemData> PlayerInventory
    {
        get { return PlayerInventory; }
    }
    // =================================================

    // ---  SLOT ---
    private List<Slot> m_Slot = new List<Slot>();
    public List<Slot> Slot
    {
        get { return m_Slot; }
    }
    // --------------------------------------------------

    protected override void Awake()
    {
        base.Awake();
        InitData();
    }

    private void InitData()
    {
        // find all ItemData in path ... ( ressource.. Assets/Resources/"ItemDatas")    
        // stock the array to list and....
        List<ItemData> itemDatas = Resources.LoadAll<ItemData>("ItemDatas").ToList();
        // stock List To DICTIONARY ...
        for (int i = 0; i < itemDatas.Count; i++)
        {
            m_AllItemData.Add(itemDatas[i].dataID, itemDatas[i]);
        }
    }

    public void AddItem(string itemName)
    {

    }

    public void UsedItem(string itemName)
    {

    }

    private int FindItemInSlot(string itemName)
    {
        return -1;
    }
}
