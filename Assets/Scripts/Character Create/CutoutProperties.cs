using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutoutProperties : MonoBehaviour
{
    public string ImageID;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(delegate { GameObject.Find("Draw").GetComponent<DrawingScript>().loadCanvas(gameObject); });
    }
}
