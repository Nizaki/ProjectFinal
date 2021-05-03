using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public static TimeState state = TimeState.Night;
    public int day;

    public Light2D light2D;

    // Start is called before the first frame update
    private void Start()
    {
        totalTimeLength = dayLength + nightLength;
    }

    private void FixedUpdate()
    {
        if (running)
            time += timeSpeed * Time.fixedDeltaTime;
        label.text = string.Format($"day:{day} Time:{state}");
        if (time < dayLength && state == TimeState.Night)
        {
            //doSomeTing
            Debug.Log("it Day Now");
            light2D.intensity = 1f;
            state = TimeState.Day;
        }
        else if (time > dayLength && state == TimeState.Day)
        {
            state = TimeState.Night;
            Debug.Log("It Night Time");
            light2D.intensity = 0.1f;
            //doSomeThing
        }

        if (time > dayLength + nightLength)
        {
            time = 0;
            day += 1;
        }
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