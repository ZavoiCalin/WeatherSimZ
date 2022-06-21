using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


public class ReadData : MonoBehaviour
{
    public static string [] firstData = new string[6];
    public static string [] secondData = new string[6];
    public static Int32 [] diffData = new Int32[6];

    public static int set = 0;

    public static int [] firstNr = new int[6];
    public static int [] secondNr = new int[6];

    public static bool ended = false;

    IEnumerator delayRead()
    {
        
        
        readSetOfData();
        
        yield return new WaitForSeconds(5);

            if(ended)
                Debug.Log("Done with all data");
    }


   
   public static void readSetOfData()
   {
       
       string pathC = Application.dataPath + "/Files/serial.txt"; //computer
       //string pathA = Application.persistentDataPath + "SERIAL_20220621_154414.txt";//trebuie fix dupa ce am dat drumu la log in terminal
      

       StreamReader reader = new StreamReader(pathC);
       //Debug.Log(reader.ReadToEnd());

       while (!reader.EndOfStream)
       {
       
       set++;
       var line = reader.ReadLine();
       
       var values = line.Split(',');

        for(var i = 0; i < 6; i++)
        {
            
            if(set == 1){
                firstData[i] = values[i];
                
            }

            if(set == 2){
           
                secondData[i] = values[i];
  
              
            }

            if(set == 3)
            {
                
                firstNr[i] = Int32.Parse(firstData[i]);
                secondNr[i] = Int32.Parse(secondData[i]);

                diffData[i] = secondNr[i] - firstNr[i];
                Debug.Log(diffData[i] + " = " + secondData[i] + " - " + firstData[i]);

                

                firstData[i] = secondData[i];
                secondData[i] = values[i];
                
            }

            if(set >= 4)
            {
                firstData[i] = secondData[i];
                secondData[i] = values[i];

                firstNr[i] = Int32.Parse(firstData[i]);
                secondNr[i] = Int32.Parse(secondData[i]);

                diffData[i] = secondNr[i] - firstNr[i];
                Debug.Log(diffData[i] + " = " + secondData[i] + " - " + firstData[i]);
            }



            /*
            firstData[i] = secondData[i];
            secondData[i] = values[i];*/

            //Debug.Log(values[i]);

            
        }
            ended = true;
       }

       reader.Close();
        
   }

   void Start() {
        StartCoroutine(delayRead());
       
   }
}
