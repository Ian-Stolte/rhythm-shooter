using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureBar : MonoBehaviour
{
    [SerializeField] private float speed;
    
    void FixedUpdate()
    {
        if (!GameObject.Find("Player").GetComponent<PlayerController>().paused)
        {
            GetComponent<RectTransform>().anchoredPosition -= new Vector2(speed*0.02f, 0);      
            if (GetComponent<RectTransform>().anchoredPosition.x < GameObject.Find("Bar").GetComponent<RectTransform>().anchoredPosition.x)
                Destroy(gameObject);
        }
    }
}
