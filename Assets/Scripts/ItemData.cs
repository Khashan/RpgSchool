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
    private int m_SlotID;
    public int slotID
    {
        get { return m_SlotID; }
        set { m_SlotID = value; }
    }

    
    // item position SlotID in Ui slot
    [SerializeField]
    private int m_Value;
    public int Value
    {
        get { return m_Value; }
        set { m_Value = value; }
    }

    [SerializeField]
    private Sprite m_Icon;
    public Sprite Icon
    {
        get { return m_Icon; }
        //set { m_Icon = value; }
    }

    [SerializeField]
    private bool m_IsEmpty;
    public bool IsEmpty
    {
        get { return m_IsEmpty; }
        set { m_IsEmpty = value; }
    }

    //---- ITEM EFFECT ----
    [SerializeField]
    private bool m_BoolEffect;
    public bool BoolEffect
    {
        get { return m_BoolEffect; }
    }
    [SerializeField]
    private int m_IntEffect;
    public int IntEffect
    {
        get { return m_IntEffect; }
    }
    [SerializeField]
    private float m_FloatEffect;
    public float FloatEffect
    {
        get { return m_FloatEffect; }
    }
    //-------------------------

    // quand on crée un new Data soie il peut etre initialiser avec des param ou pas
    public ItemData() { }
    public ItemData(string aDataID, int aSlotID, Sprite icon)
    {
        m_DataID = aDataID;
        m_SlotID = aSlotID;
        m_Icon = icon;
    }
}
