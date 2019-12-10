using Anderson.CustomWindows;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomScrollerUI : CustomWindow
{
    [Header("Inventory")]
    [SerializeField]
    private ScrollRect m_Scroller;
    [SerializeField]
    protected RectTransform m_Container;

    private float m_UpdateInputs = 0.2f;
    private float m_CurrentUpdateInputs = 0.2f;

    protected int m_CurrentChild = 0;

    protected override void OnUpdate()
    {
        if (CanUpdateInputs())
        {
            UpdateInputs();
        }

        if(m_Container.childCount != 0)
        {
            EventSystem.current.SetSelectedGameObject(m_Container.GetChild(m_CurrentChild).gameObject);
        }
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

        if (vertical < 0 && m_CurrentChild < m_Container.childCount - 1)
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
        if (m_CurrentChild != m_Container.childCount - 1)
        {
            loc = ((float)m_CurrentChild / (float)m_Container.childCount) - 1;
        }

        m_Scroller.verticalNormalizedPosition = Mathf.Abs(loc);
    }
}
