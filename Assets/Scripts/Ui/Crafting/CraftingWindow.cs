using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingWindow : MonoBehaviour
{
    private static CraftingWindow self;
    public List<RecipeObject> recipes;
    [SerializeField] private GameObject recipeHolder;

    [SerializeField] private GameObject recipeTemplate;

    [SerializeField] private GameObject RequirePanel;

    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        self = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        foreach (var item in recipes)
        {
            if (item == null)
                return;
            var go = Instantiate(recipeTemplate, Vector2.zero, Quaternion.identity);
            go.transform.SetParent(recipeHolder.transform);
            go.GetComponent<RecipeSlot>().recipe = item;
        }
    }

    public static void ShowText(string text)
    {
        self.RequirePanel.SetActive(true);
        self.text.text = text;
    }

    public static void Hide()
    {
        self.RequirePanel.SetActive(false);
    }
}