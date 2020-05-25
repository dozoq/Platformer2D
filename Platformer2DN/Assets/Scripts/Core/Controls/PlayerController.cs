using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public float maxSpeed = 7f;
    public float jumpTakeOffSpeed = 8f;

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // TODO: Change to getButtownDown and getbuttonDown to Jump from project settings
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        targetVelocity = move * maxSpeed;
    }
}
