using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance => _instance;
    private static AudioPlayer _instance;

    private AudioSource source;

    [SerializeField] private AudioClip DayBGM;
    [SerializeField] private AudioClip NighthBGM;
    [SerializeField] private List<AudioClip> BGMList;
    [SerializeField] private List<AudioClip> SFXList;
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

    public void PlaySfx(SFX id)
    {
        source.PlayOneShot(SFXList[(int)id]);
    }

    public void PlayBGM(int id)
    {
        source.Stop();
        source.clip = BGMList[id];
        source.Play();
        source.loop = true;
    }

    public void StopBGM()
    {
        source.Stop();
    }
}

public enum SFX
{
    Eat = 1,Drink = 2,Heal = 3
}