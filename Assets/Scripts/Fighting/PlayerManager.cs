using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    //controls
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public bool atk;
    public bool special;
    public bool taunt;
    public bool jump;
    //info
    public float health = 200f;
    public bool onGround;
    public string currentFrame;
    public bool debugMode;
    public float iframes;
    public float frametimer;
    public float jumptimer;
    public float landtimer;

    //json
    [SerializeField]
    public JSONManager.AnimSet[] Set;
    public JSONManager.Frame[] frame;
    public JSONManager.AudioAndDescriptions desc;

    //files
    string path;
    string template;
    string match;
    Sprite[] sprite;
    SpriteRenderer sr;
    int currentSprite;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        LoadJson();
        LoadImages();
    }

    // Update is called once per frame
    void Update()
    {
        #region inputs
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            up = true;
        } else
        {
            up = false;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            down = true;
        } else
        {
            down = false;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            left = true;
        } else
        {
            left = false;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            right = true;
        } else
        {
            right = false;
        }

        if (Input.GetKey(KeyCode.O) || Input.GetKey(KeyCode.Z))
        {
            atk = true;
        } else
        {
            atk = false;
        }

        if (Input.GetKey(KeyCode.P) || Input.GetKey(KeyCode.X))
        {
            special = true;
        } else
        {
            special = false;
        }

        if (Input.GetKey(KeyCode.U) || Input.GetKey(KeyCode.C))
        {
            taunt = true;
        } else
        {
            taunt = false;
        }

        if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.V))
        {
            jump = true;
        } else
        {
            jump = false;
        }
        #endregion

        #region debug
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (currentSprite < sprite.Length - 1)
                {
                    currentSprite++;
                }
                else
                {
                    currentSprite = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (currentSprite > 0)
                {
                    currentSprite--;
                }
                else
                {
                    currentSprite = sprite.Length - 1;
                }
            }
        }
        #endregion

        if (sr.sprite != sprite[currentSprite])
            sr.sprite = sprite[currentSprite];
    }

    void LoadJson()
    {
        desc = null;
        match = "";
        path = CharManager.P1Fighter;
        template = File.ReadAllText(path + Path.DirectorySeparatorChar + "template.noedit");

        foreach(string dir in Directory.GetDirectories(Application.persistentDataPath + Path.DirectorySeparatorChar + "Templates"))
        {
            if (match == "")
            {
                match = File.ReadAllText(dir + Path.DirectorySeparatorChar + "properties.json");
                desc = JsonUtility.FromJson<JSONManager.AudioAndDescriptions>(match);
                if (desc.UUID == template)
                {
                    match = dir;
                }
                else
                {
                    match = "";
                    desc = null;
                }
            }
        }
        if (match == "")
        {
            Debug.LogError("!!Template could not be found!!");
            return;
        }

        frame = new JSONManager.Frame[Directory.GetFiles(match + Path.DirectorySeparatorChar + "Frames").Length];
        Set = new JSONManager.AnimSet[Directory.GetFiles(match + Path.DirectorySeparatorChar + "Sets").Length];
        int i = 0;
        foreach (string file in Directory.GetFiles(match + Path.DirectorySeparatorChar + "Frames"))
        {
            string load = File.ReadAllText(file);
            frame[i] = JsonUtility.FromJson<JSONManager.Frame>(load);
            i++;
        }
        i = 0;
        foreach (string file in Directory.GetFiles(match + Path.DirectorySeparatorChar + "Sets"))
        {
            string load = File.ReadAllText(file);
            Set[i] = JsonUtility.FromJson<JSONManager.AnimSet>(load);
            i++;
        }
    }

    void LoadImages()
    {
        sprite = new Sprite[Directory.GetFiles(path + Path.DirectorySeparatorChar + "Pictures").Length];
        int i = 0;
        foreach(string file in Directory.GetFiles(path + Path.DirectorySeparatorChar + "Pictures"))
        {
            Texture2D tex = null;
            byte[] fileData;

            fileData = File.ReadAllBytes(file);
            tex = new Texture2D(2,2);
            //tex.filterMode = FilterMode.Point;
            tex.LoadImage(fileData);

            sprite[i] = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height), new Vector2(0.5f, 0.5f));
            i++;
        }
        sr.sprite = sprite[0];
    }
}
