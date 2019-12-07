using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObject/SpellData", order =3)]
public class SpellData : ScriptableObject
{   
    public int m_Damage = 40;
    public GameObject m_SpellPrefab;
    public float m_Duration = 1f;
    public string m_SpellName;
}
