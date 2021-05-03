using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public ItemObject item;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    private void Start()
    {
        //overide sprite with item that has assign
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = item.sprite;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}