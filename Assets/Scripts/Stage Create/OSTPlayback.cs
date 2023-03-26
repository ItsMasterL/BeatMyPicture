using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OSTPlayback : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> songs;
    [SerializeField]
    TextMeshProUGUI display;
    AudioSource sound;

    public int songChoice;

    float startpos;
    float looppos;
    float endpos;

    float backby;

    public List<string> songnames;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        sound.Stop();
        sound.time = 0;
    }

    public void PlayPause()
    {
        if (sound.isPlaying)
        {
            sound.Pause();
            sound.time = 0;
        } else
        {
            switch (songChoice)
            {
                default:
                    startpos = 0f;
                    looppos = 0f;
                    endpos = 67.265f;
                    break;

                case 1:
                    startpos = 0f;
                    looppos = 3.256f;
                    endpos = 60.856f;
                    break;

                case 2:
                    startpos = 0f;
                    looppos = 18.064f;
                    endpos = 74.063f;
                    break;

                case 3:
                    startpos = 0f;
                    looppos = 48.060f;
                    endpos = 91.261f;
                    break;
            }

            backby = endpos - looppos;
            sound.clip = songs[songChoice];
            sound.Play();
        }
    }

    public void SongChange(bool add)
    {
        if (add && songChoice < 3)
        {
            songChoice++;
        }
        else if (add)
        {
            songChoice = 0;
        }
        else if (songChoice > 0)
        {
            songChoice--;
        }
        else
        {
            songChoice = 3;
        }

        display.text = "Track " + songChoice.ToString("000") + "\n\n\n\n" + songnames[songChoice];
    }

    public void SetSong(int song)
    {
        songChoice = song;
        display.text = "Track " + songChoice.ToString("000") + "\n\n\n\n" + songnames[songChoice];
    }

    private void Update()
    {
        if (sound.isPlaying && sound.time >= endpos)
        {
            sound.time -= backby;
        }
    }
}
