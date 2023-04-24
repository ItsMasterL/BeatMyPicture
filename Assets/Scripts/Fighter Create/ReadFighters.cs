using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class ReadFighters : MonoBehaviour
{
    public GameObject NewButton;
    public GameObject button;
    public GameObject canvas;
    GameObject latestButton;
    public string[] dir;
    public GameObject scrollbar;

    public bool delete;

    public bool charSelect;

    private void Start()
    {
        if (charSelect)
        {
            ReadCharSel();
        }
        else
        {
            Read();
        }
        scrollbar.GetComponent<TemplateSelectScroll>().Refresh();
    }

    public void Read()
    {
        latestButton = NewButton;

        foreach (GameObject i in GameObject.FindGameObjectsWithTag("FileRead"))
        {
            Destroy(i);
        }

        dir = Directory.GetDirectories(Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters" + Path.DirectorySeparatorChar);

        foreach (string directory in dir)
        {
            GameObject iButton = Instantiate(button, canvas.transform);
            iButton.transform.localPosition = latestButton.transform.localPosition;
            iButton.transform.localPosition = new Vector3(iButton.transform.localPosition.x, iButton.transform.localPosition.y - 120, iButton.transform.localPosition.z);
            latestButton = iButton;
            iButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Path.GetFileName(directory);
            iButton.GetComponent<LoadFighter>().FilePath = directory;
        }
    }

    public void ReadCharSel()
    {
        latestButton = NewButton;
        CharManager.P1Fighter = null;

        foreach (GameObject i in GameObject.FindGameObjectsWithTag("FileRead"))
        {
            Destroy(i);
        }

        dir = Directory.GetDirectories(Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters" + Path.DirectorySeparatorChar);
        bool nextLine = true;
        foreach (string directory in dir)
        {
            GameObject iButton = Instantiate(button, canvas.transform);
            iButton.transform.localPosition = latestButton.transform.localPosition;
            if (nextLine)
            {
                iButton.transform.localPosition = new Vector3(iButton.transform.localPosition.x + 300, iButton.transform.localPosition.y + 300, iButton.transform.localPosition.z);
            } else
            {
                iButton.transform.localPosition = new Vector3(iButton.transform.localPosition.x, iButton.transform.localPosition.y - 300, iButton.transform.localPosition.z);
            }
            latestButton = iButton;
            iButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Path.GetFileName(directory);
            iButton.GetComponent<LoadFighter>().FilePath = directory;
            nextLine = !nextLine;
        }
    }

    public void ToggleDeleteMode()
    {
        delete = !delete;
    }
}
