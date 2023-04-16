using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FightSelection : MonoBehaviour
{
    [SerializeField]
    GameObject mainmenu;
    [SerializeField]
    GameObject nofighterwarning;
    [SerializeField]
    GameObject proceed;

    public void CheckFighters()
    {
        if (Directory.GetDirectories(Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters").Length != 0 && Directory.GetDirectories(Application.persistentDataPath + Path.DirectorySeparatorChar + "Stages").Length != 0)
        {
            proceed.SetActive(true);
            mainmenu.SetActive(false);
            return;
        }
        nofighterwarning.SetActive(true);
    }
}
