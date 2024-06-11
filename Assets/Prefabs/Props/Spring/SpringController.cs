using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringController : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            var other = collision.gameObject;
            Debug.Log("Spring: collided with " + other);
            var rb = other.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * 30, ForceMode2D.Impulse);
        }
    }
}
