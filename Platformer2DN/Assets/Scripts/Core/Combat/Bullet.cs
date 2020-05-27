using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] public int Damage;
    [SerializeField] public float Lifespan;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    public void ConfigBullet(int damage = 1, float lifespan = .5f)
    {
        Damage=damage;
        Lifespan=lifespan;
    }

    // Called from fighter -> weapon config at instantiate time
    public void SetTarget(Vector2 origin, Vector2 direction)
    {
        Vector2 calculatedTarget = CalculateDirection(origin, direction);
        rb.velocity = calculatedTarget;
    }

    private Vector2 CalculateDirection(Vector2 origin, Vector2 direction)
    {

        float x = direction.x - origin.x;
        float y = direction.y - origin.y;
        Vector2 calculatedDirection = new Vector2(x, y);
        

        return calculatedDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered with:" + collision.gameObject);
        Destroy(gameObject);
    }
    private void OnTrigger(Collider other)
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Triggered with:"+other.gameObject);
        Destroy(gameObject);
    }
}
