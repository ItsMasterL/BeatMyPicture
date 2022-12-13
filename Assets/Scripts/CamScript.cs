using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class CamScript : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture Cam;
    private Texture defaultBg;

    public RawImage background;
    public AspectRatioFitter fit;
    public bool switchCam;

    public float offset = 138;
    public float zoom = 0;
    public TextMeshProUGUI display;
    public List<GameObject> ConfirmDisable;
    public List<GameObject> ConfirmEnable;

    public List<GameObject> DisableForPicture;

    private void Start()
    {
        Initialize();
        Debug.Log(Screen.orientation);
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
        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            float ratio = (float)Cam.width / (float)Cam.height;
            fit.aspectRatio = ratio;

            float scaleY = Cam.videoVerticallyMirrored ? -1f : 1f;
            background.rectTransform.localScale = new Vector3(1f * ratio + zoom, scaleY * ratio + zoom, 1f * ratio + zoom);

            int orient = -Cam.videoRotationAngle;
            background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

            background.rectTransform.localPosition = new Vector3(0, offset, 0);
        } else
        {
            float ratio = (float)Cam.width / (float)Cam.height;
            fit.aspectRatio = ratio;

            float scaleX = Cam.videoVerticallyMirrored ? -1f : 1f;
            background.rectTransform.localScale = new Vector3(1f * ratio + zoom, scaleX * ratio + zoom, 1f * ratio + zoom);

            int orient = -Cam.videoRotationAngle;
            background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

            background.rectTransform.localPosition = new Vector3(0, offset, 0);
        }

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

    public void KeepStage(string input)
    {
        foreach (GameObject obj in DisableForPicture)
        {
            obj.SetActive(false);
        }
        ScreenCapture.CaptureScreenshot(OpenStage.carryover + Path.DirectorySeparatorChar + input + ".png");
        foreach (GameObject obj in DisableForPicture)
        {
            obj.SetActive(true);
        }

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

    public void UpdateZoom(Slider input)
    {
        zoom = input.value / 100;
    }
}
