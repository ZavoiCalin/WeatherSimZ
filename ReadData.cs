/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class ReadData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        

        using(var reader = new StreamReader(@"D:\EGIOC\Prototype_Weather_Simulator\Assets\Files"))
        {
            List<string> listA = new List<string>();
            
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                listA.Add(values[0]);
            }
        }

        /*
        foreach(var crt in listA)
        {
            Debug.log(crt);
        }
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/

using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;


public class RuntimeText: MonoBehaviour
{
    /*
   public static void WriteString()
   {
       string path = Application.persistentDataPath + "/data.txt";
       //Write some text to the test.txt file
       StreamWriter writer = new StreamWriter(path, true);
       writer.WriteLine("Test");
        writer.Close();
       StreamReader reader = new StreamReader(path);
       //Print the text from the file
       Debug.Log(reader.ReadToEnd());
       reader.Close();
    }
    */
   public static void ReadString()
   {
       List<string> dateTotal = new List<string>();
       string path = "D:/EGIOC/Prototype_Weather_Simulator/Assets/Files/data.txt";
       //Read the text from directly from the test.txt file
       StreamReader reader = new StreamReader(path);
       Debug.Log(reader.ReadToEnd());

       while (!reader.EndOfStream)
       {
       var line = reader.ReadLine();
       var values = line.Split(',');
       dateTotal.Add(values[0]); //AH
       dateTotal.Add(values[1]); //T
       dateTotal.Add(values[2]); //SM
       dateTotal.Add(values[3]); //RV
       dateTotal.Add(values[4]); //AP
       }
       reader.Close();
   }

   void Start() {
       ReadString();
       
   }
}
