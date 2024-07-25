using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject directionIndicator;

    void Start()
    {
        directionIndicator = GameObject.Find("Direction Indicator");        
    }

    void Update()
    {
        directionIndicator.transform.RotateAround(transform.position, Vector3.Up, 90/*get angle from mouse*/);
    }
}
