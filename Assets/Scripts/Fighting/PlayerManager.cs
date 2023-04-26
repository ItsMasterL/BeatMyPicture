using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    //controls
    public Controls input;
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public bool atk;
    public bool special;
    public bool taunt;
    public bool jump;

    private InputAction UP;
    private InputAction DOWN;
    private InputAction LEFT;
    private InputAction RIGHT;
    private InputAction ATK;
    private InputAction SPECIAL;
    private InputAction TAUNT;
    private InputAction JUMP;

    public GameObject joystick;
    public GameObject[] buttons;
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

        if (CharManager.P1Controls == 0)
        {
            foreach(GameObject obj in buttons)
            {
                obj.SetActive(true);
            }
            joystick.SetActive(false);
        } else if (CharManager.P1Controls == 1)
        {
            foreach (GameObject obj in buttons)
            {
                obj.SetActive(false);
            }
            joystick.SetActive(true);
        } else
        {
            foreach (GameObject obj in buttons)
            {
                obj.SetActive(false);
            }
            joystick.SetActive(false);
        }
    }

    private void Awake()
    {
        input = new Controls();
    }
    #region inputs
    private void OnEnable()
    {
        UP = input.Default.Up;
        UP.Enable();
        UP.performed += UP_performed;
        UP.canceled += UP_performed;
        DOWN = input.Default.Down;
        DOWN.Enable();
        DOWN.performed += DOWN_performed;
        DOWN.canceled += DOWN_performed;
        LEFT = input.Default.Left;
        LEFT.Enable();
        LEFT.performed += LEFT_performed;
        LEFT.canceled += LEFT_performed;
        RIGHT = input.Default.Right;
        RIGHT.Enable();
        RIGHT.performed += RIGHT_performed;
        RIGHT.canceled += RIGHT_performed;

        ATK = input.Default.Attack;
        ATK.Enable();
        ATK.performed += ATK_performed;
        ATK.canceled += ATK_performed;
        SPECIAL = input.Default.Special;
        SPECIAL.Enable();
        SPECIAL.performed += SPECIAL_performed;
        SPECIAL.canceled += SPECIAL_performed;
        TAUNT = input.Default.Taunt;
        TAUNT.Enable();
        TAUNT.performed += TAUNT_performed;
        TAUNT.canceled += TAUNT_performed;
        JUMP = input.Default.Jump;
        JUMP.Enable();
        JUMP.performed += JUMP_performed;
        JUMP.canceled += JUMP_performed;
    }

    private void JUMP_performed(InputAction.CallbackContext obj)
    {
        jump = obj.performed;
    }

    private void TAUNT_performed(InputAction.CallbackContext obj)
    {
        taunt = obj.performed;
    }

    private void SPECIAL_performed(InputAction.CallbackContext obj)
    {
        special = obj.performed;
    }

    private void ATK_performed(InputAction.CallbackContext obj)
    {
        atk = obj.performed;
    }

    private void RIGHT_performed(InputAction.CallbackContext obj)
    {
        right = obj.performed;
    }

    private void LEFT_performed(InputAction.CallbackContext obj)
    {
        left = obj.performed;
    }

    private void DOWN_performed(InputAction.CallbackContext obj)
    {
        down = obj.performed;
    }

    private void UP_performed(InputAction.CallbackContext obj)
    {
        up = obj.performed;
    }
    #endregion
    private void OnDisable()
    {
        UP.Disable();
        DOWN.Disable();
        LEFT.Disable();
        RIGHT.Disable();
        ATK.Disable();
        SPECIAL.Disable();
        TAUNT.Disable();
        JUMP.Disable();
    }

    // Update is called once per frame
    void Update()
    {
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

            sprite[i] = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height), new Vector2(0.5f, 0.5f), 100);
            i++;
        }
        sr.sprite = sprite[0];
    }
}
