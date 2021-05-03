using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public GameObject rankItem;

    public Transform rankParent;

    private List<Tuple<string, int>> score;

    private void OnEnable()
    {
        UpdateScore();
    }

    public void ResetScore()
    {
        GameManager.ClearScore();
        ;
        UpdateScore();
    }

    private void UpdateScore()
    {
        foreach (Transform child in rankParent) Destroy(child.gameObject);
        score = GameManager.GetScore();
        foreach (var tuple in score)
        {
            var go = Instantiate(rankItem, rankParent);
            var comp = go.GetComponentsInChildren<TextMeshProUGUI>();
            comp[0].text = tuple.Item1;
            comp[1].text = tuple.Item2.ToString();
        }
    }
}