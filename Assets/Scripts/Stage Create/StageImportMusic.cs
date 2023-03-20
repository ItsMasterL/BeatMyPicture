using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StageImportMusic : MonoBehaviour
{
    public GameObject audioloader;
    public string lastModified;
    public void ImportAudio(string filename)
    {
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
        //audioeditor.SetActive(true);

        /*if file is too long to use as a sound effect
        if (GameObject.Find("PlaySound").GetComponent<AudioLoader>().IsAudioLengthValid(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Sound" + Path.DirectorySeparatorChar, filename) == false)
        {
            if (File.Exists(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Sound" + Path.DirectorySeparatorChar + filename + ".mp3"))
                File.Delete(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Sound" + Path.DirectorySeparatorChar + filename + ".mp3");
            if (File.Exists(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Sound" + Path.DirectorySeparatorChar + filename + ".wav"))
                File.Delete(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Sound" + Path.DirectorySeparatorChar + filename + ".wav");
            if (File.Exists(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Sound" + Path.DirectorySeparatorChar + filename + ".ogg"))
                File.Delete(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Sound" + Path.DirectorySeparatorChar + filename + ".ogg");
            loaderror.SetActive(true);
        }
        LoadAudioButtons();
        */
    }

    public void LoadAudio(string filename)
    {
        if (File.Exists(OpenStage.carryover + Path.DirectorySeparatorChar + filename + ".mp3"))
        {
            audioloader.GetComponent<AudioLoader>().GetSpecificAudioFromFolder(OpenStage.carryover + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar, filename + ".mp3");
            return;
        } else if (File.Exists(OpenStage.carryover + Path.DirectorySeparatorChar + filename + ".wav"))
        {
            audioloader.GetComponent<AudioLoader>().GetSpecificAudioFromFolder(OpenStage.carryover + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar, filename + ".wav");
            return;
        } else if (File.Exists(OpenStage.carryover + Path.DirectorySeparatorChar + filename + ".ogg"))
        {
            audioloader.GetComponent<AudioLoader>().GetSpecificAudioFromFolder(OpenStage.carryover + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar, filename + ".ogg");
            return;
        }
        {
            Debug.Log("File not found!");
        }
    }
}
