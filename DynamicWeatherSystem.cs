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
                break;
            case 1:
                weatherState = WeatherState.Sun;
                break;
            case 2:
                weatherState = WeatherState.Thunder;
                break;
            case 3:
                weatherState = WeatherState.Mist;
                break;
            case 4:
                weatherState = WeatherState.Rain;
                break;
            case 5:
                weatherState = WeatherState.Snow;
                break;
            default: 
                Debug.Log("Invalid switchWeather "+switchWeather);
                break;
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

    public void ResetWeather()
    {
        if(weatherData.Length > 0)
        {
            for(int i = 0; i < weatherData.Length; i++)
            {
                if(weatherData[i].emission.enabled)
                {
                    weatherData[i].emission.enabled = false;
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
                    RenderSettings.fogColor= Color.Lerp(weatherData[i].currentForColor, weatherData[i].fogColor, weatherData[i].fogChangeSpeed * Time.deltaTime);
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
                break;
            case WeatherState.Mist:
                activateWeather("Mist");
                break;
            case WeatherState.Sun:
                activateWeather("Sun");
                break;
            case WeatherState.Rain:
                activateWeather("Rain");
                break;
            case WeatherState.Snow:
                activateWeather("Snow");
                break;
            case WeatherState.Thunder:
                activateWeather("Thunder");  
                break;
            default:
                Debug.Log("Invalid weatherState: "+ weatherState);
                break;
        }

        yield return null;
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
