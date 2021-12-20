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
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
