using platformer.attributes;
using platformer.utils;
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
            //Get rigid body for physics
            rb = GetComponent<Rigidbody2D>();
            //Set gameobject tag to bullet for colision condition
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
            if(vfxObject != null)
            {
                vfxManager=vfxObject.GetComponent<VisualEffect>();
                

            }
        }

        // Called from fighter -> weapon config at instantiate time
        public void SetTarget(Vector2 origin, Vector2 direction)
        {
            bulletDirection = CalculateDirection(origin, direction).normalized;
            //if vgxManager exists and has velocity field
            if( vfxManager!=null && vfxManager.HasFloat("VelX")&&vfxManager.HasFloat("VelY"))
            {
                //set velocity field to bullet direction multiplied by 10
                    vfxManager.SetFloat("VelX", bulletDirection.x*Speed);
                
                    vfxManager.SetFloat("VelY", bulletDirection.y*Speed);
                
                //Instantiate bullet VFX prefab with relative transform (0, 0, -0.1f)
                var vfxTemp = Instantiate(vfxObject, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f), Quaternion.identity);
                //Chain vfx under bullet as a child
                vfxTemp.transform.parent=gameObject.transform;
            }

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
            //if this is enemy bullet and don't hit enemy or other bullet
            if(isEnemyBullet && !other.CompareTag("Enemy") &&!other.CompareTag("Bullet"))
            {
                //destroy bullet
                Destroy(gameObject); 
            }
            //if it not enemy bullet and don't hit bullet and (player or can damage player)
            else if (!isEnemyBullet && other.tag!="bullet" && other.tag != "Player" || canDamagePlayer)
            {
                //Destroys bullet on hit
                Destroy(gameObject);
            }

            //take (C) health from hitted target(if exists)
            var health = other.gameObject.GetComponent<Health>();
            //If health exists and it is enemy bullet
            if(isEnemyBullet &health!=null)
            {
                //if hit player
                if(other.CompareTag("Player"))
                {
                    //do damage
                    health.TakeDamage(Damage);
                }
            }
            //if health exists and (hit other object than player or can damage player)
            else if (health != null && (!other.CompareTag("Player") || canDamagePlayer))
            {
                //Do damage
                health.TakeDamage(Damage);
            }


            //take target rigid body (if exists)
            var targetrb = other.gameObject.GetComponent<Rigidbody2D>();
            //If rigid body exists and it is enemy bullet
            if(isEnemyBullet&targetrb!=null)
            {
                //if hit player
                if(other.CompareTag("Player"))
                {
                    //add force to target rigid body as impulse
                    targetrb.AddForce(bulletDirection*Damage, ForceMode2D.Impulse);
                }
            }
            //if it isn't enemy bullet and rigid body exists and (don't hit player or can damage player)
            else if (targetrb != null && (!other.CompareTag("Player") || canDamagePlayer))
            {
                //Add force to target rigid body as impulse
                targetrb.AddForce(bulletDirection * Damage, ForceMode2D.Impulse);
            }
        }

    }

}