using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    public bool running = true;
    public float time = 0;
    public float dayLength = 300;
    public float nigntLength = 300;
    public Text label;
    public float timeSpeed = 1f;
    public static TimeState state = TimeState.NiGHT;
    public int day = 0;
    public Light2D light2D;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        if (running)
            time += timeSpeed * Time.fixedDeltaTime;
        label.text = string.Format($"day:{day} Time:{state}");
        if (time < dayLength && state == TimeState.NiGHT)
        {
            //doSomeTing
            Debug.Log("it Day Now");
            light2D.intensity = 1f;
            state = TimeState.DAY;
        }
        else if (time > dayLength && state == TimeState.DAY)
        {
            state = TimeState.NiGHT;
            Debug.Log("It Night Time");
            light2D.intensity = 0.1f;
            //doSomeThing
        }
        if (time > dayLength + nigntLength)
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
    DAY, NiGHT
}