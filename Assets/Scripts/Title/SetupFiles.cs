using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using System.IO;
using TMPro;

public class SetupFiles : MonoBehaviour
{
    string path;
    public TextMeshProUGUI display;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        path = Application.persistentDataPath;
        display.text = "Files can be found in " + path;
        Directory.CreateDirectory(path);
        Directory.CreateDirectory(path + Path.DirectorySeparatorChar + "Fighters");
        Directory.CreateDirectory(path + Path.DirectorySeparatorChar + "Stages");
        Directory.CreateDirectory(path + Path.DirectorySeparatorChar + "Templates");
    }
}
