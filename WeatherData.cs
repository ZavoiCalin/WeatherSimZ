using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherData 
{
    public string name;

    public ParticleSystem particleSystem;

    //[HideInInspector]
    public ParticleSystem.EmissionModule emission;

    public bool useAudio;
    public AudioClip weatherAudio;
    public float audioFadeInTimer, lightIntensity, lightDimTimer, fogChangeSpeed;

    public Transform windzone;

    public Color fogColor, currentForColor;
    
    
}
