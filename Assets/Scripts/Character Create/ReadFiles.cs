using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class ReadFiles : MonoBehaviour
{
    public GameObject NewButton;
    public GameObject button;
    public GameObject canvas;
    GameObject latestButton;
    public string[] dir;

    private void Start()
    {
        latestButton = NewButton;
        dir = Directory.GetDirectories(Application.persistentDataPath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar);

        foreach (string directory in dir)
        {
            GameObject iButton = Instantiate(button, canvas.transform);
            iButton.transform.localPosition = latestButton.transform.localPosition;
            iButton.transform.localPosition = new Vector3 (iButton.transform.localPosition.x, iButton.transform.localPosition.y - 120, iButton.transform.localPosition.z);
            latestButton = iButton;
            int index = directory.IndexOf(".");
            iButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = directory.Substring(index + 1);
        }
    }
}
