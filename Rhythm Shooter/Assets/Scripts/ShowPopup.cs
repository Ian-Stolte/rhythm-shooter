using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPopup : MonoBehaviour
{
    [SerializeField] private GameObject popup;

    public void Show()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Popup"))
        {
            g.SetActive(false);
            //maybe play an animation
        }
        popup.SetActive(true);
        //maybe play an animation
    }
}
