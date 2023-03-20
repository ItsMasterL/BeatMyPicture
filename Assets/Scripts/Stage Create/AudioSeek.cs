using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSeek : MonoBehaviour
{
    public Slider slider;
    public GameObject source;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    public void SetMaxLength(float endValue)
    {
        slider.value = 0;
        slider.maxValue = endValue;
        if (endValue == 0)
        {
            slider.maxValue = 1;
        }
    }

    private void Update()
    {
        if (source.GetComponent<AudioSource>().isPlaying)
        {
            slider.value = source.GetComponent<AudioSource>().time;
        }
    }
}
