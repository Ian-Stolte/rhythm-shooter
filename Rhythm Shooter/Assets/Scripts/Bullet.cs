using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    private float distanceTraveled;

    void FixedUpdate()
    {
        transform.position += direction*speed;
        distanceTraveled += Vector3.Magnitude(direction)*speed;
        if (distanceTraveled > 20)
        {
            Destroy(gameObject);
        }
    }
}
