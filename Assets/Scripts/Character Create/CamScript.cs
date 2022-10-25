﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CamScript : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture Cam;
    private Texture defaultBg;

    public RawImage background;
    public AspectRatioFitter fit;
    public bool switchCam;

    public float offset = 138;
    public TextMeshProUGUI display;
    public List<GameObject> ConfirmDisable;
    public List<GameObject> ConfirmEnable;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        defaultBg = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera!!");
            camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if(!devices[i].isFrontFacing && !switchCam)
            {
                Cam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
            if(devices[i].isFrontFacing && switchCam)
            {
                Cam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (Cam == null && !switchCam)
        {
            Debug.Log("Back cam is selected and not detected");
            return;
        }

        if (Cam == null && switchCam)
        {
            Debug.Log("Front cam is selected and not detected");
            return;
        }
        
            Cam.Play();
            background.texture = Cam;

        camAvailable = true;
    }

    private void Update()
    {
        if (!camAvailable)
            return;

        float ratio = (float)Cam.width / (float)Cam.height;
        fit.aspectRatio = ratio;

        float scaleY = Cam.videoVerticallyMirrored ? -1f: 1f;
        background.rectTransform.localScale = new Vector3(1f * ratio, scaleY * ratio, 1f * ratio);

        int orient = -Cam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

        background.rectTransform.localPosition = new Vector3(0, offset, 0);

    }

    public void SwitchCamera()
    {
            switchCam = !switchCam;
            if (Cam != null)
            Cam.Stop();
            Initialize();
    }

    public void TakePic()
    {
        Cam.Pause();
        foreach(GameObject obj in ConfirmDisable)
        {
            obj.SetActive(false);
        }
        foreach(GameObject obj in ConfirmEnable)
        {
            obj.SetActive(true);
        }
    }

    public void Retry()
    {
        foreach (GameObject obj in ConfirmDisable)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in ConfirmEnable)
        {
            obj.SetActive(false);
        }
        Cam.Play();
    }

    public void Keep()
    {
        foreach (GameObject obj in ConfirmDisable)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in ConfirmEnable)
        {
            obj.SetActive(false);
        }
    }

    public void UpdateOffset(Slider input)
    {
        offset = input.value;
    }
}
