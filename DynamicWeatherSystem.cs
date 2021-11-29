using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeatherState {Change, Sun, Thunder, Mist, Rain, Snow }

[RequireComponent(typeof(AudioSource))]

public class DynamicWeatherSystem : MonoBehaviour
{
    public float switchTimer=0, resetTimer=3600f, minLightIntensity=0f, maxLightIntensity=1f;
    
    public AudioSource audioSource;
    public Light sunLight;
    
    public WeatherState weatherState;
    public WeatherData [] weatherData;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();   
        RenderSettings.fog = true;
        RenderSettings.fogMode=FogMode.ExponentialSquared;
        RenderSettings.fogDensity=0f;   
    }

    void LoadWeatherSystem()
    {
        for(int i=0; i<weatherData.Length; i++)
        {
            //weatherData[i]=
        }
    }

    // Start is called before the first frame update
    void Start() 
    {
        LoadWeatherSystem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
