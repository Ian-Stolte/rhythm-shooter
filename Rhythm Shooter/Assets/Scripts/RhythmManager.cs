using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmManager : MonoBehaviour
{
    private float rawBeat;
    public float beat;
    [SerializeField] private int timesRepeated;
    [SerializeField] private int currentSong;
    public Song[] songs;

    [SerializeField] private GameObject kickPrefab;
    [SerializeField] private GameObject snarePrefab;

    private AudioManager audio;

    void Start()
    {
        rawBeat = -4;
        audio = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        audio.Play("Song 1");
    }

    void FixedUpdate()
    {
        //Keep time
        if (currentSong < songs.Length)
        {
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

    void Update()
    {
        if (currentSong < songs.Length)
        {
            //Spawn Notes
            foreach (Note n in songs[currentSong].notes)
            {
                if (n.beat%8 == (beat+1.5f)%8 && !n.spawned && beat >= -0.5f)
                {
                    if (n.drumType == Note.drums.KICK)
                    {
                        GameObject obj = Instantiate(kickPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Notes").transform);
                        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(800, -460, 0);
                    }
                    else if (n.drumType == Note.drums.SNARE)
                    {
                        GameObject obj = Instantiate(snarePrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Notes").transform);
                        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(800, -360, 0);
                    }
                    n.spawned = true;
                    n.played = false; //TODO: figure out a good way to store which notes have been successfully played & reset on each repeat
                }
            }
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
