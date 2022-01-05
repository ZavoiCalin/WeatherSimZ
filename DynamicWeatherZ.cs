using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWeatherZ : MonoBehaviour
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
    };

    private int weatherTotal=WeatherStates.GetNames(typeof(WeatherStates)).Length; //numarul de stari de vreme din enum
    private int weatherNum;
    public float switchWeatherTimer=0f, resetWeatherTimer=20f; //timere de schimbare
    public ParticleSystem sunnyClouds;
    //public GameObject sun;
    //sun = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
    //Destroy(sun);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateTimers();
    }

    //regions

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

    //implementare random

    void initializeWeather()
    {
        weatherNum = Random.Range(0, weatherTotal);

        switch(weatherNum)
        {
            case 0:
            weatherNum = (int)WeatherStates.SunnyWeather;
            break;

            case 1:
            weatherNum = (int)WeatherStates.MistWeather;
            break;

            case 2:
            weatherNum = (int)WeatherStates.OvercastWeather;
            break;

            case 3:
            weatherNum = (int)WeatherStates.SnowyWeather;
            break;

            case 4:
            weatherNum = (int)WeatherStates.RainyWeather;
            break;

            case 5:
            weatherNum = (int)WeatherStates.ThunderWeather;
            break;

            case 6:
            weatherNum = (int)WeatherStates.OThunderWeather;
            break;
        
        }
    }

    void activateWeather(WeatherStates selectedWeather){

    }

    void updateTimers()
    {
        Debug.Log("Updating timers switch value: "+switchWeatherTimer+" reset value: "+resetWeatherTimer);
        switchWeatherTimer -= Time.deltaTime; //o data per frame se actualizeaza timer-ul pentru ca valoarea float-ului sa corespunda cu numarul de secunde
        
        if(switchWeatherTimer < 0)
        {
            switchWeatherTimer = 0; //resetarea timer-ului
        }

        if(switchWeatherTimer > 0)
            return;

        if(switchWeatherTimer==0)
        {
            currentWeatherState=WeatherStates.InitialWeather;
        }

        switchWeatherTimer=resetWeatherTimer; //se reinitializeaza valoarea 


    }
}
