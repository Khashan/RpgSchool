using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : Singleton<HUDManager>
{
    [SerializeField]
    private RectTransform m_Layout;// pêtit souci de sequence de layout WARNING

    // fight ui
    // spell ui
    // potion ui
    // item ui

    // pêtit souci de sequence de layout WARNING dont touch
    public void ResetLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(m_Layout);
    }
}
