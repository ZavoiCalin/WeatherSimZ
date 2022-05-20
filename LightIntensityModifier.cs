using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityModifier : MonoBehaviour
{
    
    public Light crt;

    IEnumerator delayLightChange()
    {
        float intens = crt.intensity;

        //Print the time of when the function is first called.
        intens = Mathf.PingPong(Time.time, 8);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(8);

        //After we have waited 5 seconds print the time again.
        intens = 0.1f;
    } 

    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(delayLightChange());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
