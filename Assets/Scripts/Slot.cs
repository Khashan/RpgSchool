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
    private string m_DataID;
    public string dataID
    {
        get { return m_DataID; }
        set { m_DataID = value; }
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
    private int m_Counter;
    public int counterModifior
    {
        get { return m_Counter; }
        set { m_Counter = value; }
    }

    [SerializeField]
    private Color m_BaseColor;
    [SerializeField]
    private Sprite m_BaseIcon;
    [SerializeField]
    private TextMeshProUGUI m_TMPcount;
    string emptyString = "";

    public void InitSlot()
    {
        m_SlotNameID = name;
        m_Icon = GetComponent<Image>();
        m_BaseColor = m_Icon.color;
        m_BaseIcon = m_Icon.sprite;
        m_TMPcount = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ResetSlot()
    {
        m_Icon.color = m_BaseColor;
        m_Icon.sprite = m_BaseIcon;

        m_DataID = null;

        m_IsEmpty = true;

        CounterDisappear();
    }

    public void CounterAppear(int count)
    {
        m_Counter += count;
        m_TMPcount.text = m_Counter.ToString();
    }

    public void CounterDisappear()
    {
        m_Counter = 0;
        m_TMPcount.text = emptyString;
    }
}
