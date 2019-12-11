/*
David's Inventory wasn't implementated for this type of game, making it really hard to work with.
I kept his code, but the game is using mine.

David's Inventory is nice, but over complicated for this type of project
*/

using System;
using System.Collections.Generic;
using UnityEngine;

public class SimonInventoryManager : Singleton<SimonInventoryManager>
{
    public struct Item
    {
        public string m_Id;
        public SimonItemData m_ItemData;
        public int m_Quantity;

        public bool IsSizeMaxed()
        {
            return m_Quantity >= m_ItemData.StackSize;
        }

        public void AddQuantity(int aAmount)
        {
            m_Quantity += aAmount;
            if(m_Quantity > m_ItemData.StackSize)
            {
                m_Quantity = m_ItemData.StackSize;
            }
        }

        public void RemoveQuantity(int aAmount)
        {
            m_Quantity -= aAmount;
            if(m_Quantity < 0)
            {
                m_Quantity = 0;
            }
        }
    }

    private List<Item> m_Items = new List<Item>();
    public List<Item> Items
    {
        get { return m_Items; }
    }

    [SerializeField]
    private int m_Token = 3000;
    public int Token
    {
        get { return m_Token;}
    }

    public void Buy(SimonItemData aBuyItem)
    {
        if(CanBuy(aBuyItem.Cost))
        {
            for(int i = 0; i < m_Items.Count; i++)
            {
                Item item = m_Items[i];

                if(item.m_ItemData == aBuyItem && !item.IsSizeMaxed())
                {
                    item.AddQuantity(1);
                    UpdateItem(item);
                    return;
                }
            }

            Item aItem = new Item(){m_Id = Guid.NewGuid().ToString(),m_ItemData = aBuyItem, m_Quantity = 1};
            m_Items.Add(aItem);

            m_Token -= aBuyItem.Cost;
        }
    }

    private void UpdateItem(Item aItem)
    {
        for(int i = 0; i < m_Items.Count; i++)
        {
            Item tI = m_Items[i];

            if(tI.m_Id.Equals(aItem.m_Id))
            {
                m_Items[i] = aItem;
            }
        }
    }

    private bool CanBuy(int aPrice)
    {
        return m_Token - aPrice >= 0;
    }
    public void UseItem(Item aItem)
    {
        for(int i = 0; i < m_Items.Count; i++)
        {
            Item tI = m_Items[i];

            if(tI.m_Id.Equals(aItem.m_Id))
            {
                aItem.RemoveQuantity(0);
                m_Items[i] = aItem;
            }
        }
    }

    private bool IsValidItem(Item aItem)
    {
        return aItem.m_ItemData != null;
    }

    private Item GetItem(ScriptableObject aItemObject)
    {
        for(int i = 0; i < m_Items.Count; i++)
        {
            Item item = m_Items[i];

            if(item.m_ItemData == aItemObject)
            {
                return item;
            }
        }

        return new Item();
    }
}
