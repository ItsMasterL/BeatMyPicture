using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvisibleIfNoDebug : MonoBehaviour
{
    public List<PlayerManager> pm;
    bool debug;
    Image img;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        debug = false;
        foreach(PlayerManager playerManager in pm)
        {
            if (playerManager.debugMode) debug = true;
        }

        if (debug)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
        } else
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
        }
    }
}
