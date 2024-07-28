using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Song
{
    public string name;
    public int tempo;
    public int beatsPerMeasure;
    public int length;
    public int repeats;
    //public Note[] notes;
    public string kickBeats;
    public string snareBeats;
    public string hiHatBeats;
}
