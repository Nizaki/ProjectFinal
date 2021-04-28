using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NotiItem : MonoBehaviour
{
  [SerializeField]
  Image bg;
  [SerializeField]
  TextMeshProUGUI label;
  Color transparent = new Color(255, 255, 255, 0);
  // Start is called before the first frame update
  void Start()
  {
    //TODO: add destroy animation
    bg.color = transparent;
    label.color = transparent;
    StartCoroutine("count");
  }
  IEnumerator count()
  {
    yield return new WaitForEndOfFrame();
    Color c1 = bg.color;
    Color c2 = label.color;
    for (float i = 0; i <= 0.8f; i += 0.05f)
    {
      c1.a = i;
      bg.color = c1;
      c2.a = i * 1.75f > 1 ? 1 : i * 2.0f;
      Debug.Log(c2.a);
      label.color = c2;
      yield return new WaitForSeconds(0.05f);
    }
    yield return new WaitForSeconds(3);
    for (float i = 0.5f; i >= 0f; i -= 0.05f)
    {
      c1.a = i;
      bg.color = c1;
      c2.a = i * 2.0f;
      label.color = c2;
      yield return new WaitForSeconds(0.05f);
    }
    Destroy(gameObject);
  }
}
