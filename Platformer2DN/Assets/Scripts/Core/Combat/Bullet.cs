using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    public void ConfigBullet()
    {

    }

    // Called from fighter -> weapon config at instantiate time
    public void SetTarget(Vector2 origin, Vector2 direction, int bulletDamage)
    {
        Vector2 calculatedTarget = CalculateDirection(origin, direction);
        rb.velocity = calculatedTarget;
    }

    private Vector2 CalculateDirection(Vector2 origin, Vector2 direction)
    {
        print("hand: " +origin);
        print("mouse direction" +direction);
        float x = direction.x - origin.x;
        float y = direction.y - origin.y;
        Vector2 calculatedDirection = new Vector2(x, y);
        print("direction calculated "  + calculatedDirection);
        

        return calculatedDirection;
    }
}
