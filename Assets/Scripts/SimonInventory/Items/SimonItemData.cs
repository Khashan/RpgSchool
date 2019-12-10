using UnityEngine;

public abstract class SimonItemData : ScriptableObject
{
    protected const string PATH_NAME = "ScriptableObject/SInv/";

    [SerializeField]
    private string m_ItemName;
    public string ItemName
    {
        get { return m_ItemName; }
    }

    [SerializeField]
    private Sprite m_Icon;
    public Sprite Icon
    {
        get {return m_Icon;}
    }

    [SerializeField]
    private int m_MaxStackSize;
    public int StackSize
    {
        get {return m_MaxStackSize;}
    }

    [SerializeField]
    private int m_Cost;
    public int Cost
    {
        get { return m_Cost;}
    }

    public abstract void Use(FighterData aFighter);
}
