using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AudioLoader : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> songs;
    private AudioSource sound;
    bool forcedWait;

    public void Awake()
    {
        sound = GetComponent<AudioSource>();
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
            StartCoroutine(ConvertFilesToAudioClip(songFile));
        }

    }

    public bool IsAudioLengthValid(string filepath, string ID)
    {
        songs.Clear();
        DirectoryInfo directoryInfo = new DirectoryInfo(filepath);
        FileInfo[] songFiles = directoryInfo.GetFiles(ID + ".*");


        foreach (FileInfo songFile in songFiles)
        {
            StartCoroutine(ConvertFilesToAudioClipCheck(songFile));
            sound.clip = songs[songs.Count - 1];
            if (sound.clip.length > 5)
            {
                sound.clip = null;
                songs.Clear();
                forcedWait = false;
                return false;
            }
        }
        forcedWait = false;
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

    private IEnumerator ConvertFilesToAudioClipCheck(FileInfo songFile)
    {
        forcedWait = true;
        if (songFile.Name.Contains("meta"))
        {
            forcedWait = false;
            yield break;
        }
        else
        {
            string songName = songFile.FullName.ToString();
            string url = string.Format("file://{0}", songName);
            WWW www = new WWW(url);
            yield return www;
            songs.Add(www.GetAudioClip(false, false));
            forcedWait = false;
        }
    }

    private void OnDisable()
    {
        sound.Stop();
        sound.clip = null;
        songs.Clear();
        forcedWait = false;
    }
}
