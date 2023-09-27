using UnityEngine;
using System.IO;
using TMPro;

public class SetupFiles : MonoBehaviour
{
    string path;
    public TextMeshProUGUI display;

    #region Profile Setup
    public GameObject profileSetup;
    public GameObject usernameWarning;
    public GameObject UsernameField;
    public GameObject BioField;
    public class Profile
    {
        public string Username = "";
        public string UUID = "";
        public string Bio = "";
        public int ColorID = 10;
        public string version = "0.0.1";
        public string Edittime = "";
    }
    public static Profile profile = new Profile();
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        path = Application.persistentDataPath;
        display.text = "Files can be found in " + path;
        Directory.CreateDirectory(path);
        Directory.CreateDirectory(path + Path.DirectorySeparatorChar + "Fighters");
        Directory.CreateDirectory(path + Path.DirectorySeparatorChar + "Stages");
        Directory.CreateDirectory(path + Path.DirectorySeparatorChar + "Templates");
        if (!File.Exists(path + Path.DirectorySeparatorChar + "ReadMe.txt"))
        {
            File.WriteAllText(path + Path.DirectorySeparatorChar + "ReadMe.txt", "Thank you for playing Beat My Picture! \n\nYou can edit any json file manually here, but DO NOT EDIT .noedit FILES!\n" +
                "Editing those files WILL result in loss of data!");
        }

        if (!File.Exists(path + Path.DirectorySeparatorChar + "profile.noedit"))
        {
            profileSetup.SetActive(true);
        } else
        {
            string JsonFile = File.ReadAllText(path + Path.DirectorySeparatorChar + "profile.noedit");
            profile = JsonUtility.FromJson<Profile>(JsonFile);
            if (profile.Edittime != File.GetLastWriteTime(path + Path.DirectorySeparatorChar + "profile.noedit").ToString())
            {
                Debug.LogWarning("Profile edited externally!!");
                Debug.LogWarning(File.GetLastWriteTime(path + Path.DirectorySeparatorChar + "profile.noedit").ToString() + " vs saved time of " + profile.Edittime + "!! File deleted!");
                File.Delete(path + Path.DirectorySeparatorChar + "profile.noedit");

                profileSetup.SetActive(true);
            } else
            {
                foreach (SetColor col in FindObjectsOfType<SetColor>())
                {
                    col.UpdateColor();
                }
            }
        }

        if (!Directory.Exists(path + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "MagicWarrior"))
        {
            
        }
    }

    public void SetFavColor(int Input)
    {
        profile.ColorID = Input;
        foreach(SetColor col in FindObjectsOfType<SetColor>())
        {
            col.UpdateColor();
        }
    }

    public void SetUsername(string Input)
    {
        profile.Username = Input.Trim();
    }
    
    public void SetBio(string Input)
    {
        profile.Bio = Input;
    }

    public void SetProfile()
    {
        if (profile.Username != "")
        {
            if (profile.UUID == "")
            {
                profile.UUID = System.Guid.NewGuid().ToString();
            }

            profile.Edittime = System.DateTime.Now.ToString();
            string Output = JsonUtility.ToJson(profile);
            Debug.Log(profile.Edittime);
            File.WriteAllText(path + Path.DirectorySeparatorChar + "profile.noedit", Output);
            profileSetup.SetActive(false);
        } else
        {
            usernameWarning.SetActive(true);
        }
    }

    public void EditProfile()
    {
        string JsonFile = File.ReadAllText(path + Path.DirectorySeparatorChar + "profile.noedit");
        profile = JsonUtility.FromJson<Profile>(JsonFile);
        UsernameField.GetComponent<TMP_InputField>().text = profile.Username;
        BioField.GetComponent<TMP_InputField>().text = profile.Bio;
        profileSetup.SetActive(true);
    }
}
