using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating("GraphUpdate",1f,1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void GraphUpdate()
    {
        AstarPath.active.Scan();
    }
}
