using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    public EasyTween tween;

    private Player player;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        player.onTakeDamage.AddListener(hit);
    }

    private void hit(int value)
    {
        StartCoroutine(nameof(flash));
    }

    private IEnumerator flash()
    {
        yield return new WaitForSeconds(0.25f);
    }

    private void OnDestroy()
    {
        player.onTakeDamage.RemoveListener(hit);
    }
}