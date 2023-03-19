using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FavoriteColorIcon : MonoBehaviour
{
    public GameObject defaultPos;
    public Color col;

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(defaultPos.transform.localPosition.x + (SetupFiles.profile.ColorID % 4 * 190), defaultPos.transform.localPosition.y + (SetupFiles.profile.ColorID / 4 * -190), 0);
    }
}
