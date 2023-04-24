using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemplateSelectScroll : MonoBehaviour
{
    public GameObject[] buttons;
    public GameObject first;
    public GameObject last;
    bool enableScroll;
    float lastPos;

    public bool isForFighter;
    public bool isForSelect;

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
        if (buttons.Length > 8 && !isForFighter) enableScroll = true;
        if (buttons.Length > 4 && isForFighter) enableScroll = true;
        if (buttons.Length > 8 && isForSelect) enableScroll = true;
        lastPos = last.transform.localPosition.y - first.transform.localPosition.y;
        GetComponent<Scrollbar>().numberOfSteps = (buttons.Length - 1) * 120;
    }

    public void SlidinTemplate()
    {
        if (enableScroll)
            foreach (GameObject i in buttons)
            {
                if (i == null)
                {
                    Refresh();
                    return;
                }
                if (isForFighter)
                {
                    i.transform.localPosition = new Vector3
                            (i.transform.localPosition.x,
                            i.GetComponent<LoadTemplate>().yPos + 120 + GetComponent<Scrollbar>().value * GetComponent<Scrollbar>().numberOfSteps, i.transform.localPosition.z);
                }
                else
                {
                    i.transform.localPosition = new Vector3
                            (i.transform.localPosition.x,
                            i.GetComponent<LoadTemplate>().yPos + GetComponent<Scrollbar>().value * GetComponent<Scrollbar>().numberOfSteps, i.transform.localPosition.z);
                }
            }
    }

    public void SlidinFighter()
    {
        if (enableScroll)
            foreach (GameObject i in buttons)
            {
                if (i == null)
                {
                    Refresh();
                    return;
                }
                    i.transform.localPosition = new Vector3
                            (i.transform.localPosition.x,
                            i.GetComponent<LoadFighter>().yPos + GetComponent<Scrollbar>().value * GetComponent<Scrollbar>().numberOfSteps, i.transform.localPosition.z);
            }
    }

    public void SlidinStage()
    {
        if (enableScroll)
            foreach (GameObject i in buttons)
            {
                if (i == null)
                {
                    Refresh();
                    return;
                }
                    i.transform.localPosition = new Vector3
                            (i.transform.localPosition.x,
                            i.GetComponent<LoadStage>().yPos + GetComponent<Scrollbar>().value * GetComponent<Scrollbar>().numberOfSteps, i.transform.localPosition.z);
            }
    }

    public void SlidinFighterSelect()
    {
        if (enableScroll)
            foreach (GameObject i in buttons)
            {
                if (i == null)
                {
                    Refresh();
                    return;
                }
                i.transform.localPosition = new Vector3
                        (i.GetComponent<LoadFighter>().xPos + GetComponent<Scrollbar>().value * GetComponent<Scrollbar>().numberOfSteps,
                        i.transform.localPosition.y, i.transform.localPosition.z);
            }
    }
}
