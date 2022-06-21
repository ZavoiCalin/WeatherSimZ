using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSoil : MonoBehaviour
{
    [SerializeField] private Material WayTooWetMat, WetMat, DryMat, WayTooDryMat;
    [SerializeField] private Terrain crtTerrain;

    private int soilTotal=SoilStates.GetNames(typeof(SoilStates)).Length; //numarul de stari de sol din enum
    private int soilNum;

    public SoilStates currentSoil;

    
    public enum SoilStates{
        Initial,
        WayTooWet,
        Wet,
        Dry,
        WayTooDry
    }
 

    public IEnumerator switchSoil(){
        while(true)
        {
            switch(currentSoil)
            {
                case SoilStates.Initial:
                    initializeSoil();
                    break;

                case SoilStates.WayTooWet:
                    activateSoil(SoilStates.WayTooWet);
                    break;

                case SoilStates.Wet:
                    activateSoil(SoilStates.Wet);
                    break;

                case SoilStates.Dry:
                    activateSoil(SoilStates.Dry);
                    break;

                case SoilStates.WayTooDry:
                    activateSoil(SoilStates.WayTooDry);
                    break;
            }

            yield return null;
        }

        
    }

    public void initializeSoil()
    {
        soilNum = Random.Range(0, soilTotal);
        
        switch(soilNum)
        {
            case 0:
                soilNum++;
                currentSoil=SoilStates.WayTooWet;
                break;

            case 1:
                soilNum++;
                currentSoil=SoilStates.Wet;
                break;

            case 2:
                soilNum++;
                currentSoil=SoilStates.Dry;
                break;

            case 3:
                soilNum++;
                currentSoil=SoilStates.WayTooDry;
                break;
        
        }

    }

    public void activateSoil(SoilStates selectedSoil)
    {
        

        switch(selectedSoil)
        {
            case SoilStates.WayTooWet:
                crtTerrain.materialTemplate = WayTooWetMat; //modificare material sol
                break;
            case SoilStates.Wet:
                crtTerrain.materialTemplate = WetMat;
                break;
            case SoilStates.Dry:
                crtTerrain.materialTemplate = DryMat;
                break;
            case SoilStates.WayTooDry:
                crtTerrain.materialTemplate = WayTooDryMat;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
