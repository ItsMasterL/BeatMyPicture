using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadStage : MonoBehaviour
{
    ReadStages read;
    public string FilePath;
    public float yPos;
    public float xPos;

    private void Start()
    {
        yPos = transform.localPosition.y;
        xPos = transform.localPosition.x;
        read = GameObject.Find("Stage").GetComponent<ReadStages>();
    }

    public void ClickLogic()
    {
        if (read.delete)
        {
            DeleteStage();
        }
        else
        {
            GetComponent<OpenStage>().Open();
        }
    }

    public void DeleteStage()
    {
        Directory.Delete(FilePath, true);
        read.Read();
    }
}
