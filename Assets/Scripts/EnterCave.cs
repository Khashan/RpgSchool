using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCave : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D a_Trig)
    {
        PlayerController Player = a_Trig.gameObject.GetComponent<PlayerController>();
        if(Player != null)
        {
            LevelManager.Instance.ChangeLevel("CaveScene", true, 1);
        }
    }
}
