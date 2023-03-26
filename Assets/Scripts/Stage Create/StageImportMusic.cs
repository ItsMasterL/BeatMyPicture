using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class StageImportMusic : MonoBehaviour
{
    public GameObject audioloader;
    public string lastModified;
    public static int timeEdit;
    public SongTypeDisplay display;
    public OSTPlayback OST;

    #region gameobjects for gathering info
    [Space(30)]
    public TMP_InputField startpos;
    public TMP_InputField looppos;
    public TMP_InputField endpos;
    songLoopInfo Info;
    #endregion
    public class songLoopInfo
    {
        public string version = "0.0.1";

        public bool duskSongEnabled;
        public bool duskPictureEnabled;
        public bool nightEnabled;
        public bool nightPictureEnabled;

        public string daySong;
        public float dayStartPos;
        public float dayLoopPos;
        public float dayEndPos;

        public string duskSong;
        public float duskStartPos;
        public float duskLoopPos;
        public float duskEndPos;

        public string nightSong;
        public float nightStartPos;
        public float nightLoopPos;
        public float nightEndPos;
    }

    private void Start()
    {
        Info = new songLoopInfo();
    }

    public void ImportAudio()
    {
        string filename;
        if (timeEdit == 1)
        {
            filename = "duskmusic";
        } else if (timeEdit == 2)
        {
            filename = "nightmusic";
        } else
        {
            filename = "daymusic";
        }
        lastModified = filename;
        if (File.Exists(OpenStage.carryover + Path.DirectorySeparatorChar + filename + ".mp3") ||
            File.Exists(OpenStage.carryover + Path.DirectorySeparatorChar + filename + ".wav") ||
            File.Exists(OpenStage.carryover + Path.DirectorySeparatorChar + filename + ".ogg"))
        {
            File.Delete(OpenStage.carryover + Path.DirectorySeparatorChar + filename + ".mp3");
            File.Delete(OpenStage.carryover + Path.DirectorySeparatorChar + filename + ".wav");
            File.Delete(OpenStage.carryover + Path.DirectorySeparatorChar + filename + ".ogg");
            GameObject.Find("PlayCustom").GetComponent<AudioLoader>().ClearSound();
        }
        FileImporter.GetFile("audio");
        if (FileImporter.LastResult.Substring(FileImporter.LastResult.Length - 3) == "mp3")
            File.Copy(FileImporter.LastResult, OpenStage.carryover + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar + filename + ".mp3");
        if (FileImporter.LastResult.Substring(FileImporter.LastResult.Length - 3) == "wav")
            File.Copy(FileImporter.LastResult, OpenStage.carryover + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar + filename + ".wav");
        if (FileImporter.LastResult.Substring(FileImporter.LastResult.Length - 3) == "ogg")
            File.Copy(FileImporter.LastResult, OpenStage.carryover + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar + filename + ".ogg");
        GameObject.Find("PlayCustom").GetComponent<AudioLoader>().PlayMusic(0); //This preloads the song
    }

    public void LoadAudio(string filename)
    {
        if (File.Exists(OpenStage.carryover + Path.DirectorySeparatorChar + filename + ".mp3"))
        {
            audioloader.GetComponent<AudioLoader>().GetSpecificSongFromFolder(OpenStage.carryover + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar, filename + ".mp3");
            return;
        } else if (File.Exists(OpenStage.carryover + Path.DirectorySeparatorChar + filename + ".wav"))
        {
            audioloader.GetComponent<AudioLoader>().GetSpecificSongFromFolder(OpenStage.carryover + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar, filename + ".wav");
            return;
        } else if (File.Exists(OpenStage.carryover + Path.DirectorySeparatorChar + filename + ".ogg"))
        {
            audioloader.GetComponent<AudioLoader>().GetSpecificSongFromFolder(OpenStage.carryover + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar, filename + ".ogg");
            return;
        }
        {
            Debug.Log("File not found!");
        }
    }

    public void CustomSongChoice(bool custom)
    {
        if (timeEdit == 1)
        {
            if (custom == true)
                Info.duskSong = "dusk";
            else
                Info.duskSong = OST.songChoice.ToString("000");
        }
        else if (timeEdit == 2)
        {
            if (custom == true)
                Info.nightSong = "night";
            else
                Info.nightSong = OST.songChoice.ToString("000");
        }
        else
        {
            if (custom == true)
                Info.daySong = "day";
            else
                Info.daySong = OST.songChoice.ToString("000");
        }
    }

    public void SaveValues()
    {
        if (timeEdit == 1)
        {
            Info.duskStartPos = float.Parse(startpos.text);
            Info.duskLoopPos = float.Parse(looppos.text);
            Info.duskEndPos = float.Parse(endpos.text);
        } else if (timeEdit == 2)
        {
            Info.nightStartPos = float.Parse(startpos.text);
            Info.nightLoopPos = float.Parse(looppos.text);
            Info.nightEndPos = float.Parse(endpos.text);
        } else
        {
            Info.dayStartPos = float.Parse(startpos.text);
            Info.dayLoopPos = float.Parse(looppos.text);
            Info.dayEndPos = float.Parse(endpos.text);
        }
        SaveStageJSON();
    }

    public void LoadValues(int time)
    {
        timeEdit = time;
        if (Info.version == "0.0.1")
        {
            if (time == 1)
            {
                startpos.text = Info.duskStartPos.ToString();
                looppos.text = Info.duskLoopPos.ToString();
                endpos.text = Info.duskEndPos.ToString();
                display.UpdateDisplay(Info.duskSong);
                if (int.TryParse(Info.duskSong, out int j) == true)
                {
                    OST.SetSong(int.Parse(Info.duskSong));
                }
            }
            else if (time == 2)
            {
                startpos.text = Info.nightStartPos.ToString();
                looppos.text = Info.nightLoopPos.ToString();
                endpos.text = Info.nightEndPos.ToString();
                display.UpdateDisplay(Info.nightSong);
                if (int.TryParse(Info.nightSong, out int j) == true)
                {
                    OST.SetSong(int.Parse(Info.nightSong));
                }
            }
            else
            {
                startpos.text = Info.dayStartPos.ToString();
                looppos.text = Info.dayLoopPos.ToString();
                endpos.text = Info.dayEndPos.ToString();
                display.UpdateDisplay(Info.daySong);
                if (int.TryParse(Info.daySong, out int j) == true)
                {
                    OST.SetSong(int.Parse(Info.daySong));
                }
            }

        }
    }

    public void LoadStageJSON()
    {
        if (File.Exists(OpenStage.carryover + Path.DirectorySeparatorChar + "stage.json"))
        {
            string input = File.ReadAllText(OpenStage.carryover + Path.DirectorySeparatorChar + "stage.json");
            Info = JsonUtility.FromJson<songLoopInfo>(input);
        }
    }

    public void SaveStageJSON()
    {
        string output = JsonUtility.ToJson(Info);
        File.WriteAllText(OpenStage.carryover + Path.DirectorySeparatorChar + "stage.json", output);
    }
}
