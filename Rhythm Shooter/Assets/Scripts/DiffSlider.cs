using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiffSlider : MonoBehaviour
{
    private string[] diffTxts = new string[]{"Easy", "Medium", "Hard"}; //TODO: could make these more interesting
    [SerializeField] private Color[] diffColors;
    private RhythmManager r;

    void Start()
    {
        r = GameObject.Find("Rhythm Manager").GetComponent<RhythmManager>();
    }

    void Update()
    {
        if (GetComponent<Slider>().value != r.diffLvl)
        {
            r.diffLvl = (int)GetComponent<Slider>().value;
            transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = diffTxts[r.diffLvl];
            transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().color = diffColors[r.diffLvl];
            transform.GetChild(1).GetChild(0).GetComponent<Image>().color = diffColors[r.diffLvl];
            GameObject.Find("High Score").GetComponent<TMPro.TextMeshProUGUI>().text = "" + r.songs[r.songNum+r.diffLvl].highScore;
            //Checkboxes
            transform.parent.GetChild(4).gameObject.SetActive(r.songs[r.songNum+r.diffLvl].kickBeats.Length > 0);
            transform.parent.GetChild(5).gameObject.SetActive(r.songs[r.songNum+r.diffLvl].snareBeats.Length > 0);
            transform.parent.GetChild(6).gameObject.SetActive(r.songs[r.songNum+r.diffLvl].hiHatBeats.Length > 0);
        }
    }
}
