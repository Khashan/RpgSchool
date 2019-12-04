using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "New Item/ItemDatas", order = 0)]
public class ItemData : ScriptableObject
{
    // item nameID
    [SerializeField]
    private string m_DataID;
    public string dataID
    {
        get { return m_DataID; }
        set { m_DataID = value; }
    }

    // item position SlotID in Ui slot
    [SerializeField]
    private string m_SlotID;
    public string slotID
    {
        get { return m_SlotID; }
        set { m_SlotID = value; }
    }

    [SerializeField]
    private Sprite m_Icon;
    public Sprite Icon
    {
        get { return m_Icon; }
        //set { m_Icon = value; }
    }

    // item préfabs
    [SerializeField]
    private Items m_Item;
    public Items item
    {
        get { return m_Item; }
        //set { m_Item = value; }
    }

    // prefabs
    //private GameObject m_Prefabs;

    [SerializeField]
    private bool m_IsStackable;
    public bool IsStackable
    {
        get { return m_IsStackable; }
        set { m_IsStackable = value; }
    }

    // quand on crée un new Data soie il peut etre initialiser avec des param ou pas
    public ItemData() { }
    public ItemData(Items item, string aDataID, string aSlotID, Sprite icon, bool isStackable, int aCount)
    {
        m_Item = item;
        m_DataID = aDataID;
        m_SlotID = aSlotID;
        m_Icon = icon;
        m_IsStackable = IsStackable;
        if (IsStackable)
        {
            //m_Item.initCount += aCount;
        }
    }
}
