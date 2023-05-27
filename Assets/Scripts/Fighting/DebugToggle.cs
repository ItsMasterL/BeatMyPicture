using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugToggle : MonoBehaviour
{
    public void SetDebug()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            obj.GetComponent<PlayerManager>().debugMode = GetComponent<Toggle>().isOn;
        }
    }
}
