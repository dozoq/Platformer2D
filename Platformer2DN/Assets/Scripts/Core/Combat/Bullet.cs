using platformer.attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public int Damage;
    public float Lifespan;
    public float Speed;
    public bool canDamagePlayer;
    public VisualEffectAsset vfx;
    public GameObject vfxObject;

    [SerializeField]private VisualEffect vfxManager; 
    private float time = 0.0f;
    public float interpolationPeriod = 0.1f;
    float alifespan;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        time+=Time.deltaTime;

        if(time>=interpolationPeriod)
        {
            time=0.0f;

            alifespan-=0.1f;
            if(alifespan<=0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void ConfigBullet(int damage = 1, float lifespan = .5f, float speed = 15f, bool canDoDamageToPlayer = false, GameObject bulletVFX = null)
    {
        Damage=damage;
        Lifespan=lifespan;
        alifespan=Lifespan;
        Speed=speed;
        canDamagePlayer=canDoDamageToPlayer;
        //vfx=bulletVFX;
        //vfxManager.visualEffectAsset=vfx;
        //vfxManager.Play();
        vfxObject=bulletVFX;
        var vfxTemp= Instantiate(vfxObject,new Vector3(transform.position.x,transform.position.y,transform.position.z-0.1f),Quaternion.identity);
        vfxTemp.transform.parent=gameObject.transform;
    }

    // Called from fighter -> weapon config at instantiate time
    public void SetTarget(Vector2 origin, Vector2 direction)
    {
        Vector2 calculatedTarget = CalculateDirection(origin, direction);

        rb.velocity = calculatedTarget.normalized*Speed;
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
        if(other.tag!="Player"||canDamagePlayer)
        {
            Destroy(gameObject);
        }
        var health =other.gameObject.GetComponent<Health>();
        if(health!=null && (other.tag != "Player" || canDamagePlayer))
        {
            health.TakeDamage(Damage);
        }
    }
   
}
