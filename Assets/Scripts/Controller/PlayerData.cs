using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order =1)]
public class PlayerData : ScriptableObject
{
    public float m_Speed;
    public int m_Hp;
}
