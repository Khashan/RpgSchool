using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveFiles
{
    [System.Serializable]
    public struct SaveData
    {
        [JsonIgnore]
        public FileStream m_File;
        public string m_FileName;
    }

    private const int TOTAL_GAMES = 3;
    private const string SAVE_PATH_FORMAT = "{0}/Saves/save{1}.dat";

    private static SaveData[] m_Saves = new SaveData[TOTAL_GAMES];
    public static SaveData[] Saves
    {
        get { return m_Saves; }
    }

    private static FileStream m_CurrentSave;
    private static StreamWriter m_Writer = null;

    private static string m_FileJson = "";

    public static void InitFiles()
    {
        CreateFolder();

        for (int i = 0; i < m_Saves.Length; i++)
        {
            string savePath = FormattingPath(i);

            if (File.Exists(savePath))
            {
                m_Saves[i] = LoadFileData(savePath);
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

    private static SaveData LoadFileData(string savePath)
    {
        FileStream file = new FileStream(savePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        SaveData save = new SaveData { m_File = file };

        using (StreamReader reader = new StreamReader(file))
        {
            save = JsonConvert.DeserializeObject<SaveData>(reader.ReadToEnd());
        }

        return save;
    }

    public static void StartNewGame(int aId, string aName)
    {
        SaveData data = new SaveData { m_File = new FileStream(FormattingPath(aId), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite), m_FileName = aName };
        m_Saves[aId] = data;
        LoadSave(aId);
        SaveFile(data);
    }

    public static bool LoadSave(int aId)
    {
        bool success = false;

        if (m_Saves[aId].m_File != null)
        {
            m_CurrentSave = m_Saves[aId].m_File;
            success = true;

            using (StreamReader reader = new StreamReader(m_CurrentSave))
            {
                m_FileJson = reader.ReadToEnd();
            }
        }

        return success;
    }

    public static void SaveFile(object aObject)
    {
        if (m_CurrentSave != null)
        {
            using (StreamWriter writer = new StreamWriter(m_CurrentSave, Encoding.UTF8))
            {
                writer.Write(JsonConvert.SerializeObject(aObject, Formatting.Indented));
                writer.Close();
            }
        }
    }

    public static T GetSaveData<T>()
    {
        if (m_CurrentSave != null)
        {
            return JsonConvert.DeserializeObject<T>(m_FileJson);
        }

        return default(T);
    }

    private static string FormattingPath(int aId)
    {
        return string.Format(SAVE_PATH_FORMAT, Application.persistentDataPath, aId);
    }
}
