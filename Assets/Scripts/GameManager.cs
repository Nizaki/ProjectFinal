using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //TODO:Initiate gameplay sequence ,Load Scene When load Success check if Instance missing add it to gameManager

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        SceneLoader.Instance.Load("Menu");
        ClearScore();
        SaveScore("Ryu", 2497);
    }

    public static void ClearScore()
    {
        PlayerPrefs.DeleteKey("rank");
        PlayerPrefs.Save();
    }

    public static void SaveScore(string name, int score)
    {
        var temp = PlayerPrefs.GetString("rank", "");
        temp += $"{name},{score}|";
        PlayerPrefs.SetString("rank", temp);
        PlayerPrefs.Save();
    }

    public static List<Tuple<string, int>> GetScore()
    {
        var temp = PlayerPrefs.GetString("rank", "");
        var list = new List<Tuple<string, int>>();
        var temp2 = temp.Split('|');
        if (temp2.Length > 0)
            foreach (var s in temp2)
            {
                var temp3 = s.Split(',');
                if (temp3.Length == 2)
                    list.Add(new Tuple<string, int>(temp3[0], int.Parse(temp3[1])));
            }

        return list.OrderByDescending(i => i.Item2).ToList();
    }
}