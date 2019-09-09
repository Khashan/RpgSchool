using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int m_CurrentCol = 0;
    private int m_CurrentRng = 0;
    

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            gameObject.transform.position += new Vector3(-1,0,0);
            --m_CurrentRng;
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            gameObject.transform.position += new Vector3(1,0,0);
            ++m_CurrentRng;
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            gameObject.transform.position += new Vector3(0,-1,0);
            --m_CurrentCol;
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            gameObject.transform.position += new Vector3(0,1,0);
            ++m_CurrentCol;
        }
    }
}
