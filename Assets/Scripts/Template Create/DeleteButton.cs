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
            GetComponent<Image>().color = Color.red;
        } else
        {
            GetComponent<Image>().color = Color.white;
        }
    }
    public void DeleteStage()
    {
        if (GameObject.Find("Stage").GetComponent<ReadStages>().delete)
        {
            GetComponent<Image>().color = Color.red;
        } else
        {
            GetComponent<Image>().color = Color.white;
        }
    }
}
