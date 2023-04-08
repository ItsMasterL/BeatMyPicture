using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetRecInfo : MonoBehaviour
{
    public LoadInfoFromTemplates info;
    public RecordAudio rec;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI length;
    public TextMeshProUGUI count;

    private void OnEnable()
    {
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        count.text = "Sound " + rec.currentSound;
        desc.text = "(No description given... That makes it sort of tough, doesnt it?)";
        length.text = "Sound Length:\n\n0.0 seconds";
        desc.text = info.set.SoundDescriptions[rec.currentSound];
        length.text = "Sound Length:\n\n" + info.set.SoundLimits[rec.currentSound].ToString("0.0") + " seconds";
    }
}
