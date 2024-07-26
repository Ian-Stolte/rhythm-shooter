using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBehavior : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool startedFade;
    private GameObject bar;

    void Start()
    {
        bar = GameObject.Find("Bar");
    }

    void FixedUpdate()
    {
        GetComponent<RectTransform>().anchoredPosition -= new Vector2(speed*0.02f, 0);        
        if (GetComponent<RectTransform>().anchoredPosition.x < bar.GetComponent<RectTransform>().anchoredPosition.x && !startedFade)
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        startedFade = true;
        for (float i = 0; i < 1; i += 0.01f)
        {
            GetComponent<CanvasGroup>().alpha = 1-i;
            yield return new WaitForSeconds(0.008f);
        }
        Destroy(gameObject);
    }

}
