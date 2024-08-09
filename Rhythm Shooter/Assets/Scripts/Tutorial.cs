using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public IEnumerator PlayTutorial()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().paused = true;
        if (GameObject.Find("Audio Manager").GetComponent<AudioManager>().currentSong != null)
            GameObject.Find("Audio Manager").GetComponent<AudioManager>().currentSong.source.Pause();
        foreach (Transform child in transform)
        {
            yield return new WaitForSeconds(0.2f);
            child.gameObject.SetActive(true);
            if (child.transform.childCount > 1)
            {
                yield return new WaitForSeconds(1);
                child.GetChild(1).gameObject.SetActive(true);
            }
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space));
            child.gameObject.SetActive(false);
        }
        if (GameObject.Find("Rhythm Manager").GetComponent<RhythmManager>().timesRepeated > 0)
        {
            GameObject.Find("Audio Manager").GetComponent<AudioManager>().currentSong.source.Play();
            GameObject.Find("Player").GetComponent<PlayerController>().paused = false;
        }
        else
            GameObject.Find("Rhythm Manager").GetComponent<RhythmManager>().PlaySong();
    }
}