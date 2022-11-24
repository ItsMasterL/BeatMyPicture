using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class JSONManager : MonoBehaviour
{
    public GameObject NewButton;
    public GameObject NewBg;
    public GameObject button;
    public GameObject canvas;
    GameObject latestButton;

    public GameObject setNewButton;
    public GameObject setNewBg;
    public GameObject setButton;
    GameObject setLatestButton;

    public GameObject FINew;
    //public GameObject FINewBg;
    public GameObject FIInput;
    GameObject FIlatest;

    public string ID;
    public TextMeshProUGUI displayID;

    public string setID;
    public TextMeshProUGUI setDisplayID;

    public string audioID;

    public GameObject scrollbar;
    public GameObject setScrollbar;
    public GameObject FIScrollbar;

    public GameObject frameMenu;
    public GameObject setMenu;
    public GameObject setMask;

    #region All the game objects to read data from for frames
    [Space(10)]
    public GameObject frameimageid;
    public GameObject frametime;
    public GameObject invincible;
    public GameObject cancellable;
    public GameObject gravity;
    public GameObject pxmotion;
    public GameObject pymotion;
    public GameObject soundid;
    public GameObject dxpos;
    public GameObject dypos;
    public GameObject drad;
    public GameObject ddamage;
    public GameObject prxmotion;
    public GameObject prymotion;
    public GameObject prtime;
    public GameObject primageid;
    public GameObject nextframe;
    #endregion

    #region All the game objects to read data from for sets
    [Space(10)]
    public GameObject idle;
    public GameObject ground;
    public GameObject up;
    public GameObject down;
    public GameObject forward;
    public GameObject backward;
    public GameObject attack;
    public GameObject special;
    public GameObject taunt;
    public GameObject jump;
    public GameObject super;
    public GameObject exact;
    public GameObject[] frameorder;
    #endregion

    private void Start()
    {
        loadFrameButton();
        loadSetButton();
    }

    [System.Serializable]
    public class Frame
    {
        public string Version = "0.0.1"; //Not in editor
        public int FrameID = 0; //Not editable
        public string image = "000"; //Coresponds with cutout. Default to 000 if not found
        public float seconds = 0.5f; //Default to 0.5f if <= 0
        public float movex = 0f; //pos is forward, neg is backward || additive
        public float movey = 0f; //pos is up, neg is down || additive
        public bool invincible = false; //This can be abused. Try not to though.
        public float damage = 0f; //absolute value
        public float dmgxoffset = 0f; //pos is forward, neg is backward
        public float dmgyoffset = 0f; //pos is up, neg is down
        public float dmgradius = 0f; //every hitbox is circles
        public float projectilelife = 0f; //0 means no projectile
        public float projectilevectorx = 0f; //pos is forward, neg is backward
        public float projectilevectory = 0f; //pos is up, neg is down
        public string projectileimage = null; //default to 000. Hey, I just shot myself! No, not like that, i mean, I came out of my gun
        public bool cancancel = true; //If false, inputs are ignored while frame is active. Prevents turbo mode
        public bool hasgravity = true; //If false, gravity is tempoararily disabled during this frame, useful for precise control of character
        public string sound = null; //mp3 or wav. Hard length limit of probably 5 seconds, but unlimited layering unless theres an issue
        public string nextset = null; //Intended use for frame at end of set, to loop animations or easily use shared animations (Can softlock if cancancel is false on all frames!!)
    }

    [System.Serializable]
    public class AnimSet
    {
        public string Version = "0.0.1"; //Not in editor
        public int SetID; //not editable
        public List<string> frameIDs;
        //If multiple are assigned the below booleans, pick at random. Prioritize moves with most matching inputs otherwise
        public bool idle; //Ignore every other bool if toggled
        public bool whenGrounded; //Will the animation play only if on the ground, or only if in the air? (Yes, this means no shared arial/ground moves)
        public bool up; //Jumps will happen after a few frames to allow an up attack if tap jump is on (template properties)
        public bool down;
        public bool forward;
        public bool backward;
        public bool attack;
        public bool special;
        public bool taunt;
        public bool jump;
        public bool superspecial; //Only available when under 25% health
        public bool exactInput; //for example, can you taunt while holding an input or does it have to be neutral taunt
        public bool isPlaying; //Not in editor
    }

    [System.Serializable]
    public class AudioAndDescriptions
    {
        public List<int> SoundIDs;
        public List<string> IncludedSounds;
        public List<string> PoseDescriptions;
        public List<string> SoundDescriptions;
        public List<float> SoundLimits;
    }
    
    public AnimSet Set = new AnimSet();
    public Frame frame = new Frame();
    public AudioAndDescriptions desc = new AudioAndDescriptions();
    
    public void newFrameJSON()
    {
        string[] dir = Directory.GetFiles(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Frames");
        int i = 0;
        ID = "000";
        while (File.Exists(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Frames" + Path.DirectorySeparatorChar + ID + ".json"))
        {
            i++;
            ID = (i).ToString("000");
        }
        displayID.text = "Frame " + ID;
    }

    public void newSetJSON()
    {
        string[] dir = Directory.GetFiles(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Sets");
        int i = 0;
        setID = "000";
        while (File.Exists(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Sets" + Path.DirectorySeparatorChar + setID + ".json"))
        {
            i++;
            setID = (i).ToString("000");
        }
        setDisplayID.text = "Frame Set " + setID;
    }

    public void newFrameOrder()
    {
        Set.frameIDs.Add("000");
        loadFrameInputs();
    }

    public void loadFrameButton()
    {
        latestButton = NewButton;

        foreach (GameObject i in GameObject.FindGameObjectsWithTag("FileReadFrame"))
        {
            Destroy(i);
        }

        string[] dir = Directory.GetFiles(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Frames");

        foreach (string file in dir)
        {
            GameObject iButton = Instantiate(button, canvas.transform);
            iButton.transform.localPosition = latestButton.transform.localPosition;
            iButton.transform.localPosition = new Vector3(iButton.transform.localPosition.x + 170, iButton.transform.localPosition.y, iButton.transform.localPosition.z);
            latestButton = iButton;
            iButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 5);
            iButton.GetComponent<FrameProperties>().FrameID = Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 5);
        }
        NewBg.transform.SetAsLastSibling();
        NewButton.transform.SetAsLastSibling();
        scrollbar.GetComponent<ScrollFrames>().Refresh();
    }

    public void loadSetButton()
    {
        setLatestButton = setNewButton;

        foreach (GameObject i in GameObject.FindGameObjectsWithTag("FileReadSet"))
        {
            Destroy(i);
        }

        string[] dir = Directory.GetFiles(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Sets");

        foreach (string file in dir)
        {
            GameObject iButton = Instantiate(setButton, canvas.transform);
            Debug.Log(iButton.name);
            iButton.transform.localPosition = setLatestButton.transform.localPosition;
            iButton.transform.localPosition = new Vector3(iButton.transform.localPosition.x + 170, iButton.transform.localPosition.y, iButton.transform.localPosition.z);
            setLatestButton = iButton;
            iButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 5);
            iButton.GetComponent<SetProperties>().SetID = Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 5);
        }
        setNewBg.transform.SetAsLastSibling();
        setNewButton.transform.SetAsLastSibling();
        setScrollbar.GetComponent<ScrollSets>().Refresh();
    }

    public void loadFrameInputs()
    {
        FIlatest = FINew;

        foreach (GameObject i in GameObject.FindGameObjectsWithTag("FrameOrder"))
        {
            Destroy(i);
        }

        string[] frameOrder = Set.frameIDs.ToArray();
        int ii = 0;

        foreach (string frame in frameOrder)
        {
            GameObject input = Instantiate(FIInput, setMask.transform);
            input.transform.localPosition = FIlatest.transform.localPosition;
            input.transform.localPosition = new Vector3(input.transform.localPosition.x, input.transform.localPosition.y - 111, input.transform.localPosition.z);
            FIlatest = input;
            input.GetComponent<FrameInputProperties>().index = ii;
            input.GetComponent<TMP_InputField>().text = frame;
            ii++;
        }
        //FINewBg.transform.SetAsLastSibling();
        FINew.transform.SetAsLastSibling();
        FIScrollbar.GetComponent<ScrollFrameInput>().Refresh();
    }

    public void loadFrameJSON(GameObject loader)
    {
        ID = loader.GetComponent<FrameProperties>().FrameID;
        displayID.text = "Frame " + ID;
        string path = OpenTemplate.carryover;
        frameMenu.SetActive(true);
        if (File.Exists(path + Path.DirectorySeparatorChar + "Frames" + Path.DirectorySeparatorChar + ID + ".json"))
        {
            string jsonstuff = File.ReadAllText(path + Path.DirectorySeparatorChar + "Frames" + Path.DirectorySeparatorChar + ID + ".json");
            frame = JsonUtility.FromJson<Frame>(jsonstuff);

            if (frame.Version == "0.0.1")
            {
                frameimageid.GetComponent<TMP_InputField>().text = frame.image;
                frametime.GetComponent<TMP_InputField>().text = frame.seconds.ToString();
                invincible.GetComponent<Toggle>().isOn = frame.invincible;
                cancellable.GetComponent<Toggle>().isOn = frame.cancancel;
                gravity.GetComponent<Toggle>().isOn = frame.hasgravity;
                pxmotion.GetComponent<TMP_InputField>().text = frame.movex.ToString();
                pymotion.GetComponent<TMP_InputField>().text = frame.movey.ToString();
                dxpos.GetComponent<TMP_InputField>().text = frame.dmgxoffset.ToString();
                dypos.GetComponent<TMP_InputField>().text = frame.dmgyoffset.ToString();
                drad.GetComponent<TMP_InputField>().text = frame.dmgradius.ToString();
                prxmotion.GetComponent<TMP_InputField>().text = frame.projectilevectorx.ToString();
                prymotion.GetComponent<TMP_InputField>().text = frame.projectilevectory.ToString();
                prtime.GetComponent<TMP_InputField>().text = frame.projectilelife.ToString();
                primageid.GetComponent<TMP_InputField>().text = frame.projectileimage;
                soundid.GetComponent<TMP_InputField>().text = frame.sound;
                nextframe.GetComponent<TMP_InputField>().text = frame.nextset;
            }
        }
        else
        {
            loadFrameButton();
        }
    }

    public void loadSetJSON(GameObject loader)
    {
        setID = loader.GetComponent<SetProperties>().SetID;
        setDisplayID.text = "Frame Set " + setID;
        string path = OpenTemplate.carryover;
        setMenu.SetActive(true);
        if (File.Exists(path + Path.DirectorySeparatorChar + "Sets" + Path.DirectorySeparatorChar + setID + ".json"))
        {
            string jsonstuff = File.ReadAllText(path + Path.DirectorySeparatorChar + "Sets" + Path.DirectorySeparatorChar + setID + ".json");
            Set = JsonUtility.FromJson<AnimSet>(jsonstuff);

            if (Set.Version == "0.0.1")
            {
                //generate input fields for frames here
                idle.GetComponent<Toggle>().isOn = Set.idle;
                ground.GetComponent<Toggle>().isOn = Set.whenGrounded;
                up.GetComponent<Toggle>().isOn = Set.up;
                down.GetComponent<Toggle>().isOn = Set.down;
                forward.GetComponent<Toggle>().isOn = Set.forward;
                backward.GetComponent<Toggle>().isOn = Set.backward;
                attack.GetComponent<Toggle>().isOn = Set.attack;
                special.GetComponent<Toggle>().isOn = Set.special;
                taunt.GetComponent<Toggle>().isOn = Set.taunt;
                jump.GetComponent<Toggle>().isOn = Set.jump;
                super.GetComponent<Toggle>().isOn = Set.superspecial;
                exact.GetComponent<Toggle>().isOn = Set.exactInput;
                loadFrameInputs();
            }
        }
        else
        {
            loadSetButton();
        }
    }

    public void saveFrameJSON()
    {
        //Set to default first just in case there's a null
        frame.FrameID = 0;
        frame.image = "000";
        frame.seconds = 0.5f;
        frame.invincible = false;
        frame.cancancel = true;
        frame.hasgravity = true;
        frame.movex = 0f;
        frame.dmgxoffset = 0f;
        frame.dmgyoffset = 0f;
        frame.dmgradius = 0f;
        frame.projectilevectorx = 0f;
        frame.projectilevectory = 0f;
        frame.projectilelife = 0f;
        frame.projectileimage = null;
        frame.sound = null;
        frame.nextset = null;

        //All must be null checked except for ID, booleans, projectile image id, sound id, and next set, because they have default values
        frame.FrameID = int.Parse(ID);
        if (frameimageid.GetComponent<TMP_InputField>().text != "")
        {
            frame.image = frameimageid.GetComponent<TMP_InputField>().text;
        }
        if (frametime.GetComponent<TMP_InputField>().text != "")
        {
            frame.seconds = float.Parse(frametime.GetComponent<TMP_InputField>().text);
        }

        frame.invincible = invincible.GetComponent<Toggle>().isOn;
        frame.cancancel = cancellable.GetComponent<Toggle>().isOn;
        frame.hasgravity = gravity.GetComponent<Toggle>().isOn;

        if (pxmotion.GetComponent<TMP_InputField>().text != "")
        {
            frame.movex = float.Parse(pxmotion.GetComponent<TMP_InputField>().text);
        }
        if (pymotion.GetComponent<TMP_InputField>().text != "")
        {
            frame.movey = float.Parse(pymotion.GetComponent<TMP_InputField>().text);
        }
        if (dxpos.GetComponent<TMP_InputField>().text != "")
        {
            frame.dmgxoffset = float.Parse(dxpos.GetComponent<TMP_InputField>().text);
        }
        if (dypos.GetComponent<TMP_InputField>().text != "")
        {
            frame.dmgyoffset = float.Parse(dypos.GetComponent<TMP_InputField>().text);
        }
        if (drad.GetComponent<TMP_InputField>().text != "")
        {
            frame.dmgradius = float.Parse(drad.GetComponent<TMP_InputField>().text);
        }
        if (prxmotion.GetComponent<TMP_InputField>().text != "")
        {
            frame.projectilevectorx = float.Parse(prxmotion.GetComponent<TMP_InputField>().text);
        }
        if (prymotion.GetComponent<TMP_InputField>().text != "")
        {
            frame.projectilevectory = float.Parse(prymotion.GetComponent<TMP_InputField>().text);
        }
        if (prtime.GetComponent<TMP_InputField>().text != "")
        {
            frame.projectilelife = float.Parse(prtime.GetComponent<TMP_InputField>().text);
        }

        frame.projectileimage = primageid.GetComponent<TMP_InputField>().text;
        frame.sound = soundid.GetComponent<TMP_InputField>().text;
        frame.nextset = nextframe.GetComponent<TMP_InputField>().text;



        string Output = JsonUtility.ToJson(frame);
        File.WriteAllText(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Frames" + Path.DirectorySeparatorChar + ID + ".json", Output);
        clearFrameMenu();
    }

    public void saveSetJSON()
    {
        int i = 0;
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("FrameOrder"))
        {
            if (obj.GetComponent<TMP_InputField>().text != "")
                Set.frameIDs[i] = obj.GetComponent<TMP_InputField>().text;
            i++;
        }
        Set.SetID = int.Parse(setID);
        Set.idle = idle.GetComponent<Toggle>().isOn;
        Set.whenGrounded = ground.GetComponent<Toggle>().isOn;
        Set.up = up.GetComponent<Toggle>().isOn;
        Set.down = down.GetComponent<Toggle>().isOn;
        Set.forward = forward.GetComponent<Toggle>().isOn;
        Set.backward = backward.GetComponent<Toggle>().isOn;
        Set.attack = attack.GetComponent<Toggle>().isOn;
        Set.special = special.GetComponent<Toggle>().isOn;
        Set.taunt = taunt.GetComponent<Toggle>().isOn;
        Set.jump = jump.GetComponent<Toggle>().isOn;
        Set.superspecial = super.GetComponent<Toggle>().isOn;
        Set.exactInput = exact.GetComponent<Toggle>().isOn;

        string Output = JsonUtility.ToJson(Set);
        File.WriteAllText(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Sets" + Path.DirectorySeparatorChar + setID + ".json", Output);
        clearSetMenu();
    }

    public void clearFrameMenu()
    {
        frame.image = "000";
        frame.seconds = 0.5f;
        frame.invincible = false;
        frame.cancancel = true;
        frame.hasgravity = true;
        frame.movex = 0;
        frame.movey = 0;
        frame.dmgxoffset = 0;
        frame.dmgyoffset = 0;
        frame.dmgradius = 0;
        frame.projectilevectorx = 0;
        frame.projectilevectory = 0;
        frame.projectilelife = 0;
        frame.projectileimage = null;
        frame.sound = null;
        frame.nextset = null;

        frameimageid.GetComponent<TMP_InputField>().text = null;
        frametime.GetComponent<TMP_InputField>().text = null;
        invincible.GetComponent<Toggle>().isOn = false;
        cancellable.GetComponent<Toggle>().isOn = true;
        gravity.GetComponent<Toggle>().isOn = true;
        pxmotion.GetComponent<TMP_InputField>().text = null;
        pymotion.GetComponent<TMP_InputField>().text = null;
        dxpos.GetComponent<TMP_InputField>().text = null;
        dypos.GetComponent<TMP_InputField>().text = null;
        drad.GetComponent<TMP_InputField>().text = null;
        prxmotion.GetComponent<TMP_InputField>().text = null;
        prymotion.GetComponent<TMP_InputField>().text = null;
        prtime.GetComponent<TMP_InputField>().text = null;
        primageid.GetComponent<TMP_InputField>().text = null;
        soundid.GetComponent<TMP_InputField>().text = null;
        nextframe.GetComponent<TMP_InputField>().text = null;

        frameMenu.SetActive(false);
        loadFrameButton();
    }

    public void clearSetMenu()
    {
        idle.GetComponent<Toggle>().isOn = false;
        ground.GetComponent<Toggle>().isOn = false;
        up.GetComponent<Toggle>().isOn = false;
        down.GetComponent<Toggle>().isOn = false;
        forward.GetComponent<Toggle>().isOn = false;
        backward.GetComponent<Toggle>().isOn = false;
        attack.GetComponent<Toggle>().isOn = false;
        special.GetComponent<Toggle>().isOn = false;
        taunt.GetComponent<Toggle>().isOn = false;
        jump.GetComponent<Toggle>().isOn = false;
        super.GetComponent<Toggle>().isOn = false;
        exact.GetComponent<Toggle>().isOn = false;

        Set.frameIDs.Clear();
        foreach(GameObject i in GameObject.FindGameObjectsWithTag("FrameOrder"))
        {
            Destroy(i);
        }
        Set.SetID = 0;
        Set.idle = false;
        Set.whenGrounded = false;
        Set.up = false;
        Set.down = false;
        Set.forward = false;
        Set.backward = false;
        Set.attack = false;
        Set.special = false;
        Set.taunt = false;
        Set.jump = false;
        Set.superspecial = false;
        Set.exactInput = false;

        setMenu.SetActive(false);
        loadSetButton();
    }

    public void DeleteFrameJSON()
    {
        string path = OpenTemplate.carryover;
        if (File.Exists(path + Path.DirectorySeparatorChar + "Frames" + Path.DirectorySeparatorChar + ID + ".json"))
        {
            File.Delete(path + Path.DirectorySeparatorChar + "Frames" + Path.DirectorySeparatorChar + ID + ".json");
            Debug.Log("Deleted " + ID + ".json");
        }
        else
        {
            Debug.Log("Failed");
        }
        clearFrameMenu();
    }

    public void DeleteSetJSON()
    {
        string path = OpenTemplate.carryover;
        if (File.Exists(path + Path.DirectorySeparatorChar + "Sets" + Path.DirectorySeparatorChar + setID + ".json"))
        {
            File.Delete(path + Path.DirectorySeparatorChar + "Sets" + Path.DirectorySeparatorChar + setID + ".json");
            Debug.Log("Deleted " + setID + ".json");
        }
        else
        {
            Debug.Log("Failed");
        }
        clearSetMenu();
    }

    public void ImportAudio()
    {
        FileImporter.GetFile("audio");
        int i = 0;
        audioID = "000";
        while (File.Exists(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Sound" + Path.DirectorySeparatorChar + audioID + ".mp3"))
        {
            i++;
            audioID = (i).ToString("000");
        }
        File.Copy(FileImporter.LastResult, OpenTemplate.carryover + Path.DirectorySeparatorChar + "Sound" + Path.DirectorySeparatorChar + audioID + ".mp3");
        desc.IncludedSounds.Add(OpenTemplate.carryover + Path.DirectorySeparatorChar + "Sound" + Path.DirectorySeparatorChar + audioID + ".mp3");
    }
}
