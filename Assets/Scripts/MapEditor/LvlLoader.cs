using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MapData;

public class LvlLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject m_GrassPrefab;
    [SerializeField]
    private GameObject m_RockPrefab;
    [SerializeField]
    private GameObject m_WaterPrefab;
    [SerializeField]
    private float m_Spacing = 1f;
    [SerializeField]
    private string m_MapToLoad;

    private void Start()
    {
        string json = File.ReadAllText(Application.streamingAssetsPath + "/" + m_MapToLoad + ".json");
        DataWrapper data = JsonUtility.FromJson<DataWrapper>(json);

        if(data != null)
        {
            GameObject container = new GameObject("MapContainer");
            for(int y = 0; y < data.Rows; y++)
            {
                for(int x = 0; x < data.Columns; x++)
                {
                    TileType tileType = data.GetType(x,y);
                    GameObject tileObject = null;
                    switch(tileType)
                    {
                        case TileType.Grass:
                            tileObject = Instantiate(m_GrassPrefab);
                            break;
                       case TileType.Rock:
                            tileObject = Instantiate(m_RockPrefab);
                            break;
                        case TileType.Water:
                            tileObject = Instantiate(m_WaterPrefab);
                            break;
                    }
                tileObject.transform.position = new Vector3(x * -m_Spacing, y * -m_Spacing);
                tileObject.transform.SetParent(container.transform);
                }
            }
                

        }
    }

}
