using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        AudioPlayer.Instance.PlayBGM(0);
    }

    public void StartGame()
    {
        AudioPlayer.Instance.StopBGM();
        SceneLoader.Instance.Load("gameplay");
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        
        Application.Quit();
#endif
    }
}