using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBehavior : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool startedFade;
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
        if (Input.GetKeyDown(triggerKey) && Mathf.Abs(GetComponent<RectTransform>().anchoredPosition.x - barPos.x) < 30)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().FireBullet(type);
            audio.Play("Success " + type); //find some way to play it at the right time, even if the input is a little off
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        GetComponent<RectTransform>().anchoredPosition -= new Vector2(speed*0.02f, 0);        
        if (GetComponent<RectTransform>().anchoredPosition.x < barPos.x-10 && !startedFade)
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        startedFade = true;
        audio.GetComponent<AudioManager>().Play("Missed " + type);
        for (float i = 0; i < 1; i += 0.01f)
        {
            GetComponent<CanvasGroup>().alpha = 1-i;
            yield return new WaitForSeconds(0.005f);
        }
        Destroy(gameObject);
    }

}