using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteButton : MonoBehaviour
{
    public void DeleteTemplate()
    {
        if (GameObject.Find("Template").GetComponent<ReadTemplates>().delete)
        {
            GetComponent<Image>().color = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].darksecondary;
        } else
        {
            GetComponent<Image>().color = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].lightmain;
        }
    }
    public void DeleteStage()
    {
        if (GameObject.Find("Stage").GetComponent<ReadStages>().delete)
        {
            GetComponent<Image>().color = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].darksecondary;
        } else
        {
            GetComponent<Image>().color = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].lightmain;
        }
    }
    public void DeleteFighter()
    {
        if (GameObject.Find("Fighter").GetComponent<ReadFighters>().delete)
        {
            GetComponent<Image>().color = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].darksecondary;
        } else
        {
            GetComponent<Image>().color = GameObject.Find("ColorPalette").GetComponent<ColorPalettes>().colorset[SetupFiles.profile.ColorID].lightmain;
        }
    }
}
