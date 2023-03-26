using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongTypeDisplay : MonoBehaviour
{
    TextMeshProUGUI display;

    private void Start()
    {
        display = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateDisplay(string song) //returns true if custom song
    {
        if (song == "day" || song == "dusk" || song == "night")
        {
            this.display.text = "Using a custom song";
        }
        else
        {
            this.display.text = "Using BMP OST";
        }
    }
}
