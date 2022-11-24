using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileImporter : MonoBehaviour
{
    public static string LastResult;

    public static void GetFile(string filetype)
    {
            // Don't attempt to import/export files if the file picker is already open
            if (NativeFilePicker.IsFilePickerBusy())
                return;
            if (filetype == "audio")
            {
#if UNITY_ANDROID
                // Use MIMEs on Android
                string[] fileTypes = new string[] { "MP3" };
#else
			// Use UTIs on iOS
			string[] fileTypes = new string[] { "public.audio" }; //hehe future thinking
#endif

                // Pick audio
                NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
                {
                    if (path == null)
                        Debug.Log("Operation cancelled");
                    else
                        Debug.Log("Picked file: " + path);
                    LastResult = path;
                },  fileTypes );

            Debug.Log("Permission result: " + permission);
            }
           
    }

}
