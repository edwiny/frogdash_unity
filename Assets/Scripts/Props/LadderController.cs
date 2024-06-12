using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    public bool isLadder;
    public bool isWaterfall;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (isLadder)
            {
               player.SuspendGravity();

            }
            if (isWaterfall)
            {
                player.SetGravity(20);
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.RestoreGravity();
        }
    }

}
