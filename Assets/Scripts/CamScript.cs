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

    public GameObject prepScreen;
    public GameObject picScreen;
    public GameObject colorFilter;

    private void Start()
    {
        Initialize();
        Debug.Log(Screen.orientation);
    }

    public void Initialize()
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

    public void KeepFighter(GameObject transform)
    {
        foreach (GameObject obj in DisableForPicture)
        {
            obj.SetActive(false);
        }

        StartCoroutine(ProcessAndSaveFighterImage(transform));
    }

    private IEnumerator ProcessAndSaveFighterImage(GameObject transform)
    {
        yield return new WaitForEndOfFrame();

        Vector3[] points = new Vector3[4];
        Vector3[] screenspace = new Vector3[4];

        transform.GetComponent<RectTransform>().GetWorldCorners(points);

        int i = 0;

        foreach (Vector3 point in points)
        {
            screenspace[i] = Camera.main.WorldToScreenPoint(point);
            i++;
        }
        //point 2 is top right, point 0 is bottom left

        Texture2D tex = new Texture2D((int)screenspace[2].x - (int)screenspace[0].x, (int)screenspace[2].y - (int)screenspace[0].y, TextureFormat.RGB24, false);
        Rect rect = new Rect(screenspace[0], screenspace[2] - screenspace[0]);

        tex.ReadPixels(rect, 0, 0);
        tex.Apply();
        //FFBC5D - colortrigger
        Color colortrigger = colorFilter.GetComponent<Image>().color; // color triggers to change
        Color colorset = new Color(0, 0, 0, 0); //Empty!
        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                if (tex.GetPixel(x, y) == colortrigger)
                {
                    // Change the pixel to transparent
                    tex.SetPixel(x, y, colorset);
                }
            }
        }
        tex.Apply();
        byte[] texture = tex.EncodeToPNG();
        Destroy(tex);

        File.WriteAllBytes(OpenFighter.carryover + Path.DirectorySeparatorChar + "Pictures" + Path.DirectorySeparatorChar +
            GameObject.Find("Main Camera").GetComponent<LoadInfoFromTemplates>().frame.ToString("000") + ".png", texture);

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

        //go back to loading stuff
        prepScreen.SetActive(true);
        picScreen.SetActive(false);
        gameObject.GetComponent<LoadInfoFromTemplates>().UpFrameandLoad();
    }

    public void KeepStage(string input)
    {
        foreach (GameObject obj in DisableForPicture)
        {
            obj.SetActive(false);
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
            ScreenCapture.CaptureScreenshot(OpenStage.carryover + Path.DirectorySeparatorChar + input + ".png");
        else
            ScreenCapture.CaptureScreenshot(OpenStage.shortcarryover + Path.DirectorySeparatorChar + input + ".png");
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
