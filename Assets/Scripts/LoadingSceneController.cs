using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField]
    private float m_CurrentTime = 0f;
    [SerializeField]
    private float m_Delay = 20f;
    [SerializeField]
    private int m_RandomDelay;
    [SerializeField]
    private string m_SceneToGo = "Level01Scene";

    [SerializeField]
    private Slider m_LoadingBar;

    void Update()
    {
        m_CurrentTime += m_CurrentTime + Time.deltaTime;

        m_Delay = Random.Range(0, 8);
        if (m_CurrentTime > m_Delay)
        {
            m_Delay = Random.Range(20, 80);
            m_CurrentTime = 0;
            m_RandomDelay = Random.Range(0, 10);
            m_LoadingBar.value += m_RandomDelay;

            if(m_LoadingBar.value >= 100)
            {
                LevelManager.Instance.ChangeLevel(m_SceneToGo, true, 0.5f);
            }
        }



    }


}
