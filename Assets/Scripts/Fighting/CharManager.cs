using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharManager : MonoBehaviour
{
    public static string mode;
    public static bool canpause;
    public static string stage;
    public static string P1Fighter;
    public static int P1Controls = 0;

    public void SetMode(string value)
    {
        mode = value;
    }

    public void SetStage()
    {
        stage = gameObject.GetComponent<LoadStage>().FilePath;
    }

    public void SetP1Fighter()
    {
        P1Fighter = gameObject.GetComponent<LoadFighter>().FilePath;
    }

    public void SetP1Controls(int value)
    {
        P1Controls = value;
    }

    public void FighterCheck(GameObject nextScene)
    {
        if (P1Fighter != null)
        {
            GameObject.Find("charselect").SetActive(false);
            nextScene.SetActive(true);
        }
    }

    public void StageCheck(string nextScene)
    {
        if (stage != null)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
