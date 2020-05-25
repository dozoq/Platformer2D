using platformer.attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDieable
{
    private float time = 0.0f;
    private float interpolationPeriod = 0.1f;

    public void die() 
    {
        Destroy( this.gameObject );
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        time+=Time.deltaTime;
        if (time>=interpolationPeriod)
        {
            time=0.0f;
            Health enemyHealth = this.GetComponent<Health>();
            if (enemyHealth!=null)
            {
                Debug.Log( "Take dmg" );
                enemyHealth.TakeDamage( 1 );
            }
        }
    }
}
