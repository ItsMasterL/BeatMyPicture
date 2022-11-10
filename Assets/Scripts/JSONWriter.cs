using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONWriter : MonoBehaviour
{
    [System.Serializable]
    public class Frame
    {
        public string Version = "0.0.1"; //Not in editor
        public int FrameID = 0; //Not editable
        public string image = "000"; //Coresponds with cutout. Default to 000 if not found
        public float seconds = 0.5f; //Default to 0.5f if <= 0
        public float movex = 0; //pos is forward, neg is backward
        public float movey = 0; //pos is up, neg is down
        public bool invincible = false; //This can be abused. Try not to though.
        public float damage = 0; //absolute value
        public float dmgxoffset = 0; //pos is forward, neg is backward
        public float dmgyoffset = 0; //pos is up, neg is down
        public float dmgradius = 0; //every hitbox is circles
        public float projectilelife = 0; //0 means no projectile
        public float projectilevectorx = 0; //pos is forward, neg is backward
        public float projectilevectory = 0; //pos is up, neg is down
        public string projectileimage = null; //default to 000. Hey, I just shot myself! No, not like that, i mean, I came out of my gun
        public bool cancancel = true; //If false, inputs are ignored while frame is active. Prevents turbo mode
        public bool hasgravity = true; //If false, gravity is tempoararily disabled during this frame, useful for precise control of character
        public string sound = null; //mp3 or wav. Hard length limit of probably 5 seconds, but unlimited layering unless theres an issue
        public string nextset = null; //Intended use for frame at end of set, to loop animations or easily use shared animations (Can softlock if cancancel is false on all frames!!)
    }

    [System.Serializable]
    public class AnimSet
    {
        public string SetName;
        public int SetID;
        public Frame[] frame;
    }
    
    [System.Serializable]
    public class CharacterAnimations //The main one
    {
        public AnimSet[] animSets;
    }

    public CharacterAnimations Anim = new CharacterAnimations();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            saveJSON();
            Debug.Log("Saved");
        }
    }

    public void saveJSON()
    {
        string Output = JsonUtility.ToJson(Anim);

        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Custom" + Path.DirectorySeparatorChar + "anim.json", Output);
    }
}
