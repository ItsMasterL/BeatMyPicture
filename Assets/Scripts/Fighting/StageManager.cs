using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class StageManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        byte[] fileData = File.ReadAllBytes(CharManager.stage + Path.DirectorySeparatorChar + "day.png");
        Texture2D tex = new Texture2D(2, 2);
        tex.filterMode = FilterMode.Point;
        tex.LoadImage(fileData);
        GetComponent<RawImage>().texture = tex;
    }
}
