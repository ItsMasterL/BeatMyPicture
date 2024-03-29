﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadFighter : MonoBehaviour
{
    ReadFighters read;
    public string FilePath;
    public float yPos;
    public float xPos;

    private void Start()
    {
        yPos = transform.localPosition.y;
        xPos = transform.localPosition.x;
        read = GameObject.Find("Fighter").GetComponent<ReadFighters>();
    }

    public void ClickLogic()
    {
        if (read.delete)
        {
            DeleteFighter();
        }
        else
        {
            GetComponent<OpenFighter>().Open();
        }
    }

    public void DeleteFighter()
    {
        Directory.Delete(FilePath, true);
        read.Read();
    }
}
