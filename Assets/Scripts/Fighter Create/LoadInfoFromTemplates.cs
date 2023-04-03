using System.Collections;
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
    #endregion


    private void Awake()
    {
        if (File.Exists(OpenFighter.templateselected + Path.DirectorySeparatorChar + "properties.json"))
        {
            string json = File.ReadAllText(OpenFighter.templateselected + Path.DirectorySeparatorChar + "properties.json");
            JSONManager.AudioAndDescriptions set = JsonUtility.FromJson<JSONManager.AudioAndDescriptions>(json);

            poseCount.text = "This template has " + Directory.GetFiles(OpenFighter.templateselected + Path.DirectorySeparatorChar + "Cutouts" + Path.DirectorySeparatorChar).Length
                + " poses for you to take pictures of and " + set.SoundIDs.Count.ToString() + " sounds for you to record.";
        }
    }

    public void LoadPreview(int index)
    {
        poseIDDisplay.text = "POSE " + index.ToString("000");
        descDisplay.text = "(No description given... use your imaginationnnnn?)";
        if (File.Exists(OpenFighter.templateselected + Path.DirectorySeparatorChar + "properties.json"))
        {
            string json = File.ReadAllText(OpenFighter.templateselected + Path.DirectorySeparatorChar + "properties.json");
            JSONManager.AudioAndDescriptions set = JsonUtility.FromJson<JSONManager.AudioAndDescriptions>(json);

            descDisplay.text = set.PoseDescriptions[index];
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
}
