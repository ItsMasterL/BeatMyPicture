using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class DrawingScript : MonoBehaviour
{
    public Camera cam;
    public Camera canvas;
    TrailRenderer trail;
    public RenderTexture rt;
    bool reset; //to reset the canvas wipe
    bool resetLoad; //to reset position of loading image
    public GameObject drawMenu;
    public GameObject cutoutLoader;
    public GameObject imageLoadPlane;
    GameObject imageLoadCanvas;
    public TextMeshProUGUI displayID;

    private void Start()
    {
        trail = GetComponent<TrailRenderer>();
        imageLoadCanvas = imageLoadPlane.transform.parent.gameObject;
        imageLoadCanvas.SetActive(false);
    }

    void Update()
    {
        transform.position = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0) && drawMenu.activeSelf)
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
            drawMenu.SetActive(false);
            cutoutLoader.GetComponent<LoadCutouts>().LoadImages();
        }

        if (reset == true)
        {
            reset = false;
        }

        if (resetLoad == true)
        {
            resetLoad = false;
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
        imageLoadCanvas.SetActive(false);
        reset = true;
        canvas.clearFlags = CameraClearFlags.SolidColor;
    }
    

    public void saveCanvas()
    {
        imageLoadCanvas.SetActive(false);
        string path = OpenTemplate.carryover;
        byte[] bytes = toTexture2D(rt).EncodeToPNG(); //Goes to below method btw
        Directory.CreateDirectory(path + Path.DirectorySeparatorChar + "Cutouts");
        File.WriteAllBytes(path + Path.DirectorySeparatorChar + "Cutouts" + Path.DirectorySeparatorChar + cutoutLoader.GetComponent<LoadCutouts>().ID + ".png", bytes);
        clearCanvas();
        cutoutLoader.GetComponent<LoadCutouts>().LoadImages();
    }

    Texture2D toTexture2D(RenderTexture rTex) //this is for saving in case you skipped over reading the comment in the save method
    {
        Texture2D tex = new Texture2D(300, 300, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        Destroy(tex);
        return tex;
    }

    public void loadCanvas(GameObject loader)
    {
        cutoutLoader.GetComponent<LoadCutouts>().ID = loader.GetComponent<CutoutProperties>().ImageID;
        displayID.text = cutoutLoader.GetComponent<LoadCutouts>().ID;

        Texture2D tex = null;
        byte[] fileData;
        string path = OpenTemplate.carryover;
        
        drawMenu.SetActive(true);
        imageLoadCanvas.SetActive(true);

        if (File.Exists(path + Path.DirectorySeparatorChar + "Cutouts" + Path.DirectorySeparatorChar + cutoutLoader.GetComponent<LoadCutouts>().ID + ".png"))
        {
            fileData = File.ReadAllBytes(path + Path.DirectorySeparatorChar + "Cutouts" + Path.DirectorySeparatorChar + cutoutLoader.GetComponent<LoadCutouts>().ID + ".png");
            tex = new Texture2D(300, 300); //if its not the regular size for whatever reason, it'll resize automatically but this can be bad if theres anti-aliasing or something
            tex.filterMode = FilterMode.Point;
            tex.LoadImage(fileData);
            imageLoadPlane.GetComponent<RawImage>().texture = tex;
            imageLoadCanvas.gameObject.transform.localPosition = new Vector3(imageLoadCanvas.transform.localPosition.x, imageLoadCanvas.transform.localPosition.y, 89);
        } else
        {
            cutoutLoader.GetComponent<LoadCutouts>().LoadImages();
        }
    }

    public void deleteCanvas()
    {
        string path = OpenTemplate.carryover;
        if (File.Exists(path + Path.DirectorySeparatorChar + "Cutouts" + Path.DirectorySeparatorChar + cutoutLoader.GetComponent<LoadCutouts>().ID + ".png"))
        {
            File.Delete(path + Path.DirectorySeparatorChar + "Cutouts" + Path.DirectorySeparatorChar + cutoutLoader.GetComponent<LoadCutouts>().ID + ".png");
            Debug.Log("Deleted " + cutoutLoader.GetComponent<LoadCutouts>().ID + ".png");
        } else
        {
            Debug.Log("Failed");
        }
        clearCanvas();
    }
}
