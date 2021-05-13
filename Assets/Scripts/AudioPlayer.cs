using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance => _instance;
    private static AudioPlayer _instance;

    private AudioSource source;

    [SerializeField] private AudioClip DayBGM;
    [SerializeField] private AudioClip NighthBGM;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public void PlayDay()
    {
        source.Stop();
        source.clip = DayBGM;
        source.Play();
        source.loop = true;
    }
}