using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public IEnumerator PlayTutorial()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            child.gameObject.SetActive(false);
        }
        GameObject.Find("Rhythm Manager").GetComponent<RhythmManager>().PlaySong();
    }
}
