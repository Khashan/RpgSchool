using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleRotation : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate (Vector3.forward * 2);
    }

    public void OnTriggerEnter2D(Collider2D a_Trig)
    {
        PlayerController Player = a_Trig.gameObject.GetComponent<PlayerController>();
        if(Player != null)
        {
            LevelManager.Instance.ChangeLevel("AfterCave", true, 1);
        }
    }
}
