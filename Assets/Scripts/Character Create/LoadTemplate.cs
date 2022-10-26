using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadTemplate : MonoBehaviour
{
    ReadFiles read;
    public string FilePath;

    private void Start()
    {
        read = GameObject.Find("Template").GetComponent<ReadFiles>();
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

    public void DeleteTemplate()
    {
        Directory.Delete(FilePath, true);
        read.Read();
    }
}
