using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class OpenStage : MonoBehaviour
{
    public GameObject check;
    public static string carryover;
    public static bool newtemplate;

    public void Create(GameObject obj)
    {
        string input = obj.GetComponent<TMP_InputField>().text;
        if (check.GetComponent<NameCheck>().okay == true)
        {
            Directory.CreateDirectory(Application.persistentDataPath + Path.DirectorySeparatorChar + "Stages" + Path.DirectorySeparatorChar + input);
            carryover = Application.persistentDataPath + Path.DirectorySeparatorChar + "Stages" + Path.DirectorySeparatorChar + input;
            newtemplate = true;
            Screen.orientation = ScreenOrientation.Landscape;
            SceneManager.LoadScene("StageCreate");
        }
    }

    public void Open()
    {
        carryover = gameObject.GetComponent<LoadStage>().FilePath;
        /*if (!Directory.Exists(carryover + Path.DirectorySeparatorChar + "Cutouts"))
        {
            Directory.CreateDirectory(carryover + Path.DirectorySeparatorChar + "Cutouts");
            Debug.Log("Created missing Cutouts folder");
        }*/
        newtemplate = false;
        Screen.orientation = ScreenOrientation.Landscape;
        SceneManager.LoadScene("StageCreate");
    }
}
