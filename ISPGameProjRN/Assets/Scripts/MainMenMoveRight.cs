using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenMoveRight : MonoBehaviour {

    private Rigidbody2D rg2d;
    private float speed = 10f;
    private bool reverse;
    private float currentSpeed;
    void Start()
    {
        speed = 10f;
        rg2d = GetComponent<Rigidbody2D>();
        rg2d.velocity = new Vector2(0, 0);
        reverse = false;
        currentSpeed = 0;

    }
    void FixedUpdate()
    {
        if(!reverse)
        {
            rg2d.AddForce(new Vector2(speed, 0));
        } else
        {
            rg2d.AddForce(new Vector2(-speed, 0));
        }
        if(rg2d.velocity.x > 100)
        {
            reverse  = true;
        }
        if(rg2d.velocity.x < -100)
        {
            reverse = false;
        }
    }
}
