using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDestroyer : MonoBehaviour
{
    public GameObject m_Parent;
    public void Destroy()
    {
        if(m_Parent != null)
        {
            Destroy(m_Parent);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
