using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
  public static AudioPlayer Instance { get { return _instance; } }
  private static AudioPlayer _instance;

  AudioSource source;

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

  // Start is called before the first frame update
  void Start()
  {
    source = GetComponent<AudioSource>();
  }

  public void play(AudioClip clip)
  {
    source.PlayOneShot(clip);
  }
}
