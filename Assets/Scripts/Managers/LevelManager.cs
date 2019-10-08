﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private CanvasGroup m_LoadingScreen;
    [SerializeField]
    private float m_DelayLoading = 4f;

    private float m_CurrentTimer;
    private string m_SceneToLoad;

    private bool m_SceneLoaded = false;
    private bool m_FadeIn = false;
    private bool m_FadeOut = false;

    public Action m_OnLoadingFinished;

    protected override void Awake()
    {
        base.Awake();
        m_LoadingScreen.blocksRaycasts = false;
        m_LoadingScreen.interactable = false;
        m_LoadingScreen.alpha = 0;
    }

    private void Update()
    {
        if (m_FadeIn)
        {
            FadeIn();
        }
        else if (m_CurrentTimer > 0)
        {
            m_CurrentTimer -= Time.deltaTime;
            if (m_CurrentTimer <= 0 && m_SceneLoaded)
            {
                EndLoading();
            }
        }
        else if (m_FadeOut)
        {
            FadeOut();
        }
    }

    private void FadeIn()
    {
        m_LoadingScreen.alpha += Time.deltaTime;

        if (m_LoadingScreen.alpha == 1)
        {
            SceneManager.LoadScene(m_SceneToLoad);
            m_FadeIn = false;
        }
    }

    private void FadeOut()
    {
        m_LoadingScreen.alpha -= Time.deltaTime;

        if (m_LoadingScreen.alpha == 0)
        {
            m_FadeOut = false;
        }
    }

    private void StartLoading()
    {
        m_SceneLoaded = false;
        m_LoadingScreen.blocksRaycasts = true;

        if(!m_FadeIn)
        {
            SceneManager.LoadScene(m_SceneToLoad);
        }
    }

    private void EndLoading()
    {
        m_FadeOut = true;
        m_LoadingScreen.blocksRaycasts = false;
        m_LoadingScreen.interactable = false;

        if(m_OnLoadingFinished != null)
        {
            m_OnLoadingFinished();
        }
    }

    private void OnLoadingDone(Scene a_Scene, LoadSceneMode a_Mode)
    {
        SceneManager.sceneLoaded -= OnLoadingDone;
        m_SceneLoaded = true;

        if (m_CurrentTimer <= 0)
        {
            EndLoading();
        }
    }

    public bool HasBeenLoaded()
    {
        return m_LoadingScreen.alpha == 0;
    }

    public void ChangeLevel(string a_Scene, bool a_Fadding, float a_FixedDelay = -1)
    {
        m_SceneToLoad = a_Scene;
        m_CurrentTimer = (a_FixedDelay == -1) ? m_DelayLoading : a_FixedDelay;
        m_FadeIn = a_Fadding;

        SceneManager.sceneLoaded += OnLoadingDone;
        StartLoading();
    }

}