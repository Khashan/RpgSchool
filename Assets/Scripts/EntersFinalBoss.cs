using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntersFinalBoss : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D aTrig)
    {
        PlayerController player = aTrig.GetComponent<PlayerController>();
        if(player != null)
        {
            CombatManager.Instance.m_isBoss = true;
            GameManager.Instance.StartBoss();
        }
    }

}
