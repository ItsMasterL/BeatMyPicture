using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CopySliderValues : MonoBehaviour
{
    public Slider ParentSlider;
    public TMP_InputField valueInput;
    // Update is called once per frame
    void Update()
    {
        GetComponent<Slider>().maxValue = ParentSlider.maxValue;
        if (valueInput.text != "")
        GetComponent<Slider>().value = float.Parse(valueInput.text);
    }
}
