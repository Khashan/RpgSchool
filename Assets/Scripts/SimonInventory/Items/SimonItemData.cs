using UnityEngine;

public abstract class SimonItemData : ScriptableObject
{
    protected const string PATH_NAME = "ScriptableObject/SInv/";

    [SerializeField]
    private string m_ItemName;
    [SerializeField]
    private int m_MaxStackSize;
    [SerializeField]
    private int m_Cost;

    public abstract void Use(FighterData aFighter);
}
