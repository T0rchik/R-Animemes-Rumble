using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;


    // Use this for initialization
    void Awake()
    {
        
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (move.x > 0 && grounded && rb2d.transform.eulerAngles.y != 0)
        {
            rb2d.transform.Rotate(new Vector2(0, -180));
        }
        else if (move.x < 0 && grounded && rb2d.transform.eulerAngles.y != 180)
        {
            rb2d.transform.Rotate(new Vector2(0, 180));
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        if (Input.GetButtonDown("Jump") && doubleJump && !grounded)
        {
            velocity.y = jumpTakeOffSpeed;
            doubleJump = false;
        }

        targetVelocity = move * maxSpeed;
    }
}
