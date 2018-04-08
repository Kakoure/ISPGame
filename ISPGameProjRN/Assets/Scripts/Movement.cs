using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public KeyCode upkey;
    public KeyCode downkey;
    public KeyCode rightkey;
    public KeyCode leftkey;
    public float speed = 10f;
    private Rigidbody2D rgd2d;
    private int vervalues = 0;
    private int horvalues = 0;

    
        void Start()
    {
        rgd2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        vervalues = 0;
        horvalues = 0;

        if (Input.GetKey(upkey))
        {
            vervalues++;
        }
        if (Input.GetKey(downkey))
        {
            vervalues--;
        }
        if (Input.GetKey(rightkey))
        {
            horvalues++;
        }
        if (Input.GetKey(leftkey))
        {
            horvalues--;
        }
        rgd2d.velocity = new Vector2(horvalues, vervalues) * speed;
    }

}
