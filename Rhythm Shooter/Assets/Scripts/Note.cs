using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Note
{
    public enum drums
    {
        KICK,
        SNARE,
        HIHAT
    }
    public drums drumType;
    public float beat;
    [HideInInspector] public bool played;
    [HideInInspector] public bool spawned;

    public Note(drums drumType, float beat)
    {
        this.drumType = drumType;
        this.beat = beat;
    }
}
