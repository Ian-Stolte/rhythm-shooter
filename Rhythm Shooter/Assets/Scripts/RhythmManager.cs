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

    private bool endLevel;
    public bool doingKick;
    public bool doingSnare;
    public bool doingHiHat;

    [SerializeField] private GameObject kickPrefab;
    [SerializeField] private GameObject snarePrefab;
    [SerializeField] private GameObject hiHatPrefab;
    [SerializeField] private GameObject levelCleared;
    [SerializeField] private GameObject gameOver;

    private AudioManager audio;
    private PlayerController player;

    void Start()
    {
        audio = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        StartSong();
    }

    public void StartSong()
    {
        StartCoroutine(Fade(levelCleared, true));
        StartCoroutine(Fade(gameOver, true));
        foreach (Transform child in GameObject.Find("Enemies").transform)
        {
            Destroy(child.gameObject);
        }
        player.health = player.maxHealth;
        GameObject.Find("HP Bar").GetComponent<Image>().fillAmount = 1;
        player.paused = false;
        timesRepeated = 0;
        rawBeat = -4;
        audio.Play(songs[songNum].name);
        CreateNotes(songs[songNum]);
    }

    void FixedUpdate()
    {
        //Keep time
        if (songNum < songs.Length && !player.paused)
        {
            rawBeat += Time.deltaTime * (songs[songNum].tempo / 60.0f);
            if (rawBeat > songs[songNum].length - 0.125f)
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
                else if (!endLevel)
                {
                    endLevel = true;
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
                if (n.beat%songs[songNum].length == (beat+1)%songs[songNum].length && !n.spawned && beat >= 0)
                {
                    if (n.drumType == Note.drums.KICK && doingKick)
                    {
                        GameObject obj = Instantiate(kickPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Notes").transform);
                        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(800, -460, 0);
                    }
                    else if (n.drumType == Note.drums.SNARE && doingSnare)
                    {
                        GameObject obj = Instantiate(snarePrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Notes").transform);
                        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(800, -380, 0);
                    }
                    else if (n.drumType == Note.drums.HIHAT && doingHiHat)
                    {
                        GameObject obj = Instantiate(hiHatPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Notes").transform);
                        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(800, -300, 0);
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
        player.paused = true;
        StartCoroutine(Fade(levelCleared));
        yield return new WaitForSeconds(1);
        StartCoroutine(Fade(levelCleared.transform.GetChild(2).gameObject));
    }

    public IEnumerator Fade(GameObject g, bool fadingOut = false) //could add duration
    {
        if (!fadingOut)
            g.SetActive(true);
        float start = g.GetComponent<CanvasGroup>().alpha;
        for (float i = 0; i < 1; i += 0.01f)
        {
            if (fadingOut)
                g.GetComponent<CanvasGroup>().alpha = start*(1-i);
            else
                g.GetComponent<CanvasGroup>().alpha = start + (1-start)*i;
            yield return new WaitForSeconds(0.01f);
        }
        if (fadingOut)
            g.SetActive(false);
    }

    private void CreateNotes(Song s)
    {   
        if (s.kickBeats.Length > 0)
            AddNotes(Note.drums.KICK, s.kickBeats);
        if (s.snareBeats.Length > 0)
            AddNotes(Note.drums.SNARE, s.snareBeats);
        if (s.hiHatBeats.Length > 0)
            AddNotes(Note.drums.HIHAT, s.hiHatBeats);
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
