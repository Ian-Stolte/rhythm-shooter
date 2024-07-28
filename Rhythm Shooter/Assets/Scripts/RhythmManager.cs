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
    private List<Note> notes = new List<Note>();

    [SerializeField] private GameObject kickPrefab;
    [SerializeField] private GameObject snarePrefab;
    [SerializeField] private GameObject hiHatPrefab;
    [SerializeField] private GameObject levelClearedTxt;

    private AudioManager audio;

    void Start()
    {
        rawBeat = -4;
        audio = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        audio.Play("Song " + (songNum+1));
        CreateNotes(songs[songNum]);
    }

    void FixedUpdate()
    {
        //Keep time
        if (songNum < songs.Length)
        {
            rawBeat += Time.deltaTime * (songs[songNum].tempo / 60.0f);
            if (rawBeat > songs[songNum].length + 0.875f)
            {
                if (timesRepeated < songs[songNum].repeats-2)
                {
                    rawBeat -= songs[songNum].length;
                    timesRepeated++;
                    foreach (Note n in notes)
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
            foreach (Note n in notes)
            {
                if (n.beat%songs[songNum].length == (beat+1.5f)%songs[songNum].length && !n.spawned && beat >= -0.5f)
                {
                    if (n.drumType == Note.drums.KICK)
                    {
                        GameObject obj = Instantiate(kickPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Notes").transform);
                        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(836, -460, 0);
                    }
                    else if (n.drumType == Note.drums.SNARE)
                    {
                        GameObject obj = Instantiate(snarePrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Notes").transform);
                        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(836, -380, 0);
                    }
                    else if (n.drumType == Note.drums.HIHAT)
                    {
                        GameObject obj = Instantiate(hiHatPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Notes").transform);
                        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(836, -300, 0);
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

    private void CreateNotes(Song s)
    {   
        if (s.kickBeats.Length > 0)
            AddNotes(Note.drums.KICK, s.kickBeats);
        if (s.snareBeats.Length > 0)
            AddNotes(Note.drums.SNARE, s.snareBeats);
        if (s.hiHatBeats.Length > 0)
            AddNotes(Note.drums.HIHAT, s.hiHatBeats);
        foreach (Note n in notes)
        {
            Debug.Log(n.drumType + " : " + n.beat);
        }
    }

    private void AddNotes(Note.drums type, string inputArr)
    {
        string[] stringNumbers = inputArr.Split(',');

        int[] intNumbers = new int[stringNumbers.Length];
        for (int i = 0; i < stringNumbers.Length; i++)
        {
            notes.Add(new Note(type, float.Parse(stringNumbers[i])));
        }
    }
}
