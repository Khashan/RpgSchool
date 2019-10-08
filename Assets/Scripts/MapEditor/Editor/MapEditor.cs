using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MapData;
using System.IO;

public class MapEditor : EditorWindow
{
    [MenuItem("Tools/Map Editor...")]
    private static void Init()
    {
        GetWindow<MapEditor>().Show();
    }

    private const float TILE_SIZE = 40f;

    private int m_Rows = 10;
    private int m_Columns = 10;
    private TileType m_Tiles;
    private DataWrapper m_Data = new DataWrapper();
    private Vector2 m_ScrollPos;
    private Vector2Int m_LasRect = Vector2Int.one * -1;

    private void OnGUI()
    {
        SetUpTiles();
        m_Rows = EditorGUILayout.IntSlider("Rows", m_Rows, 1, 30);
        m_Columns = EditorGUILayout.IntSlider("Columns", m_Columns, 1, 30);

        m_Tiles = (TileType)EditorGUILayout.EnumPopup("Tile type",  m_Tiles);

        EditorGUILayout.BeginHorizontal();
        GUI.color = Color.green;
        if(GUILayout.Button("Save", EditorStyles.toolbarButton))
        {
            string savePath = EditorUtility.SaveFilePanel("Save Map as Json", Application.dataPath, "MapData", "json");
            if(!string.IsNullOrEmpty(savePath))
            {
                m_Data.m_Rows = m_Rows;
                m_Data.m_Columns = m_Columns;
                string json = EditorJsonUtility.ToJson(m_Data);
                File.WriteAllText(savePath,json);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        GUI.color = Color.magenta;
        if(GUILayout.Button("Load", EditorStyles.toolbarButton))
        {
            string loadPath = EditorUtility.OpenFilePanel("Load Map Data", Application.dataPath, "json");
            if(!string.IsNullOrEmpty(loadPath))
            {
                string loadText = File.ReadAllText(loadPath);
                DataWrapper data = JsonUtility.FromJson<DataWrapper>(loadText);
                if(data!= null)
                {
                    m_Data = data;
                    m_Rows = m_Data.m_Rows;
                    m_Columns = m_Data.m_Columns;
                }
            }
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();

        m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos);
        for(int y = 0; y < m_Rows; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for(int x = 0; x< m_Columns; x++)
            {
                TileType tileType = m_Data.GetType(x,y);
                switch(tileType)
                {
                    case TileType.Grass:
                        GUI.color = Color.green;
                        break;
                    case TileType.Rock:
                        GUI.color = Color.grey;
                        break;
                     case TileType.Water:
                        GUI.color = Color.blue;
                        break;
                }
                GUILayout.Box(x.ToString() + ", " + y.ToString(), GUILayout.Width(TILE_SIZE), GUILayout.Height(TILE_SIZE));
                
                Rect lasRect = GUILayoutUtility.GetLastRect();

                if(m_LasRect.x == x && m_LasRect.y == y)
                {
                    continue;
                }

                bool t_mouseIsPressed = (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag);

                if(lasRect.Contains(Event.current.mousePosition) && t_mouseIsPressed)
                {
                    m_LasRect = new Vector2Int(x,y);
                    m_Data.UpdateTile(x, y, m_Tiles);
                    Repaint();
                }
            
            
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

    private void SetUpTiles()
    {
        while(m_Rows > m_Data.m_Wrapper.Count)
        {
            m_Data.m_Wrapper.Add(new TileWrapper());
        }

        while(m_Columns < m_Data.m_Wrapper.Count)
        {
            m_Data.m_Wrapper.RemoveAt(0);
        }
        
        for(int i = 0; i < m_Data.m_Wrapper.Count; i++)
        {
            while(m_Data.m_Wrapper[i].m_TilesTypes.Count < m_Columns)
            {
                m_Data.m_Wrapper[i].m_TilesTypes.Add(TileType.Grass);
            }
            while(m_Data.m_Wrapper[i].m_TilesTypes.Count > m_Columns)
            {
                m_Data.m_Wrapper[i].m_TilesTypes.RemoveAt(0);
            }
        }
    }
}
