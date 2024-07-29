using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmManager : MonoBehaviour
{
    public float rawBeat;
    public float beat;
    public int timesRepeated;
    public int songNum;
    public Song[] songs;
    private List<Note> notes = new List<Note>();

    private bool endLevel;
    private bool resetNotes;
    public bool doingKick;
    public bool doingSnare;
    public bool doingHiHat;

    [SerializeField] private GameObject kickPrefab;
    [SerializeField] private GameObject snarePrefab;
    [SerializeField] private GameObject hiHatPrefab;
    [SerializeField] private GameObject levelCleared;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject mainMenu;

    private AudioManager audio;
    private PlayerController player;

    void Start()
    {
        audio = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void SetSongNum(int n)
    {
        songNum = n;
    }

    public void StartSong()
    {
        StartCoroutine(Fade(levelCleared, true));
        StartCoroutine(Fade(gameOver, true));
        StartCoroutine(Fade(mainMenu, true));
        player.health = player.maxHealth;
        player.transform.position = new Vector3(0, 0, 0);
        GameObject.Find("HP Bar").GetComponent<Image>().fillAmount = 1;
        timesRepeated = 0;
        rawBeat = -7;
        beat = -7;
        audio.Play(songs[songNum].name);
        CreateNotes(songs[songNum]);
        player.paused = false;
    }

    public void ExitToMenu()
    {
        StartCoroutine(ExitToMenuCor());
    }

    private IEnumerator ExitToMenuCor()
    {
        GameObject.Find("Fader").GetComponent<Animator>().Play("FadeCross");
        yield return new WaitForSeconds(0.5f);
        levelCleared.SetActive(false);
        gameOver.SetActive(false);
        mainMenu.GetComponent<CanvasGroup>().alpha = 1;
        mainMenu.SetActive(true);
    }

    void FixedUpdate()
    {
        //Keep time
        if (songNum < songs.Length && !player.paused)
        {
            rawBeat += 0.02f * (songs[songNum].tempo / 60.0f);
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
                else if (!endLevel)
                {
                    endLevel = true;
                    StartCoroutine(EndLevel());
                }
            }
            beat = Mathf.Round(4 * rawBeat) / 4;
            if (beat == 0 && !resetNotes)
            {
                resetNotes = true;
                foreach (Note n in notes)
                {
                    n.spawned = false;
                }
            }
        }
    }

    void Update()
    {
        if (songNum < songs.Length)
        {
            //Spawn Notes
            foreach (Note n in notes)
            {
                if (n.beat%songs[songNum].length == (beat+2)%songs[songNum].length && !n.spawned && beat >= -1)
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
        foreach (Transform child in GameObject.Find("Enemies").transform)
        {
            Destroy(child.gameObject);
        }
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
        notes = new List<Note>();
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
