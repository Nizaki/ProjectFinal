using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioClip hit;
    private Player player;

    private AudioSource audio;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        audio = GetComponent<AudioSource>();
        player.onTakeDamage.AddListener(Hit);
    }

    private void Hit(int value)
    {
        audio.PlayOneShot(hit);
    }
}