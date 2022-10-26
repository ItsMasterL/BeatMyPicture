using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteButton : MonoBehaviour
{
    public void DeletePress()
    {
        if (GameObject.Find("Template").GetComponent<ReadFiles>().delete)
        {
            GetComponent<Image>().color = Color.red;
        } else
        {
            GetComponent<Image>().color = Color.white;
        }
    }
}
