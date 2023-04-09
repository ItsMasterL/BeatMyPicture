using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System.IO;
using TMPro;

public class RecordAudio : MonoBehaviour
{
    float countdown;
    float duration;
    float cap;
    bool isRecording;
    public int currentSound;

    AudioClip rec;
    public GameObject templateInfo;
    AudioSource source;

    public TextMeshProUGUI countDisp;
    public Slider slider;
    public List<GameObject> hidewhilerecord;

    public GameObject hidewhendone;
    public GameObject endscreen;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (source.isPlaying)
        {
            slider.value = source.time;
            if (source.time > cap)
            {
                source.Stop();
                source.time = 0;
            }
        }
        else if (isRecording)
        {
            slider.value = duration;
        }
        else
        {
            slider.value = 0;
        }

        if (isRecording)
        {
            if (countdown > 0)
            {
                foreach (GameObject obj in hidewhilerecord)
                {
                    obj.SetActive(false);
                }
                countdown -= Time.deltaTime;
                countDisp.text = Mathf.Ceil(countdown).ToString("0");
            }
            else if (!Microphone.IsRecording(null))
            {
                rec = Microphone.Start(null, false, Mathf.CeilToInt(cap), 44100);
            }

            if (countdown <= 0 && Microphone.IsRecording(null) && duration <= cap)
            {
                duration += Time.deltaTime;
            }
            else if (Microphone.IsRecording(null))
            {
                Microphone.End(null);
                SavWav.Save(OpenFighter.carryover + Path.DirectorySeparatorChar + "Sounds" + Path.DirectorySeparatorChar + currentSound.ToString() + ".wav", rec);
                source.clip = rec;
                source.Play();
                countDisp.gameObject.SetActive(false);
                foreach (GameObject obj in hidewhilerecord)
                {
                    obj.SetActive(true);
                }
                isRecording = false;
            }
        }
    }

    public void Setup()
    {
        source.Stop();
        source.time = 0;
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        Debug.Log(Microphone.devices);

        cap = templateInfo.GetComponent<LoadInfoFromTemplates>().set.SoundLimits[currentSound];
        rec = null;

        slider.maxValue = cap;

        countdown = 3f;
        duration = 0f;
        countDisp.gameObject.SetActive(true);
        isRecording = true;
    }

    public void Load(int index)
    {
        GetComponent<AudioLoader>().GetSpecificAudioFromFolder(OpenFighter.carryover + Path.DirectorySeparatorChar + "Sounds" + Path.DirectorySeparatorChar, currentSound.ToString());
    }

    public void Replay()
    {
        source.Stop();
        source.time = 0;
        source.Play();
    }

    public void OK()
    {
        if (currentSound < templateInfo.GetComponent<LoadInfoFromTemplates>().set.SoundIDs.Count - 1)
        {
            currentSound++;
            hidewhendone.GetComponent<GetRecInfo>().UpdateInfo();
            gameObject.SetActive(false);
        }
        else
        {
            endscreen.SetActive(true);
            hidewhendone.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
