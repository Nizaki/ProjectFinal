using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Clock : MonoBehaviour
{
  public GameTime TimeObject;
  public Image clockImage;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    // clockImage.rectTransform.rotation = Quaternion.Euler(0f, 0f, (TimeObject.time / TimeObject.totalTimeLength) * 360.0f);
    clockImage.rectTransform.rotation = Quaternion.Euler(0f, 0f, -(TimeObject.time / TimeObject.totalTimeLength) * 360.0f);
  }
}
