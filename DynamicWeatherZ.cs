using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Light))]

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

    private Transform systemPosition;

    public float switchWeatherTimer=0f, resetWeatherTimer=10f; //timere de schimbare
    
    
    public ParticleSystem sunnyCloudsParticles, 
    mistParticles,
    overcastParticles, 
    snowyParticles, 
    rainyParticles; 
    public ParticleSystem [] weatherParticlesTotal;

    /* emission problems
    private ParticleSystem.EmissionModule sunnyCloudsEmission, 
    mistEmission, 
    overcastEmission, 
    snowyEmission, 
    rainyEmission;
    */

    //public List<ParticleSystem.EmissionModule> weatherEmissionsTotal = new List<ParticleSystem.EmissionModule>();

    /*
    ParticleSystem ps = GetComponent<ParticleSystem>();
        var em = ps.emission;
        em.enabled = true;
    */

    //public GameObject sun, thunder;
    //sun = GameObject.FindWithTag("Sun"); 
    //thunder = arealight; shape = multiple cubes? import from internet? remake in blender?
    //Destroy(sun);
    //script separat pt sun si thunder atasate de 
    //sun.SetActive(false);
    //overcast clouds care au colider si dispar cand ies din viziunea main camera

    public float audioFadeTime = 0.25f; //rata de modificare volum audio
    public AudioClip sunnyAudio, mistAudio, overcastAudio, snowyAudio, rainyAudio, thunderAudio;

    public float lightDimTime = 0.1f, minIntensity = 0.1f, maxIntensity = 1f, mistIntensity = 0.5f, overcastIntensity = 0.25f, snowIntensity = 0.75f; //rata de modificare a intensitatii luminii
                                    //thunderIntensity     sunnyIntensity

    public float fogChangeTime = 0.1f;
    public Color darkGrey = new Color(0.25f, 0.25f, 0.25f, 1f);
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject system = GameObject.FindGameObjectWithTag("System");
        systemPosition = system.transform;

        addAllParticles();
        
        /*emission problems
        addAllEmissions();

        sunnyCloudsEmission=sunnyCloudsParticles.emission; //asignarile emisiilor se pot face doar in metoda Start() deoarece sunt intefete si nu pot fi referite de obiecte 
        mistEmission=mistParticles.emission;
        overcastEmission=overcastParticles.emission;
        snowyEmission=snowyParticles.emission;
        rainyEmission=rainyParticles.emission;
        */

        StartCoroutine(switchWeather());
    }

    // Update is called once per frame
    void Update()
    {
        updateTimers();
    }

    // Instantiates respawnPrefab at the location
// of all game objects tagged "Respawn".

/*
public class ExampleClass : MonoBehaviour
{
    public GameObject respawnPrefab;
    public GameObject[] respawns;
    void Start()
    {
        if (respawns == null)
            respawns = GameObject.FindGameObjectsWithTag("Respawn");

        foreach (GameObject respawn in respawns)
        {
            Instantiate(respawnPrefab, respawn.transform.position, respawn.transform.rotation);
        }
    }
}
*/
    public void addAllParticles() //adauga toate sistemele de particule in lista
    {
        if(weatherParticlesTotal == null)
        {
            weatherParticlesTotal = gameObject.GetComponents<ParticleSystem>();
        }
        
        
    }

    /* emission problems
    public void addAllEmissions()
    {
        weatherEmissionsTotal.Add(sunnyCloudsEmission);
        weatherEmissionsTotal.Add(mistEmission);
        weatherEmissionsTotal.Add(overcastEmission);
        weatherEmissionsTotal.Add(snowyEmission);
        weatherEmissionsTotal.Add(rainyEmission);
    }
    */

    public void setLightLevel(float lvl)
    {
        float crtLight=GetComponent<Light>().intensity;

        if(crtLight > lvl) //reglarea intensitatii luminii curente in functie de nivelul cerut
                {
                    crtLight -= Time.deltaTime * lightDimTime; //in mod treptat
                }
                else if(crtLight < lvl)
                    {
                        crtLight += Time.deltaTime * lightDimTime;
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
            crtAudio.volume -= Time.deltaTime * audioFadeTime; //in mod treptat
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
            var em = crt.emission;
            em.enabled=false;
        }

        //sun.SetActive(false);
        //thunder.SetActive(false);

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

    public void activateWeather(WeatherStates selectedWeather)
    {
        Debug.Log("Activating "+ selectedWeather);

        switch(selectedWeather) 
        {
            case WeatherStates.SunnyWeather:
                
                var em=sunnyCloudsParticles.emission; //activarea emisiilor sistemelor de particule adecvate
                em.enabled=true;
                //activate sun gameObject

                //sun.SetActive(true);

                //setarea treptata a intensitatii luminii
                //in acest caz se doreste obtinerea intensitatii maxime a luminii
                setLightLevel(maxIntensity);

                //setarea treptata a volumului provenit de la o stare anterioara si
                //setarea clipului audio adecvat
                setAudioClip(sunnyAudio);

                break;

            case WeatherStates.MistWeather:

                RenderSettings.fogColor = Color.Lerp(Color.grey, darkGrey, Time.deltaTime * fogChangeTime); //interpolarea celor doua intensitati diferite de gri
                RenderSettings.fog = true;
                RenderSettings.fogMode = FogMode.ExponentialSquared; //activarea cetii implementate in Unity
                RenderSettings.fogDensity = 0.05f;
                

                em=mistParticles.emission;
                em.enabled=true;
                setLightLevel(mistIntensity);
                setAudioClip(mistAudio);
                break;

            case WeatherStates.OvercastWeather:

                em=overcastParticles.emission;
                em.enabled=true;
                //activate overcast clouds
                setLightLevel(overcastIntensity);
                setAudioClip(overcastAudio);
                break;

            case WeatherStates.SnowyWeather:
                
                em=snowyParticles.emission;
                em.enabled=true;
                setLightLevel(snowIntensity);
                setAudioClip(snowyAudio);
                break;

            case WeatherStates.RainyWeather:

                em=rainyParticles.emission;
                em.enabled=true;
                //activate overcast clouds
                setLightLevel(minIntensity);
                setAudioClip(rainyAudio);
                break;

            case WeatherStates.ThunderWeather:
               
                em=rainyParticles.emission;
                em.enabled=true;
                //activate thunder gameObject 
                //thunder.SetActive(true);
                setLightLevel(minIntensity);
                setAudioClip(thunderAudio);
                break;

            case WeatherStates.OThunderWeather:
                
                em=overcastParticles.emission;
                em.enabled=true;
                //controlEmissions(rainyParticles, false);
                //activate thunder gameObject
                //thunder.SetActive(true);
                //activate overcast clouds
                setLightLevel(minIntensity);
                setAudioClip(thunderAudio);
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

//Idee setare texturi 
/* 
private float[,,] alphaData;
    private TerrainData tData;
    private float percentage;
   
    private const int DESERT    = 0; //These numbers depend on the order in which
    private const int GRASS     = 1; //the textures are loaded onto the terrain
   
    void Start() {
        tData = Terrain.activeTerrain.terrainData;
       
        alphaData = tData.GetAlphamaps(0, 0, tData.alphamapWidth, tData.alphamapHeight);
       
        SetPercentage(0);
    }
   
    public void SetPercentage(double perc){
        percentage = (float) perc /100f;
       
        for(int y=0; y<tData.alphamapHeight; y++){
            for(int x = 0; x < tData.alphamapWidth; x++){
                alphaData[x, y, DESERT] = 1 - percentage;
                alphaData[x, y, GRASS] = percentage;
            }
        }
       
        tData.SetAlphamaps(0, 0, alphaData);
    }
    */