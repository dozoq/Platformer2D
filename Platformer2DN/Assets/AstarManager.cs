using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating("GraphUpdate",1f,1f);
        #if UNITY_EDITOR
                QualitySettings.vSyncCount=0;  // VSync must be disabled
                Application.targetFrameRate=60;
        #endif
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
