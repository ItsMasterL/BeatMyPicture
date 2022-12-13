using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class ReadStages : MonoBehaviour
{
    public GameObject NewButton;
    public GameObject button;
    public GameObject canvas;
    GameObject latestButton;
    public string[] dir;
    public GameObject scrollbar;

    public bool delete;

    private void Start()
    {
        Read();
        scrollbar.GetComponent<TemplateSelectScroll>().Refresh();
    }

    public void Read()
    {
        latestButton = NewButton;

        foreach (GameObject i in GameObject.FindGameObjectsWithTag("FileRead"))
        {
            Destroy(i);
        }

        dir = Directory.GetDirectories(Application.persistentDataPath + Path.DirectorySeparatorChar + "Stages" + Path.DirectorySeparatorChar);

        foreach (string directory in dir)
        {
            GameObject iButton = Instantiate(button, canvas.transform);
            iButton.transform.localPosition = latestButton.transform.localPosition;
            iButton.transform.localPosition = new Vector3(iButton.transform.localPosition.x, iButton.transform.localPosition.y - 120, iButton.transform.localPosition.z);
            latestButton = iButton;
            iButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Path.GetFileName(directory);
            iButton.GetComponent<LoadStage>().FilePath = directory;
        }
    }

    public void ToggleDeleteMode()
    {
        delete = !delete;
    }
}
