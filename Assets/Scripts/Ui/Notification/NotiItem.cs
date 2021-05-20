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

    // Start is called before the first frame update
    private void Start()
    {
        //TODO: add destroy animation
        StartCoroutine(nameof(AnimatedNoti));
        bg.DOFade(0f, 0f);
        label.DOFade(0f, 0f);
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