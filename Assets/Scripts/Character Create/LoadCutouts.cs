using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class LoadCutouts : MonoBehaviour
{
    public GameObject NewButton;
    public GameObject NewBg;
    public GameObject button;
    public GameObject canvas;
    GameObject latestButton;

    public string ID;
    public TextMeshProUGUI displayID;

    public GameObject scrollbar;

    // Start is called before the first frame update
    void Start()
    {
        LoadImages();
    }

    public void LoadImages()
    {
        latestButton = NewButton;

        foreach (GameObject i in GameObject.FindGameObjectsWithTag("FileReadCutout"))
        {
            Destroy(i);
        }

        string[] dir = Directory.GetFiles(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Cutouts");

        foreach (string file in dir)
        {
            GameObject iButton = Instantiate(button, canvas.transform);
            iButton.transform.localPosition = latestButton.transform.localPosition;
            iButton.transform.localPosition = new Vector3(iButton.transform.localPosition.x + 170, iButton.transform.localPosition.y, iButton.transform.localPosition.z);
            latestButton = iButton;
            iButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 4);
            iButton.GetComponent<CutoutProperties>().ImageID = Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 4);
        }
        NewBg.transform.SetAsLastSibling();
        NewButton.transform.SetAsLastSibling();
        scrollbar.GetComponent<ScrollCutouts>().Refresh();
    }

    public void NewImage()
    {
        string[] dir = Directory.GetFiles(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Cutouts");
        int i = 0;
        ID = "000";
        while (File.Exists(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Cutouts" + Path.DirectorySeparatorChar + ID + ".png"))
        {
            i++;
            ID = (i).ToString("000");
        }
        displayID.text = ID;
    }
}
