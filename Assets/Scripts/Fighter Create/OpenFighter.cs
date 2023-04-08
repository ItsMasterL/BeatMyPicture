﻿using System.Collections;
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
    public static bool newfighter;

    public void Create(GameObject obj)
    {
        string input = obj.GetComponent<TMP_InputField>().text;
        if (check.GetComponent<NameCheck>().okay == true)
        {
            Directory.CreateDirectory(Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters" + Path.DirectorySeparatorChar + input);
            Directory.CreateDirectory(Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters" + Path.DirectorySeparatorChar + input + Path.DirectorySeparatorChar + "Pictures");
            Directory.CreateDirectory(Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters" + Path.DirectorySeparatorChar + input + Path.DirectorySeparatorChar + "Sounds");
            carryover = Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters" + Path.DirectorySeparatorChar + input;
            shortcarryover = "Fighters" + Path.DirectorySeparatorChar + input;
            newfighter = true;
            Screen.orientation = ScreenOrientation.Landscape;
            Debug.Log("Template is " + templateselected);
            SceneManager.LoadScene("FighterCreate");
        }
    }

    public void Open()
    {
        carryover = gameObject.GetComponent<LoadStage>().FilePath;
        int removal = Application.persistentDataPath.ToCharArray().Length + 1;
        shortcarryover = carryover.Remove(0, removal);
        if (!Directory.Exists(carryover + Path.DirectorySeparatorChar + "Pictures"))
        {
            Directory.CreateDirectory(carryover + Path.DirectorySeparatorChar + "Pictures");
            Debug.Log("Created missing Pictures folder");
        }
        if (!Directory.Exists(carryover + Path.DirectorySeparatorChar + "Sounds"))
        {
            Directory.CreateDirectory(carryover + Path.DirectorySeparatorChar + "Sounds");
            Debug.Log("Created missing Sounds folder");
        }
        newfighter = false;
        Screen.orientation = ScreenOrientation.Landscape;
        SceneManager.LoadScene("FighterCreate");
    }
}
