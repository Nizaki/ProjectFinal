using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    public bool running = true;
    public float time;
    public float dayLength = 300;
    public float nightLength = 300;
    public float totalTimeLength;
    public Text label;
    public float timeSpeed = 1f;
    public static TimeState state = TimeState.Day;
    [HideInInspector] public UnityEvent<TimeState> onTimeChange;
    public int day;
    public float totalTime;
    public Light2D light2D;

    public float fadeDuration = 3f;

    // Start is called before the first frame update
    private void Start()
    {
        onTimeChange ??= new UnityEvent<TimeState>();
        totalTimeLength = dayLength + nightLength;
        StartCoroutine(nameof(Sunrise));
    }

    private void FixedUpdate()
    {
        if (running)
        {
            var add = timeSpeed * Time.fixedDeltaTime;
            time += add;
            totalTime += add;
        }
        if (time > totalTimeLength  && state == TimeState.Night)
        {
            StartCoroutine(nameof(Sunrise));
        }
        else if (time > dayLength && state == TimeState.Day)
        {
            StartCoroutine(nameof(Sunset));
        }
    }


    private IEnumerator Sunrise()
    {
        time = 0;
        day += 1;
        state = TimeState.Day;
        onTimeChange.Invoke(state);
        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            light2D.intensity = Mathf.Lerp(0, 1, i / fadeDuration);
            yield return new WaitForEndOfFrame();
        }

        light2D.intensity = 1;
        AudioPlayer.Instance.PlayDay();
    }

    private IEnumerator Sunset()
    {
        state = TimeState.Night;
        onTimeChange.Invoke(TimeState.Night);
        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            light2D.intensity = Mathf.Lerp(1, 0.1f, i / fadeDuration);
            yield return new WaitForEndOfFrame();
        }

        light2D.intensity = 0.1f;
        AudioPlayer.Instance.StopBGM();
    }

    public void StopTime()
    {
        running = false;
    }
}

public enum TimeState
{
    Day,
    Night
}