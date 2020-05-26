using System;
using System.Collections;
using System.Collections.Generic;
using platformer.combat;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public float maxSpeed = 7f;
    public float jumpTakeOffSpeed = 8f;

    [SerializeField] private GameObject bulletPreFab;

    private Fighter fighter;

    protected override void Awake()
    {
        base.Awake();
        fighter = GetComponent<Fighter>();
    }

    protected override void Update()
    {
        base.Update();
        if (fighter == null)
        {
            Debug.LogError("Fighter not initialized in PlayerController (Possible race conditions)");
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            fighter.Attack(Camera.main.ScreenPointToRay(Input.mousePosition));
        }
        
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded) // TODO: Change to getButtownDown and getbuttonDown to Jump from project settings
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

        targetVelocity = move * maxSpeed;
    }
}
