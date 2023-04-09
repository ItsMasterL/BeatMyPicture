using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalettes : MonoBehaviour
{
    [System.Serializable]
    public class Palette
    {
        public Color main;
        public Color lightmain;
        public Color darkmain;
        public Color secondary;
        public Color lightsecondary;
        public Color darksecondary;
    }
    [SerializeField]
    public List<Palette> colorset;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
