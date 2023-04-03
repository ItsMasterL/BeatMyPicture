using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class OpenFighter : MonoBehaviour
{
    public GameObject check;
    public static string carryover;
    public static string shortcarryover;
    public static string templateselected;
    public static bool newtemplate;

    public void Create(GameObject obj)
    {
        string input = obj.GetComponent<TMP_InputField>().text;
        if (check.GetComponent<NameCheck>().okay == true && templateselected != "")
        {
            Directory.CreateDirectory(Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters" + Path.DirectorySeparatorChar + input);
            carryover = Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters" + Path.DirectorySeparatorChar + input;
            shortcarryover = "Fighters" + Path.DirectorySeparatorChar + input;
            newtemplate = true;
            Screen.orientation = ScreenOrientation.Landscape;
            Debug.Log("Template is " + templateselected);
        }
    }

    public void Open()
    {
        carryover = gameObject.GetComponent<LoadStage>().FilePath;
        int removal = Application.persistentDataPath.ToCharArray().Length + 1;
        shortcarryover = carryover.Remove(0, removal);
        /*if (!Directory.Exists(carryover + Path.DirectorySeparatorChar + "Cutouts"))
        {
            Directory.CreateDirectory(carryover + Path.DirectorySeparatorChar + "Cutouts");
            Debug.Log("Created missing Cutouts folder");
        }*/
        newtemplate = false;
        Screen.orientation = ScreenOrientation.Landscape;
        SceneManager.LoadScene("FighterCreate");
    }
}
