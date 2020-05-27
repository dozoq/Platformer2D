using platformer.attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public int Damage;
    public float Lifespan;

    int interval = 1;
    float nextTime = 0;
    float alifespan;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        if(Time.time>=nextTime)
        {
            print(alifespan);
            alifespan-=1;
            if(alifespan<=0)
            {

                Destroy(gameObject);
            }

            nextTime+=interval;

        }
    }

    public void ConfigBullet(int damage = 1, float lifespan = .5f)
    {
        Damage=damage;
        Lifespan=lifespan;
        alifespan=Lifespan;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered with:"+other.gameObject);
        Destroy(gameObject);
        var health =other.gameObject.GetComponent<Health>();
        if(health!=null)
        {
            health.TakeDamage(Damage);
        }
    }
   
}
