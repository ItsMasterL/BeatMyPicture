using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagPage : MonoBehaviour
{
    private void OnEnable()
    {
        LoadInfoFromTemplates.RemoveBackground(gameObject);
    }
}
