using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UiPlayer : MonoBehaviour
{
    public Slider HpBar, HungerBar, WaterBar;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void LateUpdate()
    {
        HpBar.value = ((float)player.Hp / (float)player.MaxHP);
        HungerBar.value = (player.Hunger / player.MaxHunger);
        WaterBar.value = (player.Water / player.MaxWater);
    }
}
