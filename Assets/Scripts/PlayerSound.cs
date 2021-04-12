using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField]
    AudioClip hit;
    Player player;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        audio = GetComponent<AudioSource>();
        player.onTakeDamage.AddListener(Hit);
    }

    void Hit(int value)
    {
        audio.PlayOneShot(hit);
    }
}
