/*
David's Inventory wasn't implementated for this type of game, making it really hard to work with.
I kept his code, but the game is using mine.

David's Inventory is nice, but over complicated for this type of project
*/

using System.Collections.Generic;
using UnityEngine;

public class SimonInventoryManager : MonoBehaviour
{
    public struct Item
    {
        public SimonItemData m_ItemData;
        public int m_Quantity;

        public void AddQuantity(int aAmount)
        {
            m_Quantity += aAmount;

            if(m_Quantity > m_ItemData.StackSize)
            {
                m_Quantity = m_ItemData.StackSize;
            }
        }
    }

    private List<Item> m_Items = new List<Item>();

    public void UseItem(FighterData aUser, ScriptableObject aItemObject)
    {
        Item selectedItem = GetItem(aItemObject);

        if(IsValidItem(selectedItem))
        {
            selectedItem.m_ItemData.Use(aUser);
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
