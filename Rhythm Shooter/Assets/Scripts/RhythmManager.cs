using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    private float rawBeat;
    public float beat;
    [SerializeField] private int timesRepeated;
    [SerializeField] private int currentSong;
    public Song[] songs;
    //private List<Note> notes = new List<Note>();

    [SerializeField] private GameObject notePrefab;

    void Start()
    {
        beat = -1;
        //CreateNotes(songs[0]);
    }

    void Update()
    {
        if (currentSong < songs.Length)
        {
            //Spawn Notes
            foreach (Note n in songs[currentSong].notes)
            {
                if (n.beat == beat + 1.5f && !n.spawned)
                {
                    GameObject obj = Instantiate(notePrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Notes").transform);
                    if (n.drumType == Note.drums.KICK)
                    {
                        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(250, -130, 0);
                    }
                    else if (n.drumType == Note.drums.SNARE)
                    {
                        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(250, -80, 0);
                    }
                    n.spawned = true;
                    n.played = false; //TODO: figure out a good way to store which notes have been successfully played & reset on each repeat
                }
            }

            //Keep time
            rawBeat += Time.deltaTime * (songs[currentSong].tempo / 60.0f);
            if (rawBeat > songs[currentSong].length + 0.875f)
            {
                if (timesRepeated < songs[currentSong].repeats)
                {
                    rawBeat -= songs[currentSong].length;
                    timesRepeated++;
                    foreach (Note n in songs[currentSong].notes)
                    {
                        n.spawned = false;
                    }

                }
                else
                {
                    currentSong++;
                    timesRepeated = 0;
                    rawBeat = 0;
                }
            }
            beat = Mathf.Round(4 * rawBeat) / 4;
        }
    }

    /*private void CreateNotes(Song s)
    {
        int lastComma = 0;
        for (int i = 0; i < s.kickBeats.Length; i++)
        {
            Debug.Log(i + " : " + s.kickBeats[i]);
            if (s.kickBeats[i] == ',')
            {
                Debug.Log("MATCH! " + lastComma + "-" + (i-1) + " = " + s.kickBeats.Substring(lastComma, i-1));
                Note n = new Note(Note.drums.KICK, float.Parse("1.0"));
                kicks.Add(n);
                lastComma = i+1;
            }
        }
        foreach (Note n in notes)
        {
            Debug.Log(n.drumType + " : " + n.beat);
        }
    }*/
}
