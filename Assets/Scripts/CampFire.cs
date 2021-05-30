using System;
using System.Collections;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public GameObject light2D;
    public float duration = 120f;

    private GameTime gt;
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(nameof(CountDown));
        gt =GameObject.Find("Sun").GetComponent<GameTime>();
        gt.onTimeChange.AddListener(OnTimeChange);
        OnTimeChange(GameTime.state);
    }

    private void OnTimeChange(TimeState timeState)
    {
            Debug.Log(timeState);
        switch (timeState)
        {
            case TimeState.Day:
                light2D.SetActive(false);
                break;
            case TimeState.Night:
                light2D.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) collision.GetComponent<Player>().underLight = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) collision.GetComponent<Player>().underLight = false;
    }

    private void OnDestroy()
    {
        gt.onTimeChange.RemoveListener(OnTimeChange);
    }
}