using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public float maxEdgeGrabDistance = 0.5f;


    // Use this for initialization
    void Awake()
    {
        
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (!edgeGrab)
        {
            if (move.x > 0 && grounded && rb2d.transform.eulerAngles.y != 0)
            {
                rb2d.transform.Rotate(new Vector2(0, -180));
            }
            else if (move.x < 0 && grounded && rb2d.transform.eulerAngles.y != 180)
            {
                rb2d.transform.Rotate(new Vector2(0, 180));
            }
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
        
        // Edge Grab Check, should probably change the Y velocity check.
        if (!grounded && !edgeGrab && velocity.y < 0)
        {
            for (int i = 0; i < edges.Length; i++)
            {
                //If the edge is inbetween half player's height(from top to middle) and close enough to grab, set position to the edge and edge grab to true.
                if (edges[i].transform.position.y > rb2d.transform.position.y
                    && edges[i].transform.position.y < rb2d.transform.position.y + (rb2d.transform.lossyScale.y / 2)
                    && Vector2.Distance(edges[i].transform.position, rb2d.position) < maxEdgeGrabDistance)
                {
                    // Left, or Right Edge, a bit more than 1 since it was getting stuck on the ground.
                    if (edges[i].transform.position.x > rb2d.position.x)
                    {
                        sideOfEdge = -1.05f;
                    }
                    else
                    {
                        sideOfEdge = 1.05f;
                    }
                    //puts player at, and facing the edge
                    rb2d.transform.position = new Vector2(edges[i].transform.position.x + (rb2d.transform.lossyScale.x / 2 * sideOfEdge), edges[i].transform.position.y - (rb2d.transform.lossyScale.y / 2));
                    if (sideOfEdge > 0)
                    {
                        rb2d.transform.Rotate(new Vector2(0, -180));
                    }
                    else
                    {
                        rb2d.transform.Rotate(new Vector2(0, 180));
                    }
                    edgeGrab = true;
                }
            }
        }
    }
}
