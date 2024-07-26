using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Song
{
    public int tempo;
    public int beatsPerMeasure;
    public int length;
    public int repeats;
    public Note[] notes;
    /*public int[] kickBeats;
    public int[] snareBeats;
    public int[] hiHatBeats;
    public string kickBeats;
    public string snareBeats;
    public string hiHatBeats;*/
}
