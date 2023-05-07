using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class StageManager : MonoBehaviour
{
    public GameObject musicPlayer;
    public bool main;
    // Start is called before the first frame update
    void Start()
    {
        byte[] fileData = File.ReadAllBytes(CharManager.stage + Path.DirectorySeparatorChar + "day.png"); //TODO: Change based on time of day
        Texture2D tex = new Texture2D(2, 2);
        tex.filterMode = FilterMode.Point;
        tex.LoadImage(fileData);
        GetComponent<RawImage>().texture = tex;

        if (main)
        {
            string json = File.ReadAllText(CharManager.stage + Path.DirectorySeparatorChar + "stage.json");
            StageImportMusic.songLoopInfo info = JsonUtility.FromJson<StageImportMusic.songLoopInfo>(json);

            if (info.daySong == "day")
            {
                musicPlayer.GetComponent<OSTPlayback>().customSong = true;
                musicPlayer.GetComponent<OSTPlayback>().startpos = info.dayStartPos;
                musicPlayer.GetComponent<OSTPlayback>().looppos = info.dayLoopPos;
                musicPlayer.GetComponent<OSTPlayback>().endpos = info.dayEndPos;
                
                if (File.Exists(CharManager.stage + Path.DirectorySeparatorChar + "daymusic.mp3"))
                {
                    musicPlayer.GetComponent<AudioLoader>().GetSpecificAudioFromFolder(CharManager.stage + Path.DirectorySeparatorChar, "daymusic.mp3");
                }
                else if (File.Exists(CharManager.stage + Path.DirectorySeparatorChar  + "daymusic.wav"))
                {
                    musicPlayer.GetComponent<AudioLoader>().GetSpecificAudioFromFolder(CharManager.stage + Path.DirectorySeparatorChar, "daymusic.wav");
                }
                else if (File.Exists(CharManager.stage + Path.DirectorySeparatorChar + "daymusic.ogg"))
                {
                    musicPlayer.GetComponent<AudioLoader>().GetSpecificAudioFromFolder(CharManager.stage + Path.DirectorySeparatorChar, "daymusic.ogg");
                }
                //GetSpecificSongToFightingScene handles it from here
            } else
            {
                musicPlayer.GetComponent<OSTPlayback>().songChoice = int.Parse(info.daySong);
                musicPlayer.GetComponent<OSTPlayback>().PlayPause();
            }
        }
    }
}
