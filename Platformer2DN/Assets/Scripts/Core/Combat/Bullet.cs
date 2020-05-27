using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float lifespane = 0.5f;
    [SerializeField] float speed = 0.5f;

    Rigidbody2D rb;
    Vector2 target;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called from fighter -> weapon config at instantiate time
    public void SetTarget(Vector2 origin, Vector2 direction, int bulletDamage)
    {
        print(origin);
        print(direction);
        Vector2 calculatedTarget = CalculateDirection(origin, direction);
        print(calculatedTarget);
        rb.velocity = calculatedTarget;
    }

    private Vector2 CalculateDirection(Vector2 origin, Vector2 direction)
    {
        float x = direction.x - origin.x;
        float y = direction.y - origin.y;
        Vector2 calculatedDirection = new Vector2(x, y);
        

        return calculatedDirection;
    }
}
