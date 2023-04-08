using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class NameCheck : MonoBehaviour
{
    TextMeshProUGUI text;
    public bool okay;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void checkTemplates(GameObject obj)
    {
        string input = obj.GetComponent<TMP_InputField>().text;
        string[] dir = Directory.GetDirectories(Application.persistentDataPath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar);

        okay = true;
        text.text = "";

        if (input == "")
        {
            okay = false;
            text.text = "Name cannot be blank!";
            return;
        }
        foreach (string directory in dir)
        {
            if (directory.ToLower() == (Application.persistentDataPath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + input).ToLower())
            {
                okay = false;
                text.text = "Name already in use!";
            }
        }
    }
    public void checkStages(GameObject obj)
    {
        string input = obj.GetComponent<TMP_InputField>().text;
        string[] dir = Directory.GetDirectories(Application.persistentDataPath + Path.DirectorySeparatorChar + "Stages" + Path.DirectorySeparatorChar);

        okay = true;
        text.text = "";

        if (input == "")
        {
            okay = false;
            text.text = "Name cannot be blank!";
            return;
        }
        foreach (string directory in dir)
        {
            if (directory.ToLower() == (Application.persistentDataPath + Path.DirectorySeparatorChar + "Stages" + Path.DirectorySeparatorChar + input).ToLower())
            {
                okay = false;
                text.text = "Name already in use!";
            }
        }
    }
    
    public void checkFighters(GameObject obj)
    {
        string input = obj.GetComponent<TMP_InputField>().text;
        string[] dir = Directory.GetDirectories(Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters" + Path.DirectorySeparatorChar);

        okay = true;
        text.text = "";

        if (input == "")
        {
            okay = false;
            text.text = "Name cannot be blank!";
            return;
        }
        if (OpenFighter.templateselected == "" || OpenFighter.templateselected == null)
        {
            okay = false;
            text.text = "You must select a template!";
            return;
        }
        foreach (string directory in dir)
        {
            if (directory.ToLower() == (Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters" + Path.DirectorySeparatorChar + input).ToLower())
            {
                okay = false;
                text.text = "Name already in use!";
            }
        }
    }
}
