using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;


[RequireComponent(typeof(AudioSource))]

public class DynamicWeatherZ : ChangeSoil
{
    #region fields

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
    [SerializeField] private int weatherNum; 

    [SerializeField] protected float switchWeatherTimer=0f, resetWeatherTimer=10f; //timere de schimbare
    
    
    [SerializeField] private GameObject sunnyCloudsParticles, 
    mistParticles,
    overcastParticles, 
    snowyParticles, 
    rainyParticles; 
    [SerializeField] private GameObject [] weatherParticlesTotal;

    [SerializeField] private GameObject sun, thunder, player, ripples;

    [SerializeField] private Light crtLight;

    [SerializeField] private Material SkyboxSunny, SkyboxMist, SkyboxOvercast, SkyboxSnowy, SkyboxRainy, SkyboxThunder;

    [SerializeField] private float audioFadeTime = 0.5f; //rata de modificare volum audio
    [SerializeField] private AudioClip sunnyAudio, mistAudio, overcastAudio, snowyAudio, rainyAudio, thunderAudio;

    [SerializeField] private float lightDimTime = 0.1f, minIntensity = 0.1f, maxIntensity = 5f, mistIntensity = 2f, overcastIntensity = 1f, snowIntensity = 3f; //rata de modificare a intensitatii luminii
                                    //thunderIntensity     sunnyIntensity

    [SerializeField] private float fogChangeTime = 0.1f;
    [SerializeField] private Color darkGrey = new Color(0.25f, 0.25f, 0.25f, 1f);

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        addAllParticles();

        StartCoroutine(switchWeather());
        StartCoroutine(switchSoil());
    }

    // Update is called once per frame
    void Update()
    {

        updateTimers();

    }

    public void addAllParticles() //adauga toate sistemele de particule in lista
    {
        if(weatherParticlesTotal == null)
        {
            weatherParticlesTotal = GameObject.FindGameObjectsWithTag("Particles");
        }
    }

    public void setLightLevel(float lvl)
    {
        //Light crtLight = GetComponent<Light>();

        float crtLvl = crtLight.intensity;

        if(crtLvl > lvl) //reglarea intensitatii luminii curente in functie de nivelul cerut
                {
                    crtLvl -= Time.deltaTime * lightDimTime; //in mod treptat
                }
                else if(crtLvl < lvl)
                    {
                        crtLvl += Time.deltaTime * lightDimTime;
                    }
    }

   
    public void setAudioClip(AudioClip clip)
    {
        AudioSource crtAudio=GetComponent<AudioSource>();

        if(crtAudio.volume > 0  && crtAudio.clip != clip) //reducerea volumului de la o stare anterioara
        {
            crtAudio.volume -= Time.deltaTime * audioFadeTime; //in mod treptat
        }

        if(crtAudio.volume == 0) //oprirea clipului anterior si setarea clipului adecvat
        {
            crtAudio.Stop();        
            crtAudio.clip = clip;
            crtAudio.loop = true;
            crtAudio.Play();
        }

        if(crtAudio.volume < 1 && crtAudio.clip == clip) //cresterea volumului starii curente
        {
            crtAudio.volume += Time.deltaTime * audioFadeTime; //in mod treptat
        }
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
                    activateWeather(WeatherStates.SnowyWeather);
                    break;

                case WeatherStates.RainyWeather:
                    activateWeather(WeatherStates.RainyWeather);
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

    #region initializare_random

    public void initializeWeather()
    {
        //Debug.Log("Initializing weather");

        weatherNum = Random.Range(0, weatherTotal);

        foreach(GameObject crt in weatherParticlesTotal) //dezactiveaza toate particulele
        {
            crt.SetActive(false);
        }

        sun.SetActive(false); ////dezactiveaza toate gameobject-urile
        thunder.SetActive(false);
        ripples.SetActive(false);

        setLightLevel(minIntensity);

        switch(weatherNum)
        {
            case 0:
                weatherNum = (int)WeatherStates.SunnyWeather;
                currentWeatherState=WeatherStates.SunnyWeather;
                break;

            case 1:
                weatherNum = (int)WeatherStates.MistWeather;
                currentWeatherState=WeatherStates.MistWeather;
                break;

            case 2:
                weatherNum = (int)WeatherStates.OvercastWeather;
                currentWeatherState=WeatherStates.OvercastWeather;
                break;

            case 3:
                weatherNum = (int)WeatherStates.SnowyWeather;
                currentWeatherState=WeatherStates.SnowyWeather;
                break;

            case 4:
                weatherNum = (int)WeatherStates.RainyWeather;
                currentWeatherState=WeatherStates.RainyWeather;
                break;

            case 5:
                weatherNum = (int)WeatherStates.ThunderWeather;
                currentWeatherState=WeatherStates.ThunderWeather;
                break;

            case 6:
                weatherNum = (int)WeatherStates.OThunderWeather;
                currentWeatherState=WeatherStates.OThunderWeather;
                break;
        
        }
    }

    #endregion

    #region activare

    public void activateWeather(WeatherStates selectedWeather)
    {

        if(RenderSettings.fog)
        {
            RenderSettings.fog = false;
        }

        //Debug.Log("Activating "+ selectedWeather);

        //weatherParticlesTotal[weatherNum].Play(); cand era lista de ParticlesSystem

        switch(selectedWeather) 
        {
            case WeatherStates.SunnyWeather:
                
                sun.SetActive(true); //activarea gameobject-ului ce reprezinta soarele si copiii (child gameobjects) reprezentati de efectele aditionale ce apar la runtime si stralucirea (glow) acestuia
                
                sunnyCloudsParticles.SetActive(true); //activarea sistemelor de particule adecvate

                //setarea treptata a intensitatii luminii
                //in acest caz se doreste obtinerea intensitatii maxime a luminii
                setLightLevel(maxIntensity);

                //setare skyboxului inconjurator adecvat
                RenderSettings.skybox = SkyboxSunny;

                //setarea treptata a volumului provenit de la o stare anterioara si
                //setarea clipului audio adecvat
                setAudioClip(sunnyAudio);

                break;

            case WeatherStates.MistWeather:

                if(!RenderSettings.fog)
                {

                    RenderSettings.fogColor = Color.Lerp(Color.grey, darkGrey, Time.deltaTime * fogChangeTime); //interpolarea celor doua intensitati diferite de gri
                    RenderSettings.fog = true;
                    RenderSettings.fogMode = FogMode.ExponentialSquared; //activarea cetii implementate in Unity
                    RenderSettings.fogDensity = 0.05f;
                
                }

                mistParticles.SetActive(true);
                setLightLevel(mistIntensity);

                RenderSettings.skybox = SkyboxMist;

                setAudioClip(mistAudio);
                break;

            case WeatherStates.OvercastWeather:

                overcastParticles.SetActive(true);//activarea norilor gri
                setLightLevel(overcastIntensity);

                RenderSettings.skybox = SkyboxMist;
                
                setAudioClip(overcastAudio);
                break;

            case WeatherStates.SnowyWeather:
                
                snowyParticles.SetActive(true);
                setLightLevel(snowIntensity);

                RenderSettings.skybox = SkyboxSnowy;

                setAudioClip(snowyAudio);
                break;

            case WeatherStates.RainyWeather:

                rainyParticles.SetActive(true);
                overcastParticles.SetActive(true);
                ripples.SetActive(true);
                setLightLevel(minIntensity);

                RenderSettings.skybox = SkyboxRainy;

                setAudioClip(rainyAudio);
                break;

            case WeatherStates.ThunderWeather:
               
                rainyParticles.SetActive(true);
                overcastParticles.SetActive(true);
                ripples.SetActive(true);
                thunder.SetActive(true);  //activarea fulgerului in fata FPS 

            
                setLightLevel(minIntensity);

                RenderSettings.skybox = SkyboxThunder;

                setAudioClip(thunderAudio);
                break;

            case WeatherStates.OThunderWeather:
                
                overcastParticles.SetActive(true);
                thunder.SetActive(true);
                
                setLightLevel(minIntensity);

                RenderSettings.skybox = SkyboxThunder;

                setAudioClip(thunderAudio);
                break;
        }   

    }

    #endregion

    #region timer

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
            currentSoil = SoilStates.Initial;
        }

        switchWeatherTimer=resetWeatherTimer; //se reinitializeaza valoarea 


    }
}

#endregion

    