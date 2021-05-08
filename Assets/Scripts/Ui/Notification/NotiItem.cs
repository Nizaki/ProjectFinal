using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotiItem : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private TextMeshProUGUI label;

    private Color transparent = new Color(255, 255, 255, 0);

    // Start is called before the first frame update
    private void Start()
    {
        //TODO: add destroy animation
        bg.color = transparent;
        label.color = transparent;
        StartCoroutine(nameof(AnimatedNoti));
    }

    private IEnumerator AnimatedNoti()
    {
        bg.DOFade(1f, 1f);
        label.DOFade(1f, 1f);
        yield return new WaitForSeconds(3);
        bg.DOFade(0, 1f);
        label.DOFade(0, 1f).OnComplete(() => { Destroy(gameObject);});
    }
}