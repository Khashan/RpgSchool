using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField]
    private Image m_Icon;
    [SerializeField]
    private TextMeshProUGUI m_TextName;
    [SerializeField]
    private Button m_Btn;

    private SimonItemData m_Item;
    private FighterData m_User;

    private void Start()
    {
        m_Btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
    }

    public void InitItemUI(int aId, SimonItemData aItem, FighterData aUser = new FighterData())
    {
        m_Icon.sprite = aItem.Icon;
        m_TextName.name = aItem.ItemName;
        m_Item = aItem;

    }

    public void SetUser(FighterData aFighter)
    {
    }

    public void UseTo(FighterData aFighter)
    {

    }

}
