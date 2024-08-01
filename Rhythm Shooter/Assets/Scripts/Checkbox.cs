using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkbox : MonoBehaviour
{
    public bool isChecked;

    public void Check()
    {
        isChecked = (!isChecked);
        transform.GetChild(3).gameObject.SetActive(isChecked);
    }
}
