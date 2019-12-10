using UnityEngine;

public class InventoryUI : CustomScrollerUI
{
    [Header("Inventory")]
    [SerializeField]
    private ItemUI m_ItemUIPrefab;

    public virtual void LoadInventory()
    {
    }

    public override void Open()
    {
        base.Open();
        LoadInventory();
    }
}
