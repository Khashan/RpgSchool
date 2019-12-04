using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    // item nameID
    [SerializeField]
    private string m_DataID;
    public string dataID
    {
        get { return m_DataID; }
    }

    [SerializeField]
    private int m_ItemCount;
    public int itemCount
    {
        get { return m_ItemCount; }
        set { m_ItemCount = value; }
    }


    private void Start()
    {
        m_DataID = transform.name;
    }
}
