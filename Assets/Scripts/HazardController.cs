using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardController : MonoBehaviour
{

    
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hazard: something collided with me" +  collision.gameObject);
        var player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.AcceptHit();
        }
    }
}
