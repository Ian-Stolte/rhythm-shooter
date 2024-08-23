using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiffSlider : MonoBehaviour
{
    public int diffLvl;
    private string[] diffTxts = new string[]{"Easy", "Medium", "Hard"}; //TODO: could make these more interesting
    [SerializeField] private Color[] diffColors;

    void Update()
    {
        if (GetComponent<Slider>().value != diffLvl)
        {
            diffLvl = (int)GetComponent<Slider>().value;
            transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = diffTxts[diffLvl];
            transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().color = diffColors[diffLvl];
            transform.GetChild(1).GetChild(0).GetComponent<Image>().color = diffColors[diffLvl];
        }
    }
}
