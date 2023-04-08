using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RIF : MonoBehaviour
{
    public void UpdateSize()
    {
        GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
        GetComponent<RectTransform>().offsetMax = new Vector2(0,0);

        GetComponent<RawImage>().SizeToParent();
    }

    private void OnEnable()
    {
        UpdateSize();
    }

    private void Awake()
    {
        UpdateSize();
    }
}
