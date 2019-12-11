using UnityEngine;

public class InventoryUI : CustomScrollerUI
{
    [Header("Inventory")]
    [SerializeField]
    private ItemUI m_ItemUIPrefab;

    public void LoadInventory()
    {
        ClearContainer();

        for(int i = 0; i < SimonInventoryManager.Instance.Items.Count; i++)
        {
            SimonInventoryManager.Item item = SimonInventoryManager.Instance.Items[i];
            ItemUI ui = Instantiate(m_ItemUIPrefab, m_Container);
            ui.InitItemUI(item);
        }
    }

    private void ClearContainer()
    {
        for(int i = 0; i < m_Container.childCount; i++)
        {
            Destroy(m_Container.GetChild(i).gameObject);
        }
    }

    public override void Open()
    {
        LoadInventory();
        base.Open();
    }
}
