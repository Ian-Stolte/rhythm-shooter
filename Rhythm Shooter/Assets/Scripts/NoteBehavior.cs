using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteBehavior : MonoBehaviour
{
    [SerializeField] private float speed;
    public bool played;
    private bool startedFade;
    private bool missedSound;
    private Vector3 barPos;
    public string type;
    public KeyCode triggerKey;

    private AudioManager audio;

    void Start()
    {
        audio = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        barPos = GameObject.Find("Bar").GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(triggerKey) && !played)
        {
            bool correctTiming = false;
            foreach (GameObject o in GameObject.FindGameObjectsWithTag(type))
            {
                if (Mathf.Abs(o.GetComponent<RectTransform>().anchoredPosition.x - barPos.x) < 30)
                    correctTiming = true;
            }
            if (Mathf.Abs(GetComponent<RectTransform>().anchoredPosition.x - barPos.x) < 30)
            {
                played = true;
                GameObject.Find("Player").GetComponent<PlayerController>().FireBullet(type);
                GetComponent<Animator>().Play("HitNote");
                audio.Play("Success " + type); //TODO: find some way to play it at the right time, even if the input is a little off
                StartCoroutine(FadeOut(1));
            }
            else if (GetComponent<RectTransform>().anchoredPosition.x - barPos.x < 100 && !correctTiming)
            {
                played = true;
                GetComponent<CanvasGroup>().alpha = 0.4f;
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        GetComponent<RectTransform>().anchoredPosition -= new Vector2(speed*0.02f, 0);      
        if (GetComponent<RectTransform>().anchoredPosition.x < barPos.x-40 && !startedFade)
        {
            StartCoroutine(FadeOut(2));
        }
        if (GetComponent<RectTransform>().anchoredPosition.x < barPos.x && !missedSound)
        {
            missedSound = true;
            audio.GetComponent<AudioManager>().Play("Missed " + type);
        }
    }

    private IEnumerator FadeOut(float duration)
    {
        startedFade = true;
        float startingAlpha = GetComponent<CanvasGroup>().alpha;
        for (float i = 0; i < 1; i += 0.01f)
        {
            GetComponent<CanvasGroup>().alpha = GetComponent<CanvasGroup>().alpha*(1-i);
            yield return new WaitForSeconds(0.01f * duration);
        }
        Destroy(gameObject);
    }

}