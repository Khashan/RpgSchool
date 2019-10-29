using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObject/CharacterData", order =2)]
public class CharacterData : ScriptableObject
{
    public int m_MaxHealth = 100;
    public int m_CurrentHealth = 100;
    public int m_AttackDamage = 40;
    public GameObject m_CharacterVisualPrefab;
}
