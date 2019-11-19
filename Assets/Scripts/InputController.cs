using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private CursorController m_Cursor;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.anyKeyDown);

        //if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        //{
        //    m_Cursor.CusorSelctor(1, false);
        //}
        //else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        //{
        //    m_Cursor.CusorSelctor(-1, false);
        //}
        //else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        //{
        //    m_Cursor.CusorSelctor(0, false);
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        //{
        //    m_Cursor.CusorSelctor(0, true);
        //}
        //else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        //{
        //    m_Cursor.CusorSelctor(0, true);
        //}
    }
}
