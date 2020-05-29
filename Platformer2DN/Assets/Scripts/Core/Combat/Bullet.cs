using platformer.attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.VFX;

namespace platformer.combat
{
    public class Bullet : MonoBehaviour
    {

        //Bullet config properties
        public int Damage;
        public float Lifespan;
        public float Speed;
        public bool canDamagePlayer;
        public bool isEnemyBullet = false;
        public GameObject vfxObject;

        //VFX properties
        //Use to do diffrent calculations on VFX and pass through variables
        //Handles vfx effect
        private VisualEffectAsset vfx;
        //Handles vfx player
        private VisualEffect vfxManager;

        //Standard Rigidbody2D to handle bullet moving
        Rigidbody2D rb;



        //Timer properties
        //Time on start of timer
        private float time = 0.0f;
        //how many seconds must pass to do tick
        public float interpolationPeriod = 0.1f;

        //bullet util proporties
        //Actual life of bullet
        float alifespan;
        private Vector2 bulletDirection;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            gameObject.tag="Bullet";
        }

        private void Update()
        {
            //Add passed time to timer
            time+=Time.deltaTime;

            //check if timer passed interpolation period
            if(time>=interpolationPeriod)
            {
                //Timer reset
                time=0.0f;

                //remove from actual lifespan passed period
                alifespan-=interpolationPeriod;

                //if bullet live too long
                if(alifespan<=0)
                {
                    //Destroy it
                    Destroy(gameObject);
                }
            }
        }

        //Confugures bullet
        //called from WeaponConfig.LaunchBullet()
        public void ConfigBullet(int damage = 1, float lifespan = .5f, float speed = 15f, bool canDoDamageToPlayer = false, GameObject bulletVFX = null, bool isEnemyFireing = false)
        {
            //Standard Attributes set
            Damage = damage;
            Lifespan = lifespan;
            alifespan = Lifespan;
            Speed = speed;
            canDamagePlayer = canDoDamageToPlayer;
            isEnemyBullet=isEnemyFireing;

            //VFX handling
            /*Take bullet prefab(
             * Effects are 3D, so it need to have Z<0 to be visible,
             * if we instantiate vfx prefab as child we can do this without affecting collision)*/
            vfxObject = bulletVFX;
            //Instantiate bullet VFX prefab with relative transform (0, 0, -0.1f)
            var vfxTemp = Instantiate(vfxObject, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f), Quaternion.identity);
            //Chain vfx under bullet as a child
            vfxTemp.transform.parent = gameObject.transform;
        }

        // Called from fighter -> weapon config at instantiate time
        public void SetTarget(Vector2 origin, Vector2 direction)
        {
            bulletDirection = CalculateDirection(origin, direction).normalized;
            rb.velocity = bulletDirection * Speed;
        }


        // Calculate direction where bullet should go, where origin - barrel position, direction - mouse position
        private Vector2 CalculateDirection(Vector2 origin, Vector2 direction)
        {
            float x = direction.x - origin.x;
            float y = direction.y - origin.y;
            return new Vector2(x, y);
        }

        //Handles collision effect
        private void OnTriggerEnter2D(Collider2D other)
        {
            //Check if colliosion is on player
            //Pass if hit player or can damage player
            if(isEnemyBullet && !other.CompareTag("Enemy") &&!other.CompareTag("Bullet"))
            {
                Destroy(gameObject); 
            }
            else if (!isEnemyBullet && other.tag!="bullet" && other.tag != "Player" || canDamagePlayer)
            {
                //Destroys bullet on hit
                Destroy(gameObject);
            }

            //take (C) health from hitted target(if exists)
            var health = other.gameObject.GetComponent<Health>();
            //If health exists and it's not player or if health exists and bullet can damage player
            if(isEnemyBullet &health!=null)
            {
                if(other.CompareTag("Player"))
                {
                    health.TakeDamage(Damage);
                }
            }
            else if (health != null && (!other.CompareTag("Player") || canDamagePlayer))
            {
                //Do damage
                health.TakeDamage(Damage);
            }


            var targetrb = other.gameObject.GetComponent<Rigidbody2D>();
            if(isEnemyBullet&targetrb!=null)
            {
                if(other.CompareTag("Player"))
                {
                    targetrb.AddForce(bulletDirection*Damage, ForceMode2D.Impulse);
                }
            }
            else if (targetrb != null && (!other.CompareTag("Player") || canDamagePlayer))
            {
                targetrb.AddForce(bulletDirection * Damage, ForceMode2D.Impulse);
            }
        }

    }

}