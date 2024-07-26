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

    void Start()
    {
        barPos = GameObject.Find("Bar").GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(triggerKey) && Mathf.Abs(GetComponent<RectTransform>().anchoredPosition.x - barPos.x) < 20)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().FireBullet(type);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        GetComponent<RectTransform>().anchoredPosition -= new Vector2(speed*0.02f, 0);        
        if (GetComponent<RectTransform>().anchoredPosition.x < barPos.x && !startedFade)
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        startedFade = true;
        GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play(type + " Missed");
        for (float i = 0; i < 1; i += 0.01f)
        {
            GetComponent<CanvasGroup>().alpha = 1-i;
            yield return new WaitForSeconds(0.008f);
        }
        Destroy(gameObject);
    }

}