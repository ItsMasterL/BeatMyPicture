using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutoutProperties : MonoBehaviour
{
    public string ImageID;
    public float xPos;

    private void Start()
    {
        xPos = transform.localPosition.x;
    }

    public void TapOnButton()
    {
        DrawingScript loader = GameObject.Find("Draw").GetComponent<DrawingScript>();
        loader.loadCanvas(gameObject);
    }
}
