using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTrigger : MonoBehaviour
{
    public Player player;

    public void trigger()
    {
        player.DoDamage();
    }
}