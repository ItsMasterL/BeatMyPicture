using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public string currentFrame;
    public bool showHitBoxes;
    public float iframes;
    public float frametimer;

    //json
    [SerializeField]
    public JSONManager.AnimSet[] Set;
    public JSONManager.Frame[] frame;
    public JSONManager.AudioAndDescriptions[] desc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    void LoadJson()
    {
        //TODO: get template from template.noedit and load all jsons into respective classes via foreach
    }
}
