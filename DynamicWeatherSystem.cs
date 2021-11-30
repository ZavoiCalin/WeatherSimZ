using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeatherState {Change, Sun, Thunder, Mist, Rain, Snow}

[RequireComponent(typeof(AudioSource))]

public class DynamicWeatherSystem : MonoBehaviour
{
    private int switchWeather;
    public float switchTimer=0, resetTimer=3600f, minLightIntensity=0f, maxLightIntensity=1f;
    
    public AudioSource audioSource;
    public Light sunLight;
    
    public WeatherState weatherState;
    public WeatherData [] weatherData;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();   
        RenderSettings.fog = true;
        RenderSettings.fogMode=FogMode.ExponentialSquared;
        RenderSettings.fogDensity=0f;   
    }

    public void LoadWeatherSystem()
    {
        for(int i=0; i<weatherData.Length; i++)
        {
            weatherData[i].emission = weatherData[i].particleSystem.emission;
        }

        switchTimer = resetTimer;
    }

    //Random order implementation

    public void SelectWeather()
    {
        switchWeather = Random.Range(0, System.Enum.GetValues(typeof(WeatherState)).Length-1);//length modified
        ResetWeather();
        switch(switchWeather)
        {
            case 0:
                weatherState = WeatherState.Change;
            case 1:
                weatherState = WeatherState.Sun;
            case 2:
                weatherState = WeatherState.Thunder;
            case 3:
                weatherState = WeatherState.Mist;
            case 4:
                weatherState = WeatherState.Rain;
            case 5:
                weatherState = WeatherState.Snow;
            default: 
                Debug.Log("Invalid switchWeather "+switchWeather);
        }
    }

    public void changeWeatherSettings(float lightIntensity, AudioClip audioClip)
    {
        Light tmpLight = GetComponent<Light>();

        if(tmpLight.intensity > maxLightIntensity)
        {
            tmpLight.intensity -= Time.deltaTime * lightIntensity;
        }
        else if(tmpLight.intensity < maxLightIntensity)
        {
            tmpLight.intensity += Time.deltaTime * lightIntensity;
        }

        if(weatherData[switchWeather].useAudio)
        {
            AudioSource tmpAudio = GetComponent<AudioSource>();
            if(tmpAudio.clip != audioClip){
                if(tmpAudio.volume > 0)
                {
                    tmpAudio.volume += Time.deltaTime * weatherData[switchWeather].audioFadeInTimer;
                }
                else if(tmpAudio.volume==0)
                {
                    tmpAudio.Stop();
                    tmpAudio.clip=audioClip;
                    tmpAudio.loop=true;
                    tmpAudio.Play();
                }
                else if(tmpAudio.volume < 1)
                {
                    tmpAudio.volume -= Time.deltaTime * weatherData[switchWeather].audioFadeInTimer;
                }
                
            }
        }
    }

    public void activateWeather(string weather)
    {
        if(weatherData.Length > 0)
        {
            for(int i=0; i < weatherData.Length; i++)
            {
                if(weatherData[i].particleSystem && weatherData[i].name == weather)
                {
                    weatherData[i].emission.enabled=true;//enable modified
                    weatherData[i].fogColor= RenderSettings.fogColor;
                    RenderSetting.fogColor= Color.Lerp(weatherData[i].currentForColor, weatherData[i].fogColor, weatherData[i].fogChangeSpeed * Time.deltaTime);
                    changeWeatherSettings(weatherData[i].lightIntensity, weatherData[i].weatherAudio);
                }
            }
        }
    }

    public IEnumerator StartDynamicWeather()
    {
        switch(weatherState){
            case WeatherState.Change:
                SelectWeather();
            case WeatherState.Mist:
                activateWeather("Mist");
            case WeatherState.Sun:
                activateWeather("Sun");
            case WeatherState.Rain:
                activateWeather("Rain");
            case WeatherState.Snow:
                activateWeather("Snow");
            case WeatherState.Thunder:
                activateWeather("Thunder");  
            default:
                Debug.Log("Invalid weatherState: "+ weatherState);
        }
    }

    // Start is called before the first frame update
    void Start() 
    {
        LoadWeatherSystem();
        StartCoroutine(nameof(StartDynamicWeather));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        switchTimer -= Time.deltaTime;

        if(switchTimer<=0) 
        {
            switchTimer=0;
            weatherState=WeatherState.Change;
            switchTimer=resetTimer;
        }
        else return;
    }

}
