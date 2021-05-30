using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Growable : MonoBehaviour
{
    [SerializeField] private float timeRequire = 300f;
    [SerializeField] private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(Growing));
    }

    IEnumerator Growing()
    {
        yield return new WaitForSeconds(timeRequire);

        Instantiate(target,transform.position,quaternion.identity);
        Destroy(gameObject);
    }
}
