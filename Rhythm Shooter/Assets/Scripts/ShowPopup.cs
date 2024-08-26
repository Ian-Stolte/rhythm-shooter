using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPopup : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject exclamationPt;

    public void Show(int songNum)
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Popup"))
        {
            g.SetActive(false);
            //maybe play an animation
        }
        popup.SetActive(true);
        if (exclamationPt != null)
            exclamationPt.SetActive(false);
        //maybe play an animation

        RhythmManager r = GameObject.Find("Rhythm Manager").GetComponent<RhythmManager>();
        r.songNum = songNum;
        if (GameObject.Find("High Score") != null)
            GameObject.Find("High Score").GetComponent<TMPro.TextMeshProUGUI>().text = "" + r.songs[songNum+(int)GameObject.Find("Difficulty Slider").GetComponent<Slider>().value].highScore;
        if (r.skipTutorial)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Exclamation"))
            {
                g.SetActive(false);
            }
        }
    }
}
