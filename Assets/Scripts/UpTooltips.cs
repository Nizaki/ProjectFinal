using DG.Tweening;
using UnityEngine;

public class UpTooltips : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var cur = transform.localPosition;
        transform.DOLocalMoveY(cur.y + 0.2f,0.3f).OnComplete(() => { Destroy(this.gameObject);});
    }
}
