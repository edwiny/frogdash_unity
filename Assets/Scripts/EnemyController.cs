using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class EnemyController : MonoBehaviour
{

    public float targetingIntervalSeconds = 1.0f;
    public float speed = 1.0f;
    public GameObject target;

    PlayerController playerController;
    Vector2 targetLocation = Vector2.zero;
    float nextTargetingWindow = 0;

    Rigidbody2D rb;
    BoxCollider2D collider;


    ScreenBorders screenBorders;

    //mobility checks
    const int NUM_MOVEMENT_HISTORY = 5;
    float nextMobilityCheck = 0;
    Queue<Vector2> movementHistory;
    float restoreColliderTimer = 0;


    private void Awake()
    {
        playerController = target.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        screenBorders = new ScreenBorders();
        movementHistory = new Queue<Vector2>();
        
    }

    void locateTarget()
    {
        targetLocation = target.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var step = speed * Time.deltaTime;
        //var direction = Vector2.MoveTowards(transform.position, targetLocation, step);
        //var direction = Vector2.MoveTowards(rb.position, targetLocation, step);
        //rb.AddRelativeForce(direction, ForceMode2D.Force);



    }

    private bool IsMobile()
    {
        bool mobile_x = false, mobile_y = false;

        if (movementHistory.Count >= NUM_MOVEMENT_HISTORY)
        {
            double mean_x = movementHistory.Average(v => Math.Abs(v.x));
            double mean_y = movementHistory.Average(v => Math.Abs(v.y));

            double sumOfSquares_x = movementHistory.Sum(v => Math.Pow(Math.Abs(v.x) - mean_x, 2));
            double sumOfSquares_y = movementHistory.Sum(v => Math.Pow(Math.Abs(v.y) - mean_y, 2));

            mobile_x = (sumOfSquares_x / mean_x) > 0.1;
            mobile_y = (sumOfSquares_y / mean_y) > 0.1;

            bool result = mobile_x || mobile_y;

            if (!result)
            {
                Debug.Log("mobile_x = " + (sumOfSquares_x / mean_x) + " mobile_y = " + (sumOfSquares_y / mean_y));
            }
            return mobile_x || mobile_y;
        }

        return true;

    }

    private void FixedUpdate()
    {
        float now = Time.time;
        if(now > nextTargetingWindow)
        {
            nextTargetingWindow = now + targetingIntervalSeconds;
            targetLocation = target.transform.position;
        }
        var step = speed * Time.deltaTime;
        var direction = Vector2.MoveTowards(rb.position, targetLocation, step);
        rb.MovePosition(direction);

        if (now > nextMobilityCheck)
        {
            screenBorders.CalculateScreenBorders();
            nextMobilityCheck = now + 1;
            movementHistory.Enqueue(transform.position);
            if (movementHistory.Count > NUM_MOVEMENT_HISTORY) {
                movementHistory.Dequeue();
            }
            if (screenBorders.IsOffScreen(transform.position, 1) && !IsMobile())
            {
                collider.enabled = false;
                Debug.Log("Disabling enemy collider");
                restoreColliderTimer = now + 1;
            }
        }
        if(collider.enabled == false && now > restoreColliderTimer)
        {
            Debug.Log("Restoring Collider");
            //collider.enabled = true;
        }

    }
}
