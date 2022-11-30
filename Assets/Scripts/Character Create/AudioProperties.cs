using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AudioProperties : MonoBehaviour
{
    public string AudioID;
    public float xPos;

    private void Start()
    {
        xPos = transform.localPosition.x;
    }

    public void TapOnButton()
    {
        JSONManager manager = GameObject.Find("JSON Manager").GetComponent<JSONManager>();
        if (gameObject.tag == "AudioImports")
        {
            manager.SelectedImport = AudioID;
            GameObject.Find("PlaySound").GetComponent<AudioLoader>().GetSpecificAudioFromFolder(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Sound", AudioID);
        }
        else
        {
            string[] description = manager.desc.SoundDescriptions.ToArray();
            float[] limit = manager.desc.SoundLimits.ToArray();
            manager.SelectedRecording = AudioID;
            GameObject.Find("DescriptionInput").GetComponent<TMPro.TMP_InputField>().text = description[int.Parse(AudioID)];
            GameObject.Find("DescriptionInput").GetComponent<TMPro.TMP_InputField>().interactable = true;
            GameObject.Find("LimitSlider").GetComponent<UnityEngine.UI.Slider>().value = limit[int.Parse(AudioID)];
            GameObject.Find("LimitSlider").GetComponent<UnityEngine.UI.Slider>().interactable = true;
        }
    }
}
