using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWeatherZ : MonoBehaviour
{
    
    public WeatherStates currentWeatherState; //starea curenta _weatherState

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
    private int weatherNum; //_switchWeather

    public float switchWeatherTimer=0f, resetWeatherTimer=20f; //timere de schimbare
    
    public ParticleSystem sunnyCloudsParticles, mistParticles, overcastParticles, snowParticles, rainParticles; //_sunCloudsParticleSystem
    public List<ParticleSystem> weatherParticlesTotal = new List<ParticleSystem>();

    //public GameObject sun, thunder;
    //sun = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
    //thunder arealight shape = multiple cubes? import from internet? remake in blender?
    //Destroy(sun);
    //script separat pt sun si thunder

    // Start is called before the first frame update
    void Start()
    {
        addAllParticles();
    }

    // Update is called once per frame
    void Update()
    {
        updateTimers();
    }

    public void addAllParticles() //adauga toate sistemele de particule in lista
    {
        weatherParticlesTotal.Add(sunnyCloudsParticles);
        weatherParticlesTotal.Add(mistParticles);
        weatherParticlesTotal.Add(overcastParticles);
        weatherParticlesTotal.Add(snowParticles);
        weatherParticlesTotal.Add(rainParticles);
    }

    //regions

    public IEnumerator switchWeather(){
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

    public void initializeWeather()
    {
        weatherNum = Random.Range(0, weatherTotal);

        foreach(ParticleSystem crt in weatherParticlesTotal) //dezactiveaza toate sistemele de particule
        {
            var em = crt.emission;
            em.enabled = false;
        }

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

    public void activateWeather(WeatherStates selectedWeather)
    {

    }

    public void updateTimers()
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
