using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarManager : MonoBehaviour
{
    private IEnumerator coroutine;
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine("UpdateGraph");
    #if UNITY_EDITOR
        QualitySettings.vSyncCount=0;  // VSync must be disabled
                Application.targetFrameRate=60;
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
    }
    private IEnumerator UpdateGraph()
    {
        while(true)
        {
            yield return new WaitForSeconds(5f);
            AstarPath.active.Scan();
        }
    }
}
