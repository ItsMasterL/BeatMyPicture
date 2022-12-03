using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadStage : MonoBehaviour
{
    ReadStages read;
    public string FilePath;
    public float yPos;

    private void Start()
    {
        yPos = transform.localPosition.y;
        read = GameObject.Find("Stage").GetComponent<ReadStages>();
    }

    public void ClickLogic()
    {
        if (read.delete)
        {
            DeleteTemplate();
        }
        else
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
