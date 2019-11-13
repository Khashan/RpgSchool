using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct SaveBoxStruct
{
    public string SaveName;
    public float PlayedTime;
}

public class SaveBoxUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_SaveName;

    public void InitSave(int a_SaveId)
    {
        m_SaveName.text = SaveFile.Saves[a_SaveId] == null ? "New Game" : "Continue " + a_SaveId;
    }
}
