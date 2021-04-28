using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
  public static Notification Instance { get { return _instance; } }
  private static Notification _instance;
  [SerializeField]
  Transform viewport;
  [SerializeField]
  GameObject notiTemplate;

  void Awake()
  {
    if (_instance != null && _instance != this)
    {
      Destroy(this.gameObject);
      return;
    }
    if (_instance == null)
    {
      _instance = this;
    }
  }

  public void Show(string text)
  {
    var obj = Instantiate(notiTemplate, viewport.transform);
    obj.transform.SetParent(viewport);
    obj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
  }

  public static void ShowNotification(string text)
  {
    Instance.Show(text);
  }
}
