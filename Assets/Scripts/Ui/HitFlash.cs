using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    public EasyTween tween;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        player.onTakeDamage.AddListener(hit);
    }

    void hit(int value)
    {
        StartCoroutine(nameof(flash));
    }

    IEnumerator flash()
    {
        tween.OpenCloseObjectAnimation();
        yield return new WaitForSeconds(0.25f);
        tween.OpenCloseObjectAnimation();
        
    }

    private void OnDestroy()
    {
        player.onTakeDamage.RemoveListener(hit);
    }
}
