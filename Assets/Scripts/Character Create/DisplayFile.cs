using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayFile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = OpenTemplate.carryover;
    }
}
