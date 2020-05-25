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

            this.GetComponentInChildren<Health>().TakeDamage( 1 );
        }
    }
}
