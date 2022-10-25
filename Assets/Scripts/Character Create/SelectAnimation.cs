using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectAnimation : MonoBehaviour
{
    public GameObject NewButton;
    public GameObject button;
    public GameObject canvas;
    public JSONWriter json;
    GameObject latestButton;
    public string[] dir;

    private void Start()
    {
        latestButton = NewButton;

        /*foreach (JSONWriter.AnimSet anim in json)
        {
            GameObject iButton = Instantiate(button, canvas.transform);
            iButton.transform.localPosition = latestButton.transform.localPosition;
            iButton.transform.localPosition = new Vector3(iButton.transform.localPosition.x, iButton.transform.localPosition.y - 120, iButton.transform.localPosition.z);
            latestButton = iButton;
            int index = directory.IndexOf(".");
            iButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = directory.Substring(index + 1);
        }*/
    }
}
