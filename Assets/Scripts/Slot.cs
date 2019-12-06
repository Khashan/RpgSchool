using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private string m_SlotNameID;

    [SerializeField]
    private ItemData m_Data;
    public ItemData data
    {
        get { return m_Data; }
        set { m_Data = value; }
    }

    [SerializeField]
    private Image m_Icon;
    public Image icon
    {
        get { return m_Icon; }
        set { m_Icon = value; }
    }

    [SerializeField]
    private bool m_IsEmpty = true;
    public bool isEmpty
    {
        get { return m_IsEmpty; }
        set { m_IsEmpty = value; }
    }


    [SerializeField]
    private Color m_BaseColor;

    public void InitSlot()
    {
        m_SlotNameID = name;
        m_Icon = GetComponent<Image>();
        m_BaseColor = m_Icon.color;
    }

    public void ResetSlot()
    {
        m_Icon.color = m_BaseColor;

        m_Data = null;

        m_IsEmpty = true;
    }

    public void SetSlot(ItemData data)
    {
        m_Data = data;
        m_Icon.color = Color.white;
        m_Icon.sprite = data.Icon;
        m_IsEmpty = false;
    }

    public void AciveEffect(int effect)
    {

    }

}
