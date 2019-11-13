using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavesUI : MonoBehaviour
{
    [SerializeField]
    private SaveBoxUI[] m_SaveBoxes;

    private void Awake()
    {
        SaveFile.LoadFiles();
        InitBoxes();
    }

    private void InitBoxes()
    {
        for(int i = 0; i < SaveFile.Saves.Length; i++)
        {
            m_SaveBoxes[i].InitSave(i);
        }
    }
}
