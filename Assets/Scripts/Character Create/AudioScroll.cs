using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioScroll : MonoBehaviour
{
    public GameObject includedButton;

    public GameObject[] buttonsImp;
    public GameObject[] buttonsRec;
    public GameObject firstImp;
    public GameObject lastImp;
    bool enableScrollImp;
    float lastPosImp;
    public GameObject firstRec;
    public GameObject lastRec;
    bool enableScrollRec;
    float lastPosRec;

    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        GetComponent<Scrollbar>().SetValueWithoutNotify(0);
        if (buttonsImp != null)
            for (int i = 0; i < buttonsImp.Length; i++)
            {
                buttonsImp[i] = null;
            }

        if (buttonsRec != null)
            for (int i = 0; i < buttonsRec.Length; i++)
            {
                buttonsRec[i] = null;
            }

        buttonsImp = GameObject.FindGameObjectsWithTag("AudioImports");
        buttonsRec = GameObject.FindGameObjectsWithTag("AudioRecord");
        if (buttonsImp.Length > 0)
        {
            firstImp = buttonsImp[0].gameObject;
            lastImp = buttonsImp[buttonsImp.Length - 1].gameObject;
            enableScrollImp = false;
            if (buttonsImp.Length > 3) enableScrollImp = true;
            lastPosImp = lastImp.transform.localPosition.x - firstImp.transform.localPosition.x;
            GetComponent<Scrollbar>().numberOfSteps = (buttonsImp.Length - 1) * 150;
        }
        if (buttonsRec.Length > 0)
        {
            firstRec = buttonsRec[0].gameObject;
            lastRec = buttonsRec[buttonsRec.Length - 1].gameObject;
            enableScrollRec = false;
            if (buttonsRec.Length > 3) enableScrollRec = true;
            lastPosRec = lastRec.transform.localPosition.x - firstRec.transform.localPosition.x;
            GetComponent<Scrollbar>().numberOfSteps = (buttonsRec.Length - 1) * 150;
        }
    }

    public void Slidin()
    {
        if (includedButton.GetComponent<Button>().interactable)
        {
            if (enableScrollRec)
                foreach (GameObject i in buttonsRec)
                {
                    if (i == null)
                    {
                        Refresh();
                        return;
                    }
                    i.transform.localPosition = new Vector3
                            (i.GetComponent<AudioProperties>().xPos - GetComponent<Scrollbar>().value * (GetComponent<Scrollbar>().numberOfSteps),
                            i.transform.localPosition.y, i.transform.localPosition.z);
                }
        }
        else
        {
            if (enableScrollImp)
                foreach (GameObject i in buttonsImp)
                {
                    if (i == null)
                    {
                        Refresh();
                        return;
                    }
                    i.transform.localPosition = new Vector3
                            (i.GetComponent<AudioProperties>().xPos - GetComponent<Scrollbar>().value * (GetComponent<Scrollbar>().numberOfSteps),
                            i.transform.localPosition.y, i.transform.localPosition.z);
                }
        }
    }
}
