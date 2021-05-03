using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] private GameTime time;
    [SerializeField] private TMP_InputField nameInput;


    public void BackToMenu()
    {
        var playerName = nameInput.text != string.Empty ? nameInput.text : "Player";
        
        GameManager.SaveScore(playerName,Mathf.RoundToInt(time.totalTime));
        SceneLoader.Instance.Load("Menu");
    }
}
