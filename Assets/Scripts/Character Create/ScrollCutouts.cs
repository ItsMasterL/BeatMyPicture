using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollCutouts : MonoBehaviour
{
    public GameObject[] buttons;
    public GameObject first;
    public GameObject last;
    bool enableScroll;
    float lastPos;

    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        GetComponent<Scrollbar>().SetValueWithoutNotify(0);
        if (buttons != null)
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = null;
        }
        buttons = GameObject.FindGameObjectsWithTag("FileRead");
        first = buttons[0].gameObject;
        last = buttons[buttons.Length - 1].gameObject;
        enableScroll = false;
        if (buttons.Length > 3) enableScroll = true;
        lastPos = last.transform.localPosition.x - first.transform.localPosition.x;
        GetComponent<Scrollbar>().numberOfSteps = (buttons.Length - 1) * 170;
    }

    public void Slidin()
    {
        if (enableScroll)
        foreach(GameObject i in buttons)
        {
                if (i == null)
                {
                    Refresh();
                    return;
                }
            i.transform.localPosition = new Vector3
                    (i.GetComponent<CutoutProperties>().xPos - GetComponent<Scrollbar>().value * (GetComponent<Scrollbar>().numberOfSteps),
                    i.transform.localPosition.y, i.transform.localPosition.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
