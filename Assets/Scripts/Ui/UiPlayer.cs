using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPlayer : MonoBehaviour
{
    public Slider HpBar, HungerBar, WaterBar, LightBar;

    private Player player;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void LateUpdate()
    {
        HpBar.value = (float) player.hp / (float) player.maxHp;
        HungerBar.value = player.hunger / player.maxHunger;
        WaterBar.value = player.water / player.maxWater;
        LightBar.value = player.darkDelay / player.maxDarkDelay;
    }
}