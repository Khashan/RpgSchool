using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapData
{
    public enum TileType
    {
        Grass,
        Rock,
        Water
    }

    [System.Serializable]
    public class DataWrapper
    {
        public int m_Rows;
        public int m_Columns;
        public List<TileWrapper> m_Wrapper = new List<TileWrapper>();

        public int Rows
        {
            get{return m_Wrapper.Count;}
        }

        public int Columns
        {
            get{return m_Wrapper[0].m_TilesTypes.Count;}
        }

        public TileType GetType(int aX, int aY)
        {
            if(IsValid(aX,aY))
            {
                return m_Wrapper[aY].m_TilesTypes[aX];
            }

            return TileType.Grass;
        }

        public void UpdateTile(int aX, int aY, TileType aType)
        {
            if(IsValid(aX,aY))
            {
                m_Wrapper[aY].m_TilesTypes[aX] = aType;
                Debug.Log("Valid");
            }
        }
        private bool IsValid(int aX, int aY)
        {
            if(Rows > aY)
            {
                if(Columns > aX)
                {
                    return true;
                }
            }
            return false;
        }
    }

    [System.Serializable]
    public class TileWrapper
    {
        public List<TileType> m_TilesTypes = new List<TileType>();
    }


}

