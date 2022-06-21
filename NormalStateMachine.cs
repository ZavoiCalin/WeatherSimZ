using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStateMachine : DynamicWeatherZ
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        activateWeather(WeatherStates.MistWeather);
        activateSoil(SoilStates.WayTooWet);
    }
}
