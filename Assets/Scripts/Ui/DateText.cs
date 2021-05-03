using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DateText : MonoBehaviour
{
    public GameTime timeObject;

    public TextMeshProUGUI tmp;

    // Start is called before the first frame update
    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void Update()
    {
        tmp.text = "Day: " + timeObject.day;
    }
}