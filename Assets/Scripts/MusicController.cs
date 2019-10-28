using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_Music;

    private void Awake()
    {
        if (m_Music != null)
        {
            AudioManager.Instance.PlayMusic(m_Music);
        }
    }
}
