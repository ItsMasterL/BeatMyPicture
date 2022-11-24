using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class OpenTemplate : MonoBehaviour
{
    public GameObject check;
    public static string carryover;
    public static bool newtemplate;

    public void Create(GameObject obj)
    {
        string input = obj.GetComponent<TMP_InputField>().text;
        if (check.GetComponent<NameCheck>().okay == true)
        {
            Directory.CreateDirectory(Application.persistentDataPath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + input);
            Directory.CreateDirectory(Application.persistentDataPath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + input + Path.DirectorySeparatorChar + "Cutouts");
            Directory.CreateDirectory(Application.persistentDataPath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + input + Path.DirectorySeparatorChar + "Frames");
            Directory.CreateDirectory(Application.persistentDataPath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + input + Path.DirectorySeparatorChar + "Sets");
            carryover = Application.persistentDataPath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + input;
            newtemplate = true;
            SceneManager.LoadScene("TempCreate");
        }
    }

    public void Open()
    {
        carryover = gameObject.GetComponent<LoadTemplate>().FilePath;
        if (!Directory.Exists(carryover + Path.DirectorySeparatorChar + "Cutouts"))
        {
            Directory.CreateDirectory(carryover + Path.DirectorySeparatorChar + "Cutouts");
            Debug.Log("Created missing Cutouts folder");
        }
        if (!Directory.Exists(carryover + Path.DirectorySeparatorChar + "Frames"))
        {
            Directory.CreateDirectory(carryover + Path.DirectorySeparatorChar + "Frames");
            Debug.Log("Created missing Frames folder");
        }
        if (!Directory.Exists(carryover + Path.DirectorySeparatorChar + "Sets"))
        {
            Directory.CreateDirectory(carryover + Path.DirectorySeparatorChar + "Sets");
            Debug.Log("Created missing Sets folder");
        }
        newtemplate = false;
        SceneManager.LoadScene("TempCreate");
    }
}
