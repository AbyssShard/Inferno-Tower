using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCard : CardBase, CardAction
{
    public Vector2 direction;
    public void Action()
    {
        PlayerStatus player = FindObjectOfType<PlayerStatus>();
        player.posIndex += direction;
    }
}
