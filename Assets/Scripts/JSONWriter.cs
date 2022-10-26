using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONWriter : MonoBehaviour
{
    [System.Serializable]
    public class Frame
    {
        public int FrameID = 0;
        public string image = "000";
        public float seconds = 0.5f;
        public float movex = 0;
        public float movey = 0;
        public bool invincible = false;
        public float damage = 0;
        public float dmgxoffset = 0;
        public float dmgyoffset = 0;
        public float dmgradius = 0;
        public float projectilelife = 0;
        public float projectilevectorx = 0;
        public float projectilevectory = 0;
        public string projectileimage = null;
        public bool cancancel = true;
        public bool hasgravity = true;
        public string sound = null;
        public string nextset = null;
    }

    [System.Serializable]
    public class AnimSet
    {
        public string SetName;
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
