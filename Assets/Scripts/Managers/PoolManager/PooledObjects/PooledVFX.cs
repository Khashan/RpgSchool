using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledVFX : PooledObject
{
    [SerializeField]
    private float m_TimeToDisable = 0;
    private float m_Time = 0;
    
    private void Update()
    {
        m_Time -= Time.deltaTime;
        if(m_Time <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        m_Time = m_TimeToDisable;
    }
}
