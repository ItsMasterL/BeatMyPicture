using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetProperties : MonoBehaviour
{
    public string SetID;
    public float xPos;

    private void Start()
    {
        xPos = transform.localPosition.x;
    }

    public void TapOnButton()
    {
        JSONManager manager = GameObject.Find("JSON Manager").GetComponent<JSONManager>();
        manager.loadSetJSON(gameObject);
    }
}
