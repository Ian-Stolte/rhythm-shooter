using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    private float rawBeat;
    public int beat;
    public int measureNum = 1;
    [SerializeField] private int tempo;
    [SerializeField] private int beatsPerMeasure;

    void Update()
    {
        rawBeat += Time.deltaTime*(tempo/60);
        if (rawBeat > beatsPerMeasure+1)
        {
            measureNum++;
            rawBeat -= beatsPerMeasure;
        }
        beat = (int)Mathf.Floor(rawBeat);
    }
}
