using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class AudioLoader : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> songs;
    private AudioSource sound;
    float temptimer;
    public float lastImportLength;

    public GameObject slider;
    public GameObject startpos;
    public GameObject looppos;
    public GameObject endpos;

    public GameObject songpos;

    public void Awake()
    {
        sound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (songpos != null)
        {
            if (sound.isPlaying)
            {
                songpos.GetComponent<TextMeshProUGUI>().text = "Current Pos: " + sound.time.ToString("0.000") + "s";
            }
            else
            {
                songpos.GetComponent<TextMeshProUGUI>().text = "Current Pos: " + slider.GetComponent<Slider>().value.ToString("0.000") + "s";
            }
        }
        //if (startpos != null && looppos != null && endpos != null) //Makes sure songs dont play forever or something. Fix before release
        //{
        //    if (endpos.GetComponent<TextMeshProUGUI>().text != "" && startpos.GetComponent<TextMeshProUGUI>().text != "" &&
        //        float.Parse(endpos.GetComponent<TextMeshProUGUI>().text) <= float.Parse(startpos.GetComponent<TextMeshProUGUI>().text))
        //    {
        //        endpos.GetComponent<TMP_InputField>().text = sound.clip.length.ToString();
        //    }
        //    if (endpos.GetComponent<TextMeshProUGUI>().text != "" && looppos.GetComponent<TextMeshProUGUI>().text != "" &&
        //        float.Parse(endpos.GetComponent<TextMeshProUGUI>().text) <= float.Parse(looppos.GetComponent<TextMeshProUGUI>().text))
        //    {
        //        looppos.GetComponent<TMP_InputField>().text = "0";
        //    }
        //}
    }

    public void GetAllAudioFromFolder(string filepath)
    {
        songs.Clear();
        DirectoryInfo directoryInfo = new DirectoryInfo(filepath);
        FileInfo[] songFiles = directoryInfo.GetFiles("*.*");

        foreach (FileInfo songFile in songFiles)
        {
            StartCoroutine(ConvertFilesToAudioClip(songFile));
        }

    }

    public void GetSpecificAudioFromFolder(string filepath, string ID)
    {
        songs.Clear();
        DirectoryInfo directoryInfo = new DirectoryInfo(filepath);
        FileInfo[] songFiles = directoryInfo.GetFiles(ID + ".*");
        

        foreach (FileInfo songFile in songFiles)
        {
            Debug.Log(songFile.Length);
            lastImportLength = songFile.Length;
            StartCoroutine(ConvertFilesToAudioClip(songFile));
        }

    }

    public void GetSpecificSongFromFolder(string filepath, string ID)
    {
        songs.Clear();
        DirectoryInfo directoryInfo = new DirectoryInfo(filepath);
        FileInfo[] songFiles = directoryInfo.GetFiles(ID + ".*");


        foreach (FileInfo songFile in songFiles)
        {
            Debug.Log(songFile.Length);
            lastImportLength = songFile.Length;
            StartCoroutine(ConvertFilesToMusicClip(songFile));
        }

    }

    public bool IsAudioLengthValid(string filepath, string ID)
    {
        songs.Clear();
        DirectoryInfo directoryInfo = new DirectoryInfo(filepath);
        FileInfo[] songFiles = directoryInfo.GetFiles(ID + ".*");


        foreach (FileInfo songFile in songFiles)
        {
            if (songFile.Name.Contains("meta"))
            {
                Debug.Log("Not valid");
                return false;
            }
            else
            {
                string songName = songFile.FullName.ToString();
                string url = string.Format("file://{0}", songName);
                WWW www = new WWW(url);
                songs.Add(www.GetAudioClip(false, true));
            }
            sound.clip = songs[songs.Count - 1];
            temptimer = 0;
            while (sound.clip.length == 0 && temptimer < 1)
            {
                temptimer += 0.001f;
            }
            Debug.Log(sound.clip.length);
            if (sound.clip.length > 5)
            {
                sound.clip = null;
                songs.Clear();
                Debug.Log("Too Long");
                return false;
            }
        }
        Debug.Log("Not too long, since it is " + sound.clip.length + " seconds long");
        return true;
    }

    public void PlayAudio(int index)
    {
        sound.clip = songs[index];
        if (sound.clip.length <= 5)
        {
            sound.Play();
        } else
        {
            Debug.Log("The audio clip is too long!");
        }
        
    }
    
    public void PlayMusic(int index)
    {
        if (songs.Count == 1)
        {
            sound.clip = songs[index];
            sound.Play();
        } else
        {
            if (StageImportMusic.timeEdit == 1)
                LoadSong("duskmusic");
            else if (StageImportMusic.timeEdit == 2)
                LoadSong("nightmusic");
            else
                LoadSong("daymusic");
        }
        
    }

    public void PlayPause(int index)
    {
        if (!GetComponent<AudioSource>().isPlaying && songs.Count == 1)
        {
            sound.clip = songs[index];
            if (slider.GetComponent<Slider>().maxValue != sound.clip.length)
            {
                slider.GetComponent<AudioSeek>().SetMaxLength(sound.clip.length);
                startpos.GetComponent<TMP_InputField>().text = "0";
                looppos.GetComponent<TMP_InputField>().text = "0";
                endpos.GetComponent<TMP_InputField>().text = sound.clip.length.ToString();
            }
            sound.time = slider.GetComponent<AudioSeek>().slider.value;
            sound.Play();
            return;
        }
        if (GetComponent<AudioSource>().isPlaying && songs.Count == 1)
        {
            sound.Pause();
            return;
        }
        if (songs.Count != 1)
        {
            if (StageImportMusic.timeEdit == 1)
                LoadSong("duskmusic");
            else if (StageImportMusic.timeEdit == 2)
                LoadSong("nightmusic");
            else
                LoadSong("daymusic");
        }
    }

    public void LoadSong(string filename)
    {
        GameObject.Find("Stage Menu").GetComponent<StageImportMusic>().LoadAudio(filename);
    }

    public void LoadProperties()
    {
        slider.GetComponent<AudioSeek>().SetMaxLength(sound.clip.length);
        if (startpos.GetComponent<TMP_InputField>().text == "")
        startpos.GetComponent<TMP_InputField>().text = "0";
        if (looppos.GetComponent<TMP_InputField>().text == "")
        looppos.GetComponent<TMP_InputField>().text = "0";
        if (endpos.GetComponent<TMP_InputField>().text == "")
        endpos.GetComponent<TMP_InputField>().text = sound.clip.length.ToString();
    }

    private IEnumerator ConvertFilesToAudioClip(FileInfo songFile)
    {
        if (songFile.Name.Contains("meta"))
            yield break;
        else
        {
            string songName = songFile.FullName.ToString();
            string url = string.Format("file://{0}", songName);
            WWW www = new WWW(url);
            yield return www;
            songs.Add(www.GetAudioClip(false, false));
        }
    }

    private IEnumerator ConvertFilesToMusicClip(FileInfo songFile)
    {
        if (songFile.Name.Contains("meta"))
            yield break;
        else
        {
            string songName = songFile.FullName.ToString();
            string url = string.Format("file://{0}", songName);
            WWW www = new WWW(url);
            yield return www;
            songs.Add(www.GetAudioClip(false, false));
            LoadProperties();

        }
    }

    private void OnDisable()
    {
        ClearSound();
    }

    public void ClearSound()
    {
        sound.Stop();
        sound.clip = null;
        songs.Clear();
    }
}
