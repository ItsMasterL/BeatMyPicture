using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DrawingScript : MonoBehaviour
{
    public Camera cam;
    public Camera canvas;
    TrailRenderer trail;
    public RenderTexture rt;
    bool reset; //to reset the canvas wipe

    private void Start()
    {
        trail = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        transform.position = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            //trail.enabled = true;
            transform.position = new Vector3(transform.position.x, transform.position.y, 88); //pen down
        } else
        {
            //trail.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, 130); //pen up
        }

        if (reset == false && canvas.clearFlags != CameraClearFlags.Nothing)
        {
            canvas.clearFlags = CameraClearFlags.Nothing;
        }

        if (reset == true)
        {
            reset = false;
        }
    }

    //FF8E00 - orang
    public void Pen(bool drawMode)
    {
        Material mat = GetComponent<Renderer>().material;
        Color orang = new Color(1, 0.5587921f, 0, 1);
        if (drawMode)
        {
            mat.color = orang;
            //trail.startColor = orang;
            //trail.endColor = orang;
        } else
        {
            mat.color = Color.black;
            //trail.startColor = Color.black;
            //trail.endColor = Color.black;
        }
    }

    public void clearCanvas()
    {
        reset = true;
        canvas.clearFlags = CameraClearFlags.SolidColor;
    }

    public void saveCanvas()
    {
        string path = OpenTemplate.carryover;
        byte[] bytes = toTexture2D(rt).EncodeToPNG(); //Goes to below method btw
        Directory.CreateDirectory(path + Path.DirectorySeparatorChar + "Cutouts");
        File.WriteAllBytes(path + Path.DirectorySeparatorChar + "Cutouts" + Path.DirectorySeparatorChar + "id.png", bytes);
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(300, 300, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        Destroy(tex);
        return tex;
    }
}
