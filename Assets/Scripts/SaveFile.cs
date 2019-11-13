using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveFile
{
    private const string SAVE_PATH_FORMAT = "{0}/Saves/save{1}.dat";
    private static FileStream[] m_Saves = new FileStream[3];
    public static FileStream[] Saves
    {
        get { return m_Saves; }
    }

    private static FileStream m_CurrentSave;
    private static BinaryFormatter formatter = new BinaryFormatter();

    public static void LoadFiles()
    {
        CreateFolder();

        for (int i = 0; i < m_Saves.Length; i++)
        {
            if (File.Exists(FormattingPath(i)))
            {
                m_Saves[i] = new FileStream(FormattingPath(i), FileMode.Open);
            }
        }
    }

    private static void CreateFolder()
    {
        string folderPath = (Application.persistentDataPath + "/Saves");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    public static void StartNewGame(int a_Id, string a_GameName)
    {
        m_CurrentSave = new FileStream(FormattingPath(a_Id), FileMode.Create);
        formatter.Serialize(m_CurrentSave, "{GameName: PP}");
        m_CurrentSave.Close();
    }

    public static void Load(int a_Id)
    {
        m_CurrentSave = m_Saves[a_Id];
    }

    public static void Save()
    {
        
    }

    private static string FormattingPath(int a_SaveId)
    {
        return string.Format(SAVE_PATH_FORMAT, Application.persistentDataPath, a_SaveId);
    }
}
