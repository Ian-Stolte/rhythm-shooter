using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        GameObject.Find("Rhythm Manager").GetComponent<RhythmManager>().songNum = songNum;
    }
}
