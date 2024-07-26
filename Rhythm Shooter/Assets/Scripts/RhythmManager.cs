using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    private float rawBeat;
    public int beat;
    public int measureNum = 1;
    public Song[] songs;
    private int currentSong;

    void Update()
    {
        rawBeat += Time.deltaTime*(songs[currentSong].tempo/60);
        if (rawBeat > songs[currentSong].beatsPerMeasure+1)
        {
            measureNum++;
            rawBeat -= songs[currentSong].beatsPerMeasure;
        }
        beat = (int)Mathf.Floor(rawBeat);
    }
}
