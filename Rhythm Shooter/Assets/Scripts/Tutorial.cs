using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public IEnumerator PlayTutorial()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().paused = true;
        foreach (Transform child in transform)
        {
            yield return new WaitForSeconds(0.2f);
            child.gameObject.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            child.gameObject.SetActive(false);
        }
        GameObject.Find("Rhythm Manager").GetComponent<RhythmManager>().PlaySong();
    }
}