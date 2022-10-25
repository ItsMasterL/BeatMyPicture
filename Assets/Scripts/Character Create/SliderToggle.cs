using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderToggle : MonoBehaviour
{
    public GameObject slider;
    public void ToggleActive()
    {
        if (slider.activeSelf)
            slider.SetActive(false);
        else
            slider.SetActive(true);
    }
}
