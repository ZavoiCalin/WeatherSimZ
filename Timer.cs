using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public SoilStates currentSoil;

    
    public enum SoilStates{
        Initial,
        WayTooWet,
        Wet,
        Dry,
        WayTooDry
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
