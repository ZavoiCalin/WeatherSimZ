using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public WeatherStates currentWeatherState; //starea curenta

    public enum WeatherStates{                  //toate starile posibile
        InitialWeather,
        SunnyWeather,
        MistWeather,
        OvercastWeather,
        SnowyWeather,
        RainyWeather,
        ThunderWeather,
        OThunderWeather
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //region

    IEnumerator switchWeather(){
        while(true)
        {
            switch(currentWeatherState)   //masina cu stari finite in care se alege starea
            {
                case WeatherStates.InitialWeather:
                    initializeWeather();
                    break;
                case WeatherStates.SunnyWeather:
                    activateWeather(WeatherStates.SunnyWeather);
                    break;
                case WeatherStates.MistWeather:
                    activateWeather(WeatherStates.MistWeather);
                    break;
                case WeatherStates.OvercastWeather:
                    activateWeather(WeatherStates.OvercastWeather);
                    break;
                case WeatherStates.SnowyWeather:
                    activateWeather(WeatherStates.RainyWeather);
                    break;
                case WeatherStates.RainyWeather:
                    activateWeather(WeatherStates.SunnyWeather);
                    break;
                case WeatherStates.ThunderWeather:
                    activateWeather(WeatherStates.ThunderWeather);
                    break;
                case WeatherStates.OThunderWeather:
                    activateWeather(WeatherStates.OThunderWeather);
                    break;
            }
            yield return null;
        }
    }

    void initializeWeather()
    {
        
    }

    void activateWeather(WeatherStates selectedWeather){

    }
}
