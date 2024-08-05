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
    public int songsUnlocked;
    [SerializeField] private List<Note> notes = new List<Note>();

    private bool endLevel;
    private bool resetNotes;
    private bool spawnMeasureBar = true;
    public bool doingKick;
    public bool doingSnare;
    public bool doingHiHat;

    [SerializeField] private GameObject kickPrefab;
    [SerializeField] private GameObject snarePrefab;
    [SerializeField] private GameObject hiHatPrefab;
    [SerializeField] private GameObject measureBarPrefab;
    [SerializeField] private GameObject levelCleared;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject enemySpawner;

    private bool snareUnlocked;
    [SerializeField] private GameObject snareCheckbox;
    private bool fightUnlocked;
    [SerializeField] private GameObject fightButton;
    

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

    public void StartSong(bool practice)
    {
        Time.timeScale = 1;
        if (practice)
           enemySpawner.SetActive(false);
        StartCoroutine(SetUpSong());
    }

    private IEnumerator SetUpSong()
    {
        GameObject.Find("Fader").GetComponent<Animator>().Play("FadeCross");
        if (GameObject.Find("Kick Checkbox") != null)
            doingKick = GameObject.Find("Kick Checkbox").GetComponent<Checkbox>().isChecked;
        if (GameObject.Find("Snare Checkbox") != null)
            doingSnare = GameObject.Find("Snare Checkbox").GetComponent<Checkbox>().isChecked;
        if (GameObject.Find("HiHat Checkbox") != null)
            doingHiHat = GameObject.Find("HiHat Checkbox").GetComponent<Checkbox>().isChecked;
        yield return new WaitForSeconds(0.5f);
        foreach (Transform child in GameObject.Find("Notes").transform)
            Destroy(child.gameObject);
        foreach (Transform child in GameObject.Find("Measure Bars").transform)
            Destroy(child.gameObject);
        foreach (Transform child in GameObject.Find("Enemies").transform)
            Destroy(child.gameObject);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Popup"))
            g.SetActive(false);
        levelCleared.SetActive(false);
        gameOver.SetActive(false);
        mainMenu.SetActive(false);
        /*StartCoroutine(Fade(levelCleared, true));
        StartCoroutine(Fade(gameOver, true));
        StartCoroutine(Fade(mainMenu, true));*/
        player.health = player.maxHealth;
        player.transform.position = new Vector3(0, 0, 0);
        GameObject.Find("HP Bar").GetComponent<Image>().fillAmount = 1;
        timesRepeated = 0;
        endLevel = false;
        spawnMeasureBar = true;
        rawBeat = 1 - 2*songs[songNum].beatsPerMeasure;
        beat = 1 - 2*songs[songNum].beatsPerMeasure;
        if (!snareUnlocked)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(GameObject.Find("First Tutorial").GetComponent<Tutorial>().PlayTutorial());
        }
        else
        {
            PlaySong();
        }
    }

    public void PlaySong()
    {
        audio.Play(songs[songNum].name);
        CreateNotes(songs[songNum]);
        player.paused = false;
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1;
        StartCoroutine(ExitToMenuCor());
    }

    private IEnumerator ExitToMenuCor()
    {
        StartCoroutine(GameObject.Find("Audio Manager").GetComponent<AudioManager>().FadeOutAll(1));
        GameObject.Find("Fader").GetComponent<Animator>().Play("FadeCross");
        yield return new WaitForSeconds(0.5f);
        enemySpawner.SetActive(true);
        levelCleared.SetActive(false);
        gameOver.SetActive(false);
        mainMenu.GetComponent<CanvasGroup>().alpha = 1;
        mainMenu.SetActive(true);
        for (int i = 0; i < songs.Length; i++)
        {
            GameObject.Find("Song Buttons").transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = songs[i].unlocked;
        }
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
                        n.spawned = false;
                }
                else if (!endLevel)
                {
                    endLevel = true;
                    StartCoroutine(EndLevel());
                }
            }
            beat = Mathf.Round(4 * rawBeat) / 4;
            if (beat == 1 && !resetNotes)
            {
                resetNotes = true;
                foreach (Note n in notes)
                    n.spawned = false;
                //TODO: fix this! (notes still disappearing)
            }
            if (Mathf.Abs(beat)%1 == 0 && !spawnMeasureBar)
            {
                spawnMeasureBar = true;
                GameObject obj = Instantiate(measureBarPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Measure Bars").transform);
                obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(776, -375, 0);
                obj.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "" + ((beat+1+2*songs[songNum].beatsPerMeasure)%songs[songNum].beatsPerMeasure+1);
                if ((beat+2)%songs[songNum].beatsPerMeasure != 1)
                    obj.GetComponent<CanvasGroup>().alpha = 0.1f;
            }
            else if (Mathf.Abs(beat)%1 == 0.5f)
                spawnMeasureBar = false;
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
                        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(800, -440, 0);
                    }
                    else if (n.drumType == Note.drums.SNARE && doingSnare)
                    {
                        GameObject obj = Instantiate(snarePrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Notes").transform);
                        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(800, -375, 0);
                    }
                    else if (n.drumType == Note.drums.HIHAT && doingHiHat)
                    {
                        GameObject obj = Instantiate(hiHatPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Notes").transform);
                        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(800, -310, 0);
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
        if (fightUnlocked && enemySpawner.activeSelf)
        {
            songsUnlocked++;
            songs[songsUnlocked].unlocked = true;
        }
        if (snareUnlocked)
        {
            fightUnlocked = true;
            fightButton.SetActive(true);
        }
        snareUnlocked = true;
        snareCheckbox.SetActive(true);
        yield return new WaitForSeconds(1);
        foreach (Transform child in GameObject.Find("Enemies").transform)
            Destroy(child.gameObject);
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
