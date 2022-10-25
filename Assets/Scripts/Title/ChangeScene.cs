using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void SceneChangeLandscape(string input)
    {
        Screen.orientation = ScreenOrientation.Landscape;
        
        SceneManager.LoadScene(input);
    }

    public void SceneChangePortrait(string input)
    {
        Screen.orientation = ScreenOrientation.Portrait;

        SceneManager.LoadScene(input);
    }
}
