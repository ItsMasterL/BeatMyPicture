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
    public static bool newfighter;

    public void Create(GameObject obj)
    {
        string input = obj.GetComponent<TMP_InputField>().text;
        if (check.GetComponent<NameCheck>().okay == true)
        {
            Directory.CreateDirectory(Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters" + Path.DirectorySeparatorChar + input);
            Directory.CreateDirectory(Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters" + Path.DirectorySeparatorChar + input + Path.DirectorySeparatorChar + "Pictures");
            Directory.CreateDirectory(Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters" + Path.DirectorySeparatorChar + input + Path.DirectorySeparatorChar + "Sounds");
            string jsoninfo = File.ReadAllText(templateselected + Path.DirectorySeparatorChar + "properties.json");
            JSONManager.AudioAndDescriptions info = JsonUtility.FromJson<JSONManager.AudioAndDescriptions>(jsoninfo);
            string UUID = info.UUID;
            carryover = Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters" + Path.DirectorySeparatorChar + input;
            File.WriteAllText(carryover + Path.DirectorySeparatorChar + "template.noedit", UUID);
            shortcarryover = "Fighters" + Path.DirectorySeparatorChar + input;
            newfighter = true;
            Screen.orientation = ScreenOrientation.Portrait;
            Debug.Log("Template is " + templateselected);
            SceneManager.LoadScene("FighterCreate");
        }
    }

    public void Open()
    {
        carryover = gameObject.GetComponent<LoadFighter>().FilePath;
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
        string[] dir = Directory.GetDirectories(Application.persistentDataPath + Path.DirectorySeparatorChar + "Templates");
        string key = File.ReadAllText(carryover + Path.DirectorySeparatorChar + "template.noedit");
        for (int i = 0; i < dir.Length; i++)
        {
            string check = File.ReadAllText(dir[i] + Path.DirectorySeparatorChar + "properties.json");
            JSONManager.AudioAndDescriptions info = JsonUtility.FromJson<JSONManager.AudioAndDescriptions>(check);
            Debug.Log(info.UUID + " vs " + key);
            if (info.UUID == key)
            {
                templateselected = dir[i];
                i = dir.Length + 30;
            }
        }
        if (templateselected == null)
        {
            Debug.LogError("There was no matching UUID");
            return;
        }
        Debug.Log(templateselected);
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("FighterCreate");
    }
}
