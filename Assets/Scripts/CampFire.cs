using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CampFire : MonoBehaviour
{
    public CircleCollider2D coli;
    public GameObject light2D;
    public bool turnOn = false;
    public float fuel = 100;

    public float fuelDrain = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        coli = GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        if (GameTime.state == TimeState.Day) light2D.SetActive(false);

        if (fuel > 0)
        {
            coli.enabled = true;
            fuel -= fuelDrain * Time.fixedDeltaTime;
            if (GameTime.state == TimeState.Night) light2D.SetActive(true);
        }
        else
        {
            coli.enabled = false;
            light2D.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO:Make player Immune to darkness
        if (collision.CompareTag("Player")) collision.GetComponent<Player>().underLight = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) collision.GetComponent<Player>().underLight = false;
    }
}