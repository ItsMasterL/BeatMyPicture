﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class LoadInfoFromTemplates : MonoBehaviour
{
    #region preview items
    public TextMeshProUGUI poseIDDisplay;
    public TextMeshProUGUI descDisplay;
    public RawImage poseDisplay;
    public RawImage cutout;
    #endregion

    #region info screen items
    [Space(15)]
    public TextMeshProUGUI poseCount;
    public TextMeshProUGUI pictureDesc;
    #endregion

    public int frame;
    public int limit;

    public GameObject soundPage;
    public GameObject endPage;
    public GameObject prepPage;
    public GameObject startPage;
    public GameObject editPage;
    public GameObject lagpage;

    public JSONManager.AudioAndDescriptions set;

    private void Awake()
    {
        if (OpenFighter.newfighter)
        {
            limit = Directory.GetFiles(OpenFighter.templateselected + Path.DirectorySeparatorChar + "Cutouts" + Path.DirectorySeparatorChar).Length;
            if (File.Exists(OpenFighter.templateselected + Path.DirectorySeparatorChar + "properties.json"))
            {
                string json = File.ReadAllText(OpenFighter.templateselected + Path.DirectorySeparatorChar + "properties.json");
                set = JsonUtility.FromJson<JSONManager.AudioAndDescriptions>(json);

                poseCount.text = "This template has " + limit + " poses for you to take pictures of and " + set.SoundIDs.Count.ToString() + " sounds for you to record.";
            }
            frame = 0;
        } else
        {
            startPage.SetActive(false);
            editPage.SetActive(true);
        }
    }

    public void UpFrameandLoad()
    {
        if (frame < limit - 1)
        {
            frame++;
            LoadPreview(frame);
        }
        else if (set.SoundIDs.Count > 0)
        {
            soundPage.SetActive(true);
            prepPage.SetActive(false);
        } else
        {
            lagpage.SetActive(true);
            endPage.SetActive(true);
            prepPage.SetActive(false);
        }
    }

    public void LoadPreview(int index)
    {
        poseIDDisplay.text = "POSE " + index.ToString("000");
        descDisplay.text = "(No description given... use your imaginationnnnn?)";
        pictureDesc.text = "[Missing Description]";
        if (File.Exists(OpenFighter.templateselected + Path.DirectorySeparatorChar + "properties.json"))
        {
            string json = File.ReadAllText(OpenFighter.templateselected + Path.DirectorySeparatorChar + "properties.json");
            JSONManager.AudioAndDescriptions set = JsonUtility.FromJson<JSONManager.AudioAndDescriptions>(json);
            if (index <= set.PoseDescriptions.Count - 1)
            {
                descDisplay.text = set.PoseDescriptions[index];
                pictureDesc.text = descDisplay.text;
            }
        }

        //Loading image - FIGURE OUT COUROTINES GOSH DARN IT
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(OpenFighter.templateselected + Path.DirectorySeparatorChar + "Cutouts" + Path.DirectorySeparatorChar + index.ToString("000") + ".png"))
        {
            fileData = File.ReadAllBytes(OpenFighter.templateselected + Path.DirectorySeparatorChar + "Cutouts" + Path.DirectorySeparatorChar + index.ToString("000") + ".png");
            tex = new Texture2D(300, 300); //if its not the regular size for whatever reason, it'll resize automatically but this can be bad if theres anti-aliasing or something
            tex.filterMode = FilterMode.Point;
            tex.LoadImage(fileData);
            //FF8E00 - colortrigger
            Color colortrigger = new Color(1, 0.5568628f, 0, 1); // color triggers to change
            Color colorset = new Color(0, 0, 0, 0); //Empty!
            for (int y = 0; y < tex.height; y++)
            {
                for (int x = 0; x < tex.width; x++)
                {
                    if (tex.GetPixel(x, y) != colortrigger)
                    {
                        // Change the pixel to transparent
                        tex.SetPixel(x, y, colorset);
                    }
                }
            }
            tex.Apply();
            poseDisplay.texture = tex;
            cutout.texture = tex;
            cutout.gameObject.GetComponent<RIF>().UpdateSize();
        }
    }
    private IEnumerator LoadImage(int index)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(OpenFighter.templateselected + "Cutouts" + Path.DirectorySeparatorChar + index.ToString("000") + ".png"))
        {
            fileData = File.ReadAllBytes(OpenFighter.templateselected + Path.DirectorySeparatorChar + "Cutouts" + Path.DirectorySeparatorChar + index.ToString("000") + ".png");
            tex = new Texture2D(300, 300); //if its not the regular size for whatever reason, it'll resize automatically but this can be bad if theres anti-aliasing or something
            tex.filterMode = FilterMode.Point;
            tex.LoadImage(fileData);
            //FF8E00 - colortrigger
            Color colortrigger = new Color(1, 0.5587921f, 0, 1); // color triggers to change
            Color colorset = new Color(0, 0, 0, 0); //Empty!
            for (int y = 0; y < tex.height; y++)
            {
                for (int x = 0; x < tex.width; x++)
                {
                    if (tex.GetPixel(x, y) != colortrigger)
                    {
                        // Change the pixel to transparent
                        tex.SetPixel(x, y, colorset);
                    }
                }
            }
            tex.Apply();
            yield return tex;
        }
        else
            yield return null;
    }

    public static void RemoveBackground(GameObject lagpage) //Make this ienumerator/courotine sometime!!
    {
        Texture2D tex = new Texture2D(2, 2);
        Color colortrigger = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].main; // color triggers to change
        Color colorset = new Color(0, 0, 0, 0); //Empty!
        Debug.Log(Directory.GetFiles(OpenFighter.carryover + Path.DirectorySeparatorChar + "Pictures").Length);
        string key = ColorUtility.ToHtmlStringRGB(colortrigger);
        Color triggercolor = Color.black;
        foreach (string file in Directory.GetFiles(OpenFighter.carryover + Path.DirectorySeparatorChar + "Pictures"))
        {
            byte[] fileData = File.ReadAllBytes(file);
            tex = new Texture2D(300, 300);
            tex.filterMode = FilterMode.Point;
            tex.LoadImage(fileData);
            for (int y = 0; y < tex.height; y++)
            {
                for (int x = 0; x < tex.width; x++)
                {
                    if (triggercolor != Color.black && tex.GetPixel(x, y) == triggercolor)
                    {
                        // Change the pixel to transparent
                        tex.SetPixel(x, y, colorset);
                    }
                    else if (ColorUtility.ToHtmlStringRGB(tex.GetPixel(x, y)) == key)
                    {
                        //save pixel info to optimise
                        triggercolor = tex.GetPixel(x,y);
                        // Change the pixel to transparent
                        tex.SetPixel(x, y, colorset);
                    }
                }
            }
            tex.Apply();
            byte[] texture = tex.EncodeToPNG();
            File.WriteAllBytes(file, texture);
        }
        lagpage.SetActive(false);
    }
}
