using System.Collections;
using System.Collections.Generic;
using Anderson.CustomWindows;
using TMPro;
using UnityEngine;

public class UpdateToken : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_TokenText;

    private void Update()
    {
        m_TokenText.text = "Token: " + SimonInventoryManager.Instance.Token;
    }
}
