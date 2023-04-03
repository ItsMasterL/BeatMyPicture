using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RIF : MonoBehaviour
{
    public void UpdateSize()
    {
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
