using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    //Based on the "Conductor" pattern by Graham Tattersall : https://www.gamasutra.com/blogs/GrahamTattersall/20190515/342454/Coding_to_the_Beat__Under_the_Hood_of_a_Rhythm_Game_in_Unity.php
    //Each 4 Beat bar takes 2.645 seconds. A Beat occurs every 0.66125 seconds. The Range of being 'acceptably close to the beat' is 0.16
    public float songBPM;
    public float firstBeatOffset;
    float secPerBeat;
    float songPostionInBeats;
    float hitRange = 0.1653F;
    float dspSongTime;
    public AudioSource musicScore;
    float currentTime;
    float currentBeatTime;

    // Start is called before the first frame update
    void Start()
    {
        musicScore = GetComponent<AudioSource>();

        secPerBeat = 60f / songBPM;

        dspSongTime = (float)AudioSettings.dspTime;

        musicScore.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //"Heart Beat" Code
        currentTime = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        songPostionInBeats = currentTime / secPerBeat;

        currentBeatTime = currentTime % secPerBeat;
    }

    //Called when a part of the game wants to know if an event is on beat.
    public bool CheckBeat()
    {
        if(currentBeatTime < hitRange || currentBeatTime > secPerBeat - (hitRange))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetCurrentBeatTime()
    {
        return currentBeatTime;
    }
}
