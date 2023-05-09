using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SelectPreview : MonoBehaviour
{
    public bool fighterNotStage;
    string path;
    RawImage ri;
    // Start is called before the first frame update
    void Start()
    {
        ri = GetComponentInChildren<RawImage>();

        if (fighterNotStage)
        {
            int imagetoload;
            string match = "";
            JSONManager.AudioAndDescriptions desc = new JSONManager.AudioAndDescriptions();
            path = GetComponent<LoadFighter>().FilePath;
            string template = File.ReadAllText(path + Path.DirectorySeparatorChar + "template.noedit");
            foreach (string dir in Directory.GetDirectories(Application.persistentDataPath + Path.DirectorySeparatorChar + "Templates"))
            {
                if (match == "")
                {
                    match = File.ReadAllText(dir + Path.DirectorySeparatorChar + "properties.json");
                    desc = JsonUtility.FromJson<JSONManager.AudioAndDescriptions>(match);
                    if (desc.UUID == template)
                    {
                        match = dir;
                    }
                    else
                    {
                        match = "";
                    }
                }
            }
            if (match != "")
            {
                imagetoload = int.Parse(desc.SelectPose);

                if (File.Exists(path + Path.DirectorySeparatorChar + "Pictures" + Path.DirectorySeparatorChar + imagetoload.ToString("000") + ".png"))
                {
                    byte[] fileData = File.ReadAllBytes(path + Path.DirectorySeparatorChar + "Pictures" + Path.DirectorySeparatorChar + imagetoload.ToString("000") + ".png");
                    Texture2D tex = new Texture2D(300, 300);
                    tex.filterMode = FilterMode.Point;
                    tex.LoadImage(fileData);
                    ri.texture = tex;
                }
            }
        }
        else
        {
            path = GetComponent<LoadStage>().FilePath;
            if (File.Exists(path + Path.DirectorySeparatorChar + "day.png")) //TODO: make time of day dependant
            {
                byte[] fileData = File.ReadAllBytes(path + Path.DirectorySeparatorChar + "day.png");
                Texture2D tex = new Texture2D(300, 300);
                tex.filterMode = FilterMode.Point;
                tex.LoadImage(fileData);
                ri.texture = tex;
            }
            
        }
    }
}
