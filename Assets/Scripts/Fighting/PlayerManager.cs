﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Linq;

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
    public bool P1;
    public int CPUType = 0; //CPU 0 - No AI || CPU 1 - Endless; Choose a random attacking move, then move forward and use move when nearby player ||
                            //CPU 2 - Vs; categorize moves by damage, cooldown, and range, and use accordingly || CPU -1 - Another player (multiplayer)
    public int CPULevel = 5;
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
    public float cputimer;

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
    [SerializeField]
    List<AudioClip> clips;
    [SerializeField]
    List<AudioClip> templateclips;
    AudioSource audioSource;
    SpriteRenderer sr;
    int currentSprite;

    //debug
    public GameObject hitbox;
    [SerializeField]
    List<CpuDetails> details;
    GameObject player1;
    [SerializeField]
    GameObject endscreen;
    float initialx = 1;
    float modx = 0;
    float lerpx = 0;
    float initialy = 0;
    float mody = 0;
    float lerpy = 0;
    float lerpr = 0;


    //loading
    int spritesLoaded;
    public GameObject scaleMatch;
    public GameObject projectile;
    Renderer scale;

    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("P1");
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        scale = GameObject.Find("Scale").GetComponent<Renderer>();
        LoadJson();
        LoadImages();
        LoadSound();
        Debug.Log(CharManager.P1Controls);
        if (CharManager.P1Controls == 0)
        {
            foreach (GameObject obj in buttons)
            {
                obj.SetActive(false);
            }
            joystick.SetActive(true);
            osc.SetActive(true);
        }
        else if (CharManager.P1Controls == 1)
        {
            foreach (GameObject obj in buttons)
            {
                obj.SetActive(true);
            }
            joystick.SetActive(false);
            osc.SetActive(true);
        }
        else
        {
            foreach (GameObject obj in buttons)
            {
                obj.SetActive(false);
            }
            joystick.SetActive(false);
            osc.SetActive(false);
        }


        CheckSets(false);

        if (!P1 && CPUType >= 0)
        {
            details = new List<CpuDetails>();
            foreach (JSONManager.AnimSet set in Set)
            {
                CpuDetails setdetails = new CpuDetails();
                foreach (string Frame in set.frameIDs)
                {
                    setdetails.damage += frame[int.Parse(Frame)].damage;
                    if (frame[int.Parse(Frame)].cancancel == false)
                        setdetails.lag += frame[int.Parse(Frame)].seconds;
                    setdetails.xrange += frame[int.Parse(Frame)].dmgxoffset * frame[int.Parse(Frame)].dmgradius; //theoretically gets the best damage/radius ratio for x axis
                    setdetails.yrange += frame[int.Parse(Frame)].dmgyoffset * frame[int.Parse(Frame)].dmgradius; //theoretically gets the best damage/radius ratio for y axis
                    setdetails.move += frame[int.Parse(Frame)].movex;
                    setdetails.jump += frame[int.Parse(Frame)].movey;
                    if (frame[int.Parse(Frame)].projectilelife > 0) setdetails.projectile = true;
                    setdetails.num = set.SetID;
                    details.Add(setdetails);
                }
            }
            Debug.Log("set details: " + details.Count);
        }
    }

    public class CpuDetails
    {
        public int num;
        public float damage;
        public float lag;
        public float xrange;
        public float yrange;
        public float move;
        public float jump;
        public bool projectile;
    }

    private void Awake()
    {
        input = new Controls();
    }
    #region inputs

    private void OnEnable()
    {
        if (P1)
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
        if (health <= 0)
        {
            endscreen.SetActive(true);
            foreach (PlayerManager pm in FindObjectsOfType<PlayerManager>())
            {
                pm.enabled = false;
            }
        }

        if (gameObject.transform.position.x < -8) transform.position = new Vector3(-7, transform.position.y, transform.position.z);
        if (gameObject.transform.position.x > 8) transform.position = new Vector3(7, transform.position.y, transform.position.z);
        if (gameObject.transform.position.y < -8) transform.position = new Vector3(transform.position.x, -1, transform.position.z);

        if (player1.gameObject != gameObject && gameObject.transform.position.x < player1.gameObject.transform.position.x)
        {
            facingRight = true;
            player1.GetComponent<PlayerManager>().facingRight = false;
        }
        if (player1.gameObject != gameObject && gameObject.transform.position.x > player1.gameObject.transform.position.x)
        {
            facingRight = false;
            player1.GetComponent<PlayerManager>().facingRight = true;
        }

        if (currentSprite > sprite.Length)
        {
            currentSprite -= sprite.Length;
        }

        if (spritesLoaded < sprite.Length)
        {
            sr.sprite = sprite[spritesLoaded];
            spritesLoaded++;

            //the scale fix
            Renderer renderer = GetComponent<Renderer>();
            float resizeX = scale.bounds.size.x / renderer.bounds.size.x;
            float resizeY = scale.bounds.size.y / renderer.bounds.size.y;
            float resizeZ = scale.bounds.size.z / renderer.bounds.size.z;

            resizeX *= transform.localScale.x;
            resizeY *= transform.localScale.y;
            resizeZ *= transform.localScale.z;

            transform.localScale = new Vector3(resizeX, resizeY, resizeZ);
            return;
        }


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
        {
            sr.sprite = sprite[currentSprite];

            //the scale fix
            Renderer renderer = GetComponent<Renderer>();
            float resizeX = scale.bounds.size.x / renderer.bounds.size.x;
            float resizeY = scale.bounds.size.y / renderer.bounds.size.y;
            float resizeZ = scale.bounds.size.z / renderer.bounds.size.z;

            resizeX *= transform.localScale.x;
            resizeY *= transform.localScale.y;
            resizeZ *= transform.localScale.z;

            if (!facingRight) resizeX = -Mathf.Abs(resizeX);
            if (facingRight) resizeX = Mathf.Abs(resizeX);

            initialx = resizeX;
            initialy = resizeY;

            transform.localScale = new Vector3(resizeX, resizeY, resizeZ);
        }

        modx = initialx * frame[currentFrame].xscale;
        mody = initialy * frame[currentFrame].yscale;

        GetComponent<Rigidbody2D>().gravityScale = 1;
        if (frame[currentFrame].hasgravity == false) GetComponent<Rigidbody2D>().gravityScale = 0;

        if (frame[currentFrame].instanttransform)
        {
            transform.localScale = new Vector3(modx, mody, transform.localScale.z);
            if (!facingRight) transform.localRotation = Quaternion.Euler(0, 0, frame[currentFrame].rotation);
            if (facingRight) transform.localRotation = Quaternion.Euler(0, 0, -frame[currentFrame].rotation);
        } else
        {
            transform.localScale = new Vector3(Mathf.Lerp(initialx, modx, lerpx), Mathf.Lerp(initialy, mody, lerpy), transform.localScale.z);
            if (!facingRight) transform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, frame[currentFrame].rotation, lerpr));
            if (facingRight) transform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, -frame[currentFrame].rotation, lerpr));
        }
        if (lerpx < frame[currentFrame].seconds)
        {
            lerpx += Time.deltaTime / frame[currentFrame].seconds;
        }
        if (lerpy < frame[currentFrame].seconds)
        {
            lerpy += Time.deltaTime / frame[currentFrame].seconds;
        }
        if (lerpr < frame[currentFrame].seconds)
        {
            lerpr += Time.deltaTime / frame[currentFrame].seconds;
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
                    CheckSets(false);
            }
            foreach (JSONManager.Frame frames in frame) 
            {
                if (matched == false)
                {
                    if (frames.FrameID.ToString("000") == active.frameIDs[currentFrame])
                    {
                        lerpr = 0;
                        lerpx = 0;
                        lerpy = 0;
                        currentSprite = int.Parse(frames.image);
                        frametimer = frames.seconds;
                        matched = true;
                        if (frames.damage != 0 && frames.projectilelife == 0)
                        {
                            currentDamage = Mathf.Abs(frames.damage);
                            hitbox.gameObject.SetActive(true);
                            hitbox.transform.parent = null;
                            if (debugMode)
                                hitbox.GetComponent<SpriteRenderer>().enabled = true;
                            else
                                hitbox.GetComponent<SpriteRenderer>().enabled = false;
                            if (facingRight)
                                hitbox.transform.localPosition = new Vector2(transform.position.x + frames.dmgxoffset, transform.position.y + frames.dmgyoffset);
                            else
                                hitbox.transform.localPosition = new Vector2(transform.position.x - frames.dmgxoffset, transform.position.y + frames.dmgyoffset);

                            hitbox.transform.localScale = new Vector3(frames.dmgradius, frames.dmgradius);
                            hitbox.transform.parent = gameObject.transform;
                        }
                        if (frames.projectilelife != 0)
                        {
                            currentDamage = Mathf.Abs(frames.damage);
                            if (facingRight)
                            {
                                GameObject obj = GameObject.Instantiate(projectile, transform);
                                obj.transform.parent = null;
                                obj.transform.localPosition = new Vector2(transform.position.x + frames.dmgxoffset, transform.position.y + frames.dmgyoffset);
                                obj.transform.localScale = new Vector3(frames.dmgradius, frames.dmgradius);
                                obj.GetComponent<Projectile>().lifetime = frames.projectilelife;
                                obj.GetComponent<Projectile>().damage = currentDamage;
                                obj.GetComponent<Projectile>().deltax = frames.projectilevectorx;
                                obj.GetComponent<Projectile>().deltay = frames.projectilevectory;
                                obj.GetComponent<SpriteRenderer>().sprite = sprite[int.Parse(frames.projectileimage)];
                            }
                            else
                            {
                                GameObject obj = GameObject.Instantiate(projectile, transform);
                                obj.transform.parent = null;
                                obj.transform.localPosition = new Vector2(transform.position.x - frames.dmgxoffset, transform.position.y + frames.dmgyoffset);
                                obj.transform.localScale = new Vector3(frames.dmgradius, frames.dmgradius);
                                obj.GetComponent<Projectile>().lifetime = frames.projectilelife;
                                obj.GetComponent<Projectile>().damage = currentDamage;
                                obj.GetComponent<Projectile>().deltax = -frames.projectilevectorx;
                                obj.GetComponent<Projectile>().deltay = frames.projectilevectory;
                                obj.GetComponent<SpriteRenderer>().sprite = sprite[int.Parse(frames.projectileimage)];
                                obj.GetComponent<SpriteRenderer>().flipX = true;
                            }
                        }
                        if (frames.sound != "")
                        {
                            if (int.Parse(frames.sound) < 100 && frames.sound.ToCharArray().Length < 3)
                            {
                                audioSource.clip = clips[int.Parse(frames.sound)];
                                audioSource.Play();
                            }
                            else if (frames.sound.ToCharArray().Length == 3)
                            {
                                audioSource.clip = templateclips[int.Parse(frames.sound)];
                                audioSource.Play();
                            }
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
            //if (hitbox != null)
            //{
            //    ContactFilter2D filter = new ContactFilter2D();
            //    filter.layerMask = LayerMask.GetMask("Player");
            //    Collider2D[] collider = new Collider2D[0];
            //    hitbox.GetComponent<CircleCollider2D>().OverlapCollider(filter, collider);
            //    foreach(Collider2D col in collider)
            //    {
            //        //if (col.gameObject != col.transform.parent.gameObject)
            //        {
            //            col.gameObject.GetComponent<PlayerManager>().TakeDamage(frametimer,currentDamage);
            //        }
            //    }
            //}
        }
        else
        {
            hitbox.gameObject.SetActive(false);
            frametimer = 0;
            currentFrame++;
        }

        if (iframes > 0)
        {
            iframes -= Time.deltaTime;
        } else
        {
            iframes = 0;
            hurt = false;
            sr.color = new Color(1, 1, 1, 1);
        }

        if (cputimer > 0 && CPUType == 2)
        {
            cputimer -= Time.deltaTime;
        }

        if (cputimer <= 0 && CPUType == 2)
        {
            int bestrange = 0;
            float maxrange = 0;

            int worstrange = 0;
            float minrange = Mathf.Infinity;

            int bestheight = 0;
            float maxheight = 0;

            int bestdamage = 0;
            float maxdamage = 0;

            int bestmove = 0;
            float maxmove = 0;

            int bestbackmove = 0;
            float maxbackmove = 0;

            int bestjump = 0;
            float maxjump = 0;

            int bestlag = 0;
            float leastlag = 0;
             
            foreach (CpuDetails det in details)
            {
                if (det.xrange > maxrange)
                {
                    maxrange = det.xrange;
                    bestrange = det.num;
                }
                if (det.xrange < minrange && det.xrange != 0)
                {
                    minrange = det.xrange;
                    worstrange = det.num;
                }
                if (det.yrange > maxheight)
                {
                    maxheight = det.yrange;
                    bestheight = det.num;
                }
                if (det.damage > maxdamage)
                {
                    maxdamage = det.damage;
                    bestdamage = det.num;
                }
                if (det.move > maxmove)
                {
                    maxmove = det.move;
                    bestmove = det.num;
                }
                if (det.move < maxbackmove)
                {
                    maxbackmove = det.move;
                    bestbackmove = det.num;
                }
                if (det.jump > maxjump)
                {
                    maxjump = det.jump;
                    bestjump = det.num;
                }
                if (-det.lag < leastlag)
                {
                    leastlag = -det.lag;
                    bestlag = det.num;
                }
            }
            leastlag = Mathf.Abs(leastlag);
            Debug.Log("range: " + bestrange + ";" + worstrange + ", height: " + bestheight + ", damage: " + bestdamage + ", move: " + bestmove + ";" + bestbackmove + ", jump: " + bestjump + ", lag: " + bestlag);
            //for now use best move per situation, in future mix up moves by sorting by different attributes and picking randomly from the top 3-5
            bool chosen = false;
            int best = 0;
            if (!chosen && Vector2.Distance(player1.transform.position, gameObject.transform.position) > bestrange) //prioritize getting in
            {
                int decider = Random.Range(0, 2);

                best = bestmove;
                if (decider == 0) best = bestjump;

                up = Set[best].up;
                down = Set[best].down;
                forward = Set[best].forward;
                backward = Set[best].backward;
                jump = Set[best].jump;
                atk = Set[best].attack;
                special = Set[best].special;
                taunt = Set[best].taunt;
                superspecial = Set[best].superspecial;
                chosen = true;
            }
            if (!chosen && Vector2.Distance(player1.transform.position, gameObject.transform.position) < bestrange && Vector2.Distance(player1.transform.position, gameObject.transform.position) > worstrange) //pick between best mobility and best range
            {
                int decider = Random.Range(0, 3);

                best = bestmove;
                if (decider == 0) best = bestrange;

                up = Set[best].up;
                down = Set[best].down;
                forward = Set[best].forward;
                backward = Set[best].backward;
                jump = Set[best].jump;
                atk = Set[best].attack;
                special = Set[best].special;
                taunt = Set[best].taunt;
                superspecial = Set[best].superspecial;
                chosen = true;
            }
            if (!chosen && Vector2.Distance(player1.transform.position, gameObject.transform.position) < bestrange && Vector2.Distance(player1.transform.position, gameObject.transform.position) < worstrange) //prioritize attack but sometimes retreat
            {
                int decider = Random.Range(0, 6);

                best = bestmove;
                if (decider <= 1) best = bestrange;
                if (decider >= 2 && decider <= 4) best = bestdamage;
                if (decider == 5) best = bestbackmove;

                up = Set[best].up;
                down = Set[best].down;
                forward = Set[best].forward;
                backward = Set[best].backward;
                jump = Set[best].jump;
                atk = Set[best].attack;
                special = Set[best].special;
                taunt = Set[best].taunt;
                superspecial = Set[best].superspecial;
                chosen = true;
            }

            cputimer = Random.Range(0, (5 - Mathf.Sqrt(CPULevel * 2.5f)));
        }
    }

    void LoadJson()
    {
        desc = null;
        match = "";
        if (P1 && CharManager.P1Fighter != null)
        {
            path = CharManager.P1Fighter;
        } else
        {   //Remove/change this when local multiplayer is involved
            string[] dir = Directory.GetDirectories(Application.persistentDataPath + Path.DirectorySeparatorChar + "Fighters");
            path = dir[Random.Range(0,dir.Length)];
        }
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
    }

    #region LoadSound
    void LoadSound()
    {
        if (Directory.GetFiles(path + Path.DirectorySeparatorChar + "Sounds").Length > 0)
        {
            GetAllAudioFromFolder(path + Path.DirectorySeparatorChar + "Sounds");
        }

        if (Directory.GetFiles(match + Path.DirectorySeparatorChar + "Sound").Length > 0)
        {
            GetAllTemplateAudioFromFolder(match + Path.DirectorySeparatorChar + "Sound");
        }
    }

    public void GetAllAudioFromFolder(string filepath)
    {
        clips.Clear();
        DirectoryInfo directoryInfo = new DirectoryInfo(filepath);
        FileInfo[] songFiles = directoryInfo.GetFiles("*.*");

        foreach (FileInfo songFile in songFiles)
        {
            StartCoroutine(ConvertFilesToAudioClip(songFile));
        }
    }
    public void GetAllTemplateAudioFromFolder(string filepath)
    {
        clips.Clear();
        DirectoryInfo directoryInfo = new DirectoryInfo(filepath);
        FileInfo[] songFiles = directoryInfo.GetFiles("*.*");

        foreach (FileInfo songFile in songFiles)
        {
            StartCoroutine(ConvertTemplateFilesToAudioClip(songFile));
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
            clips.Add(www.GetAudioClip(false, false));
        }
    }
    private IEnumerator ConvertTemplateFilesToAudioClip(FileInfo songFile)
    {
        if (songFile.Name.Contains("meta"))
            yield break;
        else
        {
            string songName = songFile.FullName.ToString();
            string url = string.Format("file://{0}", songName);
            WWW www = new WWW(url);
            yield return www;
            templateclips.Add(www.GetAudioClip(false, false));
        }
    }
    #endregion

    void CheckSets(bool checkcancel = true)
    {
        if (checkcancel == true)
        {
            if (frame[int.Parse(active.frameIDs[currentFrame])].cancancel == false) return;
        }
        frametimer = 0;
        sortedSets = new List<JSONManager.AnimSet>();
        int inputcount = 0b0000_0000;
        if (up) inputcount = inputcount | 0b0000_0001;
        if (down) inputcount = inputcount | 0b0000_0010;
        if (forward) inputcount = inputcount | 0b0000_0100;
        if (backward) inputcount = inputcount | 0b0000_1000;
        if (atk) inputcount = inputcount | 0b0001_0000;
        if (special) inputcount = inputcount | 0b0010_0000;
        if (taunt) inputcount = inputcount | 0b0100_0000;
        if (jump) inputcount = inputcount | 0b1000_0000;
        Debug.Log(inputcount);

        bool exactMatch = false;
        //Hurt code caused issue
        //if (!hurt)
        //{
            foreach (JSONManager.AnimSet set in Set)
            {
                if (set.whenGrounded == onGround)
                {
                    if (set.up == true) set.matches = set.matches | 0b0000_0001;
                    if (set.down == true) set.matches = set.matches | 0b0000_0010;
                    if (set.forward == true) set.matches = set.matches | 0b0000_0100;
                    if (set.backward == true) set.matches = set.matches | 0b0000_1000;
                    if (set.attack == true) set.matches = set.matches | 0b0001_0000;
                    if (set.special == true) set.matches = set.matches | 0b0010_0000;
                    if (set.taunt == true) set.matches = set.matches | 0b0100_0000;
                    if (set.jump == true) set.matches = set.matches | 0b1000_0000;
                    //if (set.superspecial == true && superspecial == true) set.matches++; ADD LATER

                    if (set.matches == inputcount && inputcount != 0) //works
                    {
                        Debug.Log("Exact");
                        sortedSets.Add(set);
                        exactMatch = true;
                    }
                }
            }
            if (exactMatch == false && inputcount != 0 && desc.randomOnNoMatch)
            {
                foreach (JSONManager.AnimSet set in Set) //TODO: add sorting logic? make it so highest takes priority?
                {
                    if ((inputcount & set.matches) != 0 && inputcount != 0 && set.whenGrounded == onGround)
                    {
                        Debug.Log("Includes input; " + set.matches);
                        sortedSets.Add(set);
                    }
                }
                foreach (JSONManager.AnimSet set in sortedSets)
                {
                    Debug.Log("Set " + set.SetID);
                }
            }


            if (inputcount == 0b0000_0000 && onGround)
            {
                foreach (JSONManager.AnimSet set in Set)
                {
                    set.matches = 0b0000_0000;
                    if (set.idle)
                    {
                        sortedSets.Add(set);
                    }
                }
                active = sortedSets[Random.Range(0, sortedSets.Count)];
                Debug.Log("Set " + active.SetID + " was chosen! Idle");
            }
            if (sortedSets.Count == 0 && desc.randomOnNoMatch)
            {
                foreach (JSONManager.AnimSet set in Set)
                {
                    sortedSets.Add(set);
                }
            }
            if (sortedSets.Count == 0)
            {
                sortedSets.Add(Set[0]);
            }
            active = sortedSets[Random.Range(0, sortedSets.Count)];
            Debug.Log("Set " + active.SetID + " was chosen! Exact match: " + exactMatch + " (" + inputcount + ")");
        }
        //else
        //{
        //    foreach (JSONManager.AnimSet set in Set)
        //    {
        //        if (set.hurt)
        //        {
        //            sortedSets.Add(set);
        //        }
        //    }

        //    if (sortedSets.Count == 0)
        //    {
        //        sortedSets.Add(Set[0]);
        //    }
        //    active = sortedSets[Random.Range(0, sortedSets.Count)];
        //    Debug.Log("Set " + active.SetID + " was chosen! Exact match: " + exactMatch + " (" + inputcount + ")");
        //}
    

    public void TakeDamage(float iframe, float dmg)
    {
        if (iframes == 0 && frame[int.Parse(active.frameIDs[currentFrame])].invincible == false)
        {
            iframes = iframe;
            health -= dmg;
            if (health < 0) health = 0;
            hurt = true;
            if (debugMode)
            {
                sr.color = new Color(1, 1, 1, 0.75f);
            }
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
