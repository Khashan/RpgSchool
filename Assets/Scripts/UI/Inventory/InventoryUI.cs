using Anderson.CustomWindows;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : CustomWindow
{
    [Header("Inventory")]
    [SerializeField]
    private ItemUI m_ItemUIPrefab;
    [SerializeField]
    private ScrollRect m_Scroller;
    [SerializeField]
    private RectTransform m_Contrainer;

    private float m_UpdateInputs = 0.2f;
    private float m_CurrentUpdateInputs = 0.2f;

    private int m_CurrentChild = 0;

    public virtual void LoadInventory()
    {
    }

    private void OnEnable()
    {
        if (m_Contrainer.GetChild(0) != null)
        {
            EventSystem.current.SetSelectedGameObject(m_Contrainer.GetChild(0).gameObject);
        }
    }

    protected override void OnUpdate()
    {
        if (CanUpdateInputs())
        {
            UpdateInputs();
        }

        EventSystem.current.SetSelectedGameObject(m_Contrainer.GetChild(m_CurrentChild).gameObject);
    }

    private bool CanUpdateInputs()
    {
        if (m_CurrentUpdateInputs > 0)
        {
            m_CurrentUpdateInputs -= Time.deltaTime;
            return false;
        }

        return true;
    }

    private void UpdateInputs()
    {
        float vertical = Input.GetAxisRaw("Vertical");

        if (vertical > 0 && m_CurrentChild > 0)
        {
            m_CurrentChild--;
        }

        if (vertical < 0 && m_CurrentChild < m_Contrainer.childCount - 1)
        {
            m_CurrentChild++;
        }

        if (vertical != 0)
        {
            UpdateScrollView();
        }
    }

    private void UpdateScrollView()
    {
        m_CurrentUpdateInputs = m_UpdateInputs;

        float loc = 0;
        if (m_CurrentChild != m_Contrainer.childCount - 1)
        {
            loc = ((float)m_CurrentChild / (float)m_Contrainer.childCount) - 1;
        }

        m_Scroller.verticalNormalizedPosition = Mathf.Abs(loc);
    }

}
