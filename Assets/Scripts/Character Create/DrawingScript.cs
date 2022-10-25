using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingScript : MonoBehaviour
{
    public Camera cam;
    public Camera canvas;
    TrailRenderer trail;
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
}
