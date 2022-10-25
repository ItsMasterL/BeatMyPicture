using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONReader : MonoBehaviour
{
    private string jsonString;

    private void Start()
    {
        jsonString = File.ReadAllText(Application.dataPath + "/Resources/");
    }

}
