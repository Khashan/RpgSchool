using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour
{
    [SerializeField]
    private Transform m_SaveParent;
    [SerializeField]
    private GameObject m_NameInputBox;
    [SerializeField]
    private TMP_InputField m_NameLabel;

    private int m_TotalSaves;
    private bool m_NameConfirmed = false;
    private int m_BtnIndexSelected = -1;

    private void Start()
    {
        SaveFiles.InitFiles();
        m_TotalSaves = SaveFiles.Saves.Length;

        Debug.Log(m_TotalSaves);
        ConfigSaveBtns();
    }

    private void ConfigSaveBtns()
    {

        for (int i = 0; i < m_SaveParent.childCount; i++)
        {
            Button aBtn = m_SaveParent.GetChild(i).GetComponent<Button>();

            if (aBtn != null)
            {
                ConfigButton(aBtn, i);
            }
        }
    }

    private void ConfigButton(Button aBtn, int aIndex)
    {
        aBtn.interactable = aIndex < m_TotalSaves;
        bool canBeLoaded = (aIndex < m_TotalSaves) ? SaveFiles.Saves[aIndex].m_File != null : false;

        if (canBeLoaded)
        {
            aBtn.onClick.AddListener(() => LoadSave(aIndex));
            aBtn.GetComponentInChildren<TextMeshProUGUI>()?.SetText(SaveFiles.Saves[aIndex].m_FileName);
        }
        else
        {
            aBtn.onClick.AddListener(() => CreateGame(aIndex));
            aBtn.GetComponentInChildren<TextMeshProUGUI>()?.SetText("New Game");
        }
    }

    private void LoadSave(int aId)
    {
        if (SaveFiles.LoadSave(aId))
        {

        }
    }

    private void CreateGame(int aId)
    {
        m_NameInputBox.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(WaitForName(aId));
    }

    public void ConfirmName()
    {
        if (!string.IsNullOrEmpty(m_NameLabel.text))
        {
            m_NameConfirmed = true;
            m_NameInputBox.SetActive(false);
        }
    }

    private IEnumerator WaitForName(int aId)
    {
        yield return new WaitUntil(() => m_NameConfirmed);
        SaveFiles.StartNewGame(aId, m_NameLabel.text);
    }
}
