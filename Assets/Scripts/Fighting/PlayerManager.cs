using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    #region controls
    public Controls input;
    public bool _up, _down, _left, _right, _atk, _special, _taunt, _jump, _superspecial;
    bool up
    {
        get { return _up; }
        set
        {
            _up = value;
            CheckSets();
        }
    }
    bool down
    {
        get { return _down; }
        set
        {
            _down = value;
            CheckSets();
        }
    }
    bool left
    {
        get { return _left; }
        set
        {
            _left = value;
            CheckSets();
        }
    }
    bool right
    {
        get { return _right; }
        set
        {
            _right = value;
            CheckSets();
        }
    }
    bool atk
    {
        get { return _atk; }
        set
        {
            _atk = value;
            CheckSets();
        }
    }
    bool special
    {
        get { return _special; }
        set
        {
            _special = value;
            CheckSets();
        }
    }
    bool taunt
    {
        get { return _taunt; }
        set
        {
            _taunt = value;
            CheckSets();
        }
    }
    bool jump
    {
        get { return _jump; }
        set
        {
            _jump = value;
            CheckSets();
        }
    }
    bool superspecial
    {
        get { return _superspecial; }
        set
        {
            _superspecial = value;
            CheckSets();
        }
    }

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
    public GameObject osc;
    #endregion
    //info
    public bool debugMode;
    public float health = 200f;
    public bool onGround;
    public bool landing;
    public bool hurt;
    public int currentFrame;
    public float currentDamage;
    public bool facingRight = true;
    public bool forward;
    public bool backward;
    public float iframes;
    public float frametimer;
    public float jumptimer;
    public float landtimer;

    //json
    [SerializeField]
    public JSONManager.AnimSet[] Set;
    public JSONManager.Frame[] frame;
    public JSONManager.AudioAndDescriptions desc;

    List<JSONManager.AnimSet> sortedSets;
    [SerializeField]
    JSONManager.AnimSet active;

    //files
    string path;
    string template;
    string match;
    Sprite[] sprite;
    SpriteRenderer sr;
    int currentSprite;

    //debug
    public GameObject hitbox;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        LoadJson();
        LoadImages();
        Debug.Log(CharManager.P1Controls);
        if (CharManager.P1Controls == 0)
        {
            foreach(GameObject obj in buttons)
            {
                obj.SetActive(false);
            }
            joystick.SetActive(true);
            osc.SetActive(true);
        } else if (CharManager.P1Controls == 1)
        {
            foreach (GameObject obj in buttons)
            {
                obj.SetActive(true);
            }
            joystick.SetActive(false);
            osc.SetActive(true);
        } else
        {
            foreach (GameObject obj in buttons)
            {
                obj.SetActive(false);
            }
            joystick.SetActive(false);
            osc.SetActive(false);
        }
        ForceCancelSet();
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
        if (obj.performed && facingRight) forward = true;
        else forward = false;
        if (obj.performed && !facingRight) backward = true;
        else backward = false;
        right = obj.performed;
    }

    private void LEFT_performed(InputAction.CallbackContext obj)
    {
        if (obj.performed && !facingRight) forward = true;
        else forward = false;
        if (obj.performed && facingRight) backward = true;
        else backward = false;
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

        if (facingRight)
        {
            sr.flipX = false;
        } else
        {
            sr.flipX = true;
        }

        if (active != null && frametimer == 0)
        {
            bool matched = false;
            bool matched2 = false;
            if (currentFrame >= active.frameIDs.Count)
            {
                foreach (JSONManager.AnimSet set in Set)
                {
                    if (set.SetID.ToString("000") == frame[int.Parse(active.frameIDs[active.frameIDs.Count - 1])].nextset)
                    {
                        active = set;
                        matched2 = true;
                    }
                }
                currentFrame = 0;
                if (matched2 == false)
                    ForceCancelSet();
            }
            foreach (JSONManager.Frame frames in frame) 
            {
                if (matched == false)
                {
                    if (frames.FrameID.ToString("000") == active.frameIDs[currentFrame])
                    {
                        currentSprite = int.Parse(frames.image);
                        frametimer = frames.seconds;
                        matched = true;
                        if (frames.damage != 0 && frames.projectilelife == 0)
                        {
                            currentDamage = frames.damage;
                            hitbox.gameObject.SetActive(true);
                            if (debugMode)
                                hitbox.GetComponent<SpriteRenderer>().enabled = true;
                            else
                                hitbox.GetComponent<SpriteRenderer>().enabled = false;

                            hitbox.transform.localPosition = new Vector2(frames.dmgxoffset, frames.dmgyoffset);
                            hitbox.transform.localScale = new Vector3(frames.dmgradius, frames.dmgradius);
                        }
                    }
                }
            }
        }

        if (frametimer > 0)
        {
            frametimer -= Time.deltaTime;
            if (frame[int.Parse(active.frameIDs[currentFrame])].movex != 0 && facingRight)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(frame[int.Parse(active.frameIDs[currentFrame])].movex, GetComponent<Rigidbody2D>().velocity.y);
            }
            if (frame[int.Parse(active.frameIDs[currentFrame])].movex != 0 && !facingRight)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-frame[int.Parse(active.frameIDs[currentFrame])].movex, GetComponent<Rigidbody2D>().velocity.y);
            }
            if (frame[int.Parse(active.frameIDs[currentFrame])].movey != 0)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, frame[int.Parse(active.frameIDs[currentFrame])].movey);
            }
            if (hitbox != null)
            {
                ContactFilter2D filter = new ContactFilter2D();
                filter.layerMask = LayerMask.GetMask("Player");
                Collider2D[] collider = new Collider2D[0];
                hitbox.GetComponent<CircleCollider2D>().OverlapCollider(filter, collider);
                foreach(Collider2D col in collider)
                {
                    if (col.gameObject != gameObject)
                    {
                        col.gameObject.GetComponent<PlayerManager>().TakeDamage(frametimer,currentDamage);
                    }
                }
            }
        }
        else
        {
            hitbox.gameObject.SetActive(false);
            frametimer = 0;
            currentFrame++;
        }
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
            //sortedSets.Add(Set[i]);
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

    void CheckSets()
    {
        if (frame[int.Parse(active.frameIDs[currentFrame])].cancancel)
        {
            frametimer = 0;
            sortedSets = new List<JSONManager.AnimSet>();
            int inputcount = 0;
            int matches = 0;
            if (up) inputcount++;
            if (down) inputcount++;
            if (forward) inputcount++;
            if (backward) inputcount++;
            if (atk) inputcount++;
            if (special) inputcount++;
            if (taunt) inputcount++;
            if (jump) inputcount++;
            foreach (JSONManager.AnimSet set in Set)
            {
                bool idleskip = false;
                set.matches = 0;
                if (set.idle)
                {
                    if (up == false && down == false && left == false && right == false
                        && atk == false && special == false && taunt == false && jump == false)
                    {
                        set.matches = 999999;
                    }
                    idleskip = true;
                }
                if (idleskip == false)
                {

                    if (set.whenGrounded == onGround)
                    {
                        if (set.up == true && up == true) set.matches++;
                        if (set.forward == true && forward == true) set.matches++;
                        if (set.backward == true && backward == true) set.matches++;
                        if (set.down == true && down == true) set.matches++;
                        if (set.attack == true && atk == true) set.matches++;
                        if (set.special == true && special == true) set.matches++;
                        if (set.taunt == true && taunt == true) set.matches++;
                        if (set.jump == true && jump == true) set.matches++;
                        if (set.superspecial == true && superspecial == true) set.matches++;

                        if (set.exactInput && set.matches == inputcount && inputcount != 0)
                        {
                            //TODO: Add binary counting system for checking (0000100 is gonna be equal to 0000100 and not 0010000)
                            Debug.Log("Exact match");
                            set.matches = 999999;
                        }
                    }
                }
                if (set.matches > matches)
                {
                    matches = set.matches;
                    Debug.Log(set.SetID);
                }
            }
            if (matches == 0 && !desc.randomOnNoMatch) return;
            foreach (JSONManager.AnimSet set in Set)
            {
                if (set.matches == matches) sortedSets.Add(set);
            }
            active = sortedSets[Random.Range(0, sortedSets.Count)];
            Debug.Log("Set " + active.SetID + " was chosen with " + matches + " matches!");
        }
    }

    void ForceCancelSet()
    {
        frametimer = 0;
        sortedSets = new List<JSONManager.AnimSet>();
        int inputcount = 0;
        int matches = 0;
        if (up) inputcount++;
        if (down) inputcount++;
        if (forward) inputcount++;
        if (backward) inputcount++;
        if (atk) inputcount++;
        if (special) inputcount++;
        if (taunt) inputcount++;
        if (jump) inputcount++;
        foreach (JSONManager.AnimSet set in Set)
        {
            bool idleskip = false;
            set.matches = 0;
            if (set.idle)
            {
                if (up == false && down == false && left == false && right == false
                    && atk == false && special == false && taunt == false && jump == false)
                {
                    set.matches = 999999;
                }
                idleskip = true;
            }
            if (idleskip == false)
            {

                if (set.whenGrounded == onGround)
                {
                    if (set.up == true && up == true) set.matches++;
                    if (set.forward == true && forward == true) set.matches++;
                    if (set.backward == true && backward == true) set.matches++;
                    if (set.down == true && down == true) set.matches++;
                    if (set.attack == true && atk == true) set.matches++;
                    if (set.special == true && special == true) set.matches++;
                    if (set.taunt == true && taunt == true) set.matches++;
                    if (set.jump == true && jump == true) set.matches++;
                    if (set.superspecial == true && superspecial == true) set.matches++;

                    if (set.exactInput && set.matches == inputcount && inputcount != 0)
                    {
                        //TODO: Add binary counting system for checking (0000100 is gonna be equal to 0000100 and not 0010000)
                        Debug.Log("Exact match");
                        set.matches = 999999;
                    }
                }
            }
            if (set.matches > matches)
            {
                matches = set.matches;
                Debug.Log(set.SetID);
            }
        }
        if (matches == 0 && !desc.randomOnNoMatch) return;
        foreach (JSONManager.AnimSet set in Set)
        {
            if (set.matches == matches) sortedSets.Add(set);
        }
        active = sortedSets[Random.Range(0, sortedSets.Count)];
        Debug.Log("Set " + active.SetID + " was chosen with " + matches + " matches!");
    }

    void TakeDamage(float iframe, float dmg)
    {
        if (iframes == 0)
        {
            iframes = iframe;
            health -= dmg;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
            onGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
            onGround = false;
    }
}
