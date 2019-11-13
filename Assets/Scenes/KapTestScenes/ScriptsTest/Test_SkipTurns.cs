using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SkipTurns : MonoBehaviour
{
    private static Test_SkipTurns s_Instance;
    public static Test_SkipTurns Instance
    {
        get { return s_Instance; }
    }


    public System.Action m_TurnStarts;
    public System.Action m_TurnEnds;

    private void Awake()
    {
        s_Instance = this;
    }

    public void EndTurns()
    {
        if(m_TurnEnds != null)
        {
            m_TurnEnds();
        }
    }

    public void StartTurns()
    {
        if(m_TurnStarts != null)
        {
            m_TurnStarts();
        }
    }
}
