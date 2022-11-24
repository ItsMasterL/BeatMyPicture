using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FrameInputProperties : MonoBehaviour
{
    public int index;
    public float yPos;

    private void Start()
    {
        yPos = transform.localPosition.y;
    }

    public void CheckInput()
    {
        if (GetComponent<TMP_InputField>().text == "-")
        {
            GameObject.Find("JSON Manager").GetComponent<JSONManager>().Set.frameIDs[index] = null;
            Destroy(gameObject);
        } else {
            GameObject.Find("JSON Manager").GetComponent<JSONManager>().Set.frameIDs[index] = GetComponent<TMP_InputField>().text;
        }
    }
}
