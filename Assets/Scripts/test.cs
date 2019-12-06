using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Button m_Button01;
    public Button m_Button02;
    public Button m_Button03;
    public Button m_Button04;


    void Start()
    {       
        EventSystem.current.SetSelectedGameObject(m_Button01.gameObject);
    }

    void Update()
    {
    }
}
