using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmManager : MonoBehaviour
{
    private float rawBeat;
    public float beat;
    public int timesRepeated;
    public int songNum;
    public Song[] songs;

    [SerializeField] private GameObject kickPrefab;
    [SerializeField] private GameObject snarePrefab;
    [SerializeField] private GameObject levelClearedTxt;

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
        if (songNum < songs.Length)
        {
            rawBeat += Time.deltaTime * (songs[songNum].tempo / 60.0f);
            if (rawBeat > songs[songNum].length + 0.875f)
            {
                if (timesRepeated < songs[songNum].repeats)
                {
                    rawBeat -= songs[songNum].length;
                    timesRepeated++;
                    foreach (Note n in songs[songNum].notes)
                    {
                        n.spawned = false;
                    }

                }
                else
                {
                    songNum++;
                    timesRepeated = 0;
                    rawBeat = 0;
                    StartCoroutine(EndLevel());
                }
            }
            beat = Mathf.Round(4 * rawBeat) / 4;
        }
    }

    void Update()
    {
        if (songNum < songs.Length)
        {
            //Spawn Notes
            foreach (Note n in songs[songNum].notes)
            {
                if (n.beat%8 == (beat+1.5f)%8 && !n.spawned && beat >= -0.5f)
                {
                    if (n.drumType == Note.drums.KICK)
                    {
                        GameObject obj = Instantiate(kickPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Notes").transform);
                        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(830, -460, 0);
                    }
                    else if (n.drumType == Note.drums.SNARE)
                    {
                        GameObject obj = Instantiate(snarePrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Notes").transform);
                        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(830, -360, 0);
                    }
                    n.spawned = true;
                    n.played = false; //TODO: figure out a good way to store which notes have been successfully played & reset on each repeat
                }
            }
        }
    }

    private IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(1);
        for (float i = 0; i < 1; i += 0.01f)
        {
            levelClearedTxt.GetComponent<CanvasGroup>().alpha = i;
            yield return new WaitForSeconds(0.01f);
        }
        Time.timeScale = 0;
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
