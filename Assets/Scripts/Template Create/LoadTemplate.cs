using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadTemplate : MonoBehaviour
{
    ReadTemplates read;
    public string FilePath;
    public float yPos;

    private void Start()
    {
        yPos = transform.localPosition.y;
        read = GameObject.Find("Template").GetComponent<ReadTemplates>();
        OpenFighter.templateselected = "";
    }

    public void ClickLogic()
    {
        if (read.delete)
        {
            DeleteTemplate();
        } else
        {
            GetComponent<OpenTemplate>().Open();
        }
    }

    public void SelectTemplateForFighter()
    {
        OpenFighter.templateselected = FilePath;
    }

    public void DeleteTemplate()
    {
        Directory.Delete(FilePath, true);
        read.Read();
    }
}
