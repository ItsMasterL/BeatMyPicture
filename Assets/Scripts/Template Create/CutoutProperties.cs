using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CutoutProperties : MonoBehaviour
{
    public string ImageID;
    public float xPos;

    private void Start()
    {
        xPos = transform.localPosition.x;
    }

    public void TapOnButton()
    {
        DrawingScript loader = GameObject.Find("Draw").GetComponent<DrawingScript>();
        loader.loadCanvas(gameObject);
    }

    public void TapOnButton2()
    {
        JSONManager manager = GameObject.Find("JSON Manager").GetComponent<JSONManager>();
        
        string[] description = manager.desc.PoseDescriptions.ToArray();
        float[] limit = manager.desc.SoundLimits.ToArray();
        manager.SelectedPose = ImageID;
        GameObject.Find("CutoutDescriptionInput").GetComponent<TMPro.TMP_InputField>().text = description[int.Parse(ImageID)];
        GameObject.Find("CutoutDescriptionInput").GetComponent<TMPro.TMP_InputField>().interactable = true;
    }
}
