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
    
    public ParticleSystem sunnyCloudsParticles, mistParticles, overcastParticles, snowyParticles, rainyParticles; //_sunCloudsParticleSystem
    public List<ParticleSystem> weatherParticlesTotal = new List<ParticleSystem>();

    //public GameObject sun, thunder;
    //sun = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
    //thunder = arealight; shape = multiple cubes? import from internet? remake in blender?
    //Destroy(sun);
    //script separat pt sun si thunder atasate de 
    //gameObject.active=false

    public float audioFadeTime = 0.25f; //rata de modificare volum audio
    public AudioClip sunnyAudio, mistAudio, overcastAudio, snowyAudio, rainyAudio, thunderAudio;

    public float lightDimTime = 0.1f, minIntensity = 0f, maxIntensity = 1f, mistIntensity = 0.5f, overcastIntensity = 0.25f, snowIntensity = 0.75f; //rata de modificare a intensitatii luminii
    
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
        weatherParticlesTotal.Add(snowyParticles);
        weatherParticlesTotal.Add(rainyParticles);
    }

    public void controlEmissions(ParticleSystem ps, bool opt)
    {
        var em=ps.emission;
        em.enabled=opt;
    }

    //regions

    public IEnumerator switchWeather(){
        while(true)
        {//masina cu stari finite pentru schimbarea vremii
            switch(currentWeatherState)   
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
        Debug.Log("Initializing weather");

        weatherNum = Random.Range(0, weatherTotal);

        foreach(ParticleSystem crt in weatherParticlesTotal) //dezactiveaza toate sistemele de particule
        {
            controlEmissions(crt, false);
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
        Debug.Log("Activating "+ selectedWeather);

        //activarea sistemelor de particule adecvate
        switch(selectedWeather) 
        {
            case WeatherStates.SunnyWeather:
                controlEmissions(sunnyCloudsParticles, true);
                //activate sun gameObject
                break;

            case WeatherStates.MistWeather:
                controlEmissions(mistParticles, true);
                break;

            case WeatherStates.OvercastWeather:
                controlEmissions(overcastParticles, true);
                break;

            case WeatherStates.SnowyWeather:
                controlEmissions(snowyParticles, true);
                break;

            case WeatherStates.RainyWeather:
                controlEmissions(rainyParticles, true);
                break;

            case WeatherStates.ThunderWeather:
                controlEmissions(rainyParticles, true);
                //activate thunder gameObject 
                break;

            case WeatherStates.OThunderWeather:
                controlEmissions(overcastParticles, true);
                //activate thunder gameObject 
                break;
        }   

    }

    public void updateTimers()
    {
        //Debug.Log("Updating timers switch value: "+switchWeatherTimer+" reset value: "+resetWeatherTimer);
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
