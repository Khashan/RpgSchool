using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveFiles
{
    private const int TOTAL_GAMES = 3;
    private const string SAVE_PATH_FORMAT = "{0}/Saves/save{1}.dat";

    private static FileStream[] m_Saves = new FileStream[TOTAL_GAMES];
    public static FileStream[] Saves
    {
        get { return m_Saves; }
    }

    private static FileStream m_CurrentSave;
    private static StreamWriter m_Writer = null;

    public static void InitFiles()
    {
        CreateFolder();

        for (int i = 0; i < m_Saves.Length; i++)
        {
            string savePath = FormattingPath(i);

            if (File.Exists(savePath))
            {
                m_Saves[i] = new FileStream(savePath, FileMode.Open);
            }
        }
    }

    private static void CreateFolder()
    {
        string folderPath = (Application.streamingAssetsPath + "/Saves");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    public static void StartNewGame(int aId, string aName)
    {
        m_CurrentSave = new FileStream(FormattingPath(aId), FileMode.CreateNew);
        string json = JsonConvert.SerializeObject(aName, Formatting.None);

        StreamWriter write = new StreamWriter(m_CurrentSave);
        write.Write(json);
        write.Close();
    }

    public static bool Load(int aId)
    {
        bool success = false;

        if (m_Saves[aId] != null)
        {
            m_CurrentSave = m_Saves[aId];
            success = true;
        }

        return success;
    }

    public static object GetDataInCurrentSave(string aDataKey)
    {
        if (m_CurrentSave != null)
        {
            return JsonConvert.DeserializeObject(aDataKey);
        }
        else
        {
            return null;
        }
    }

    public static object GetDataInSaveFile(int aSaveId, string aDataKey)
    {
        if (m_Saves[aSaveId] != null)
        {
            return JsonConvert.DeserializeObject(aDataKey);
        }
        else
        {
            return null;
        }
    }


    private static string FormattingPath(int aId)
    {
        return string.Format(SAVE_PATH_FORMAT, Application.streamingAssetsPath, aId);
    }
}
