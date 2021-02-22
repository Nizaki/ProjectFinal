using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TooltipsScreenSpace : MonoBehaviour
{

    public static TooltipsScreenSpace Instance { get; private set; }

    [SerializeField]
    private RectTransform canvas;
    [SerializeField]
    public RectTransform backgroundTransform;
    [SerializeField]
    TextMeshProUGUI textMeshPro;
    RectTransform rectTransform;
    private void Awake()
    {
        Instance = this;
        rectTransform = transform.GetComponent<RectTransform>();
        Hide();
    }
    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();
        Vector2 textsize = textMeshPro.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);
        backgroundTransform.sizeDelta = textsize + paddingSize;
    }

    private void Update()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvas.localScale.x;

        if (anchoredPosition.x + backgroundTransform.rect.width > canvas.rect.width)
        {
            anchoredPosition.x = canvas.rect.width - backgroundTransform.rect.width;
        }
        if (anchoredPosition.y + backgroundTransform.rect.height > canvas.rect.height)
        {
            anchoredPosition.y = canvas.rect.height - backgroundTransform.rect.height;
        }
        anchoredPosition.y += 5;
        rectTransform.anchoredPosition = anchoredPosition;
    }

    void Show(string tooltipsText)
    {
        gameObject.SetActive(true);
        SetText(tooltipsText);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip(string text)
    {
        Instance.Show(text);
    }
    public static void HideTooltip()
    {
        Instance.Hide();
    }
}
