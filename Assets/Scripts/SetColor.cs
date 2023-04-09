using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetColor : MonoBehaviour
{
    public bool isCamera;
    public bool main;
    public bool Lmain;
    public bool Dmain;
    public bool secondary;
    public bool Lsecondary;
    public bool Dsecondary;

    private void OnEnable()
    {
        UpdateColor();
    }

    public void UpdateColor()
    {
        if (GameObject.Find("ColorPalette") == null)
        {
            return;
        }
        if (main)
        {
            SetColorMain();
        } else if (Lmain)
        {
            SetColorMainLight();
        } else if (Dmain)
        {
            SetColorMainDark();
        } else if (secondary)
        {
            SetColorSecondary();
        } else if (Lsecondary)
        {
            SetColorSecondaryLight();
        } else
        {
            SetColorSecondaryDark();
        }
    }

    private void SetColorMain()
    {
        if (isCamera)
        {
            Color col = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].main;
            GetComponent<Camera>().backgroundColor = new Color(col.r,col.g,col.b, GetComponent<Camera>().backgroundColor.a);
            
        } else
        {
            Color col = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].main;
            GetComponent<Image>().color = new Color(col.r, col.g, col.b, GetComponent<Image>().color.a);
        }
    }
    private void SetColorMainLight()
    {
        if (isCamera)
        {
            Color col = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].lightmain;
            GetComponent<Camera>().backgroundColor = new Color(col.r, col.g, col.b, GetComponent<Camera>().backgroundColor.a);

        }
        else
        {
            Color col = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].lightmain;
            GetComponent<Image>().color = new Color(col.r, col.g, col.b, GetComponent<Image>().color.a);
        }
    }
    private void SetColorMainDark()
    {
        if (isCamera)
        {
            Color col = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].darkmain;
            GetComponent<Camera>().backgroundColor = new Color(col.r, col.g, col.b, GetComponent<Camera>().backgroundColor.a);

        } else
        {
            Color col = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].darkmain;
            GetComponent<Image>().color = new Color(col.r, col.g, col.b, GetComponent<Image>().color.a);
        }
    }
    private void SetColorSecondary()
    {
        if (isCamera)
        {
            Color col = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].secondary;
            GetComponent<Camera>().backgroundColor = new Color(col.r, col.g, col.b, GetComponent<Camera>().backgroundColor.a);

        }
        else
        {
            Color col = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].secondary;
            GetComponent<Image>().color = new Color(col.r, col.g, col.b, GetComponent<Image>().color.a);
        }
    }
    private void SetColorSecondaryLight()
    {
        if (isCamera)
        {
            Color col = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].lightsecondary;
            GetComponent<Camera>().backgroundColor = new Color(col.r, col.g, col.b, GetComponent<Camera>().backgroundColor.a);

        }
        else
        {
            Color col = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].lightsecondary;
            GetComponent<Image>().color = new Color(col.r, col.g, col.b, GetComponent<Image>().color.a);
        }
    }
    private void SetColorSecondaryDark()
    {
        if (isCamera)
        {
            Color col = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].darksecondary;
            GetComponent<Camera>().backgroundColor = new Color(col.r, col.g, col.b, GetComponent<Camera>().backgroundColor.a);

        } else
        {
            Color col = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].darksecondary;
            GetComponent<Image>().color = new Color(col.r, col.g, col.b, GetComponent<Image>().color.a);
        }
    }
}
