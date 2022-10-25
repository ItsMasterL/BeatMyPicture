using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateTemplate : MonoBehaviour
{
    public GameObject check;
    public static string carryover;
    public static bool newtemplate;

    public void Create(GameObject obj)
    {
        string input = obj.GetComponent<TMP_InputField>().text;
        if (check.GetComponent<NameCheck>().okay == true)
        {
            Directory.CreateDirectory(Application.persistentDataPath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "." + input);
            carryover = input;
            newtemplate = true;
            SceneManager.LoadScene("TempCreate");
        }
    }
}
