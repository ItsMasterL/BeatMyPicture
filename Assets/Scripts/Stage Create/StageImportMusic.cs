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
            audioloader.GetComponent<AudioLoader>().GetSpecificAudioFromFolder(OpenStage.carryover + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar, filename);
            return;
        }
        FileImporter.GetFile("audio");
        if (FileImporter.LastResult.Substring(FileImporter.LastResult.Length - 3) == "mp3")
            File.Copy(FileImporter.LastResult, OpenStage.carryover + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar + filename + ".mp3");
        if (FileImporter.LastResult.Substring(FileImporter.LastResult.Length - 3) == "wav")
            File.Copy(FileImporter.LastResult, OpenStage.carryover + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar + filename + ".wav");
        if (FileImporter.LastResult.Substring(FileImporter.LastResult.Length - 3) == "ogg")
            File.Copy(FileImporter.LastResult, OpenStage.carryover + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar + filename + ".ogg");
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
}
