using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepDelay : MonoBehaviour
{
    public GameObject disable;
    public GameObject enable;
    public List<GameObject> immediateDisable;
    float delta;

    private void Update()
    {
        if (delta > 0)
        {
            delta -= Time.deltaTime;
        }
        if (delta < 0)
        {
            delta = 0;
            foreach (GameObject obj in immediateDisable)
            {
                obj.SetActive(true);
            }
            enable.SetActive(true);
            disable.SetActive(false);
        }
    }

    public void PicStageReturn(float input)
    {
        foreach(GameObject obj in immediateDisable)
        {
            obj.SetActive(false);
        }
        delta = input;
    }
}
