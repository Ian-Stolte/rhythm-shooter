using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float dmg;
    private float distanceTraveled;

    void FixedUpdate()
    {
        if (!GameObject.Find("Player").GetComponent<PlayerController>().paused)
        {
            transform.position += direction*speed;
            distanceTraveled += Vector3.Magnitude(direction)*speed;
            if (distanceTraveled > 20)
            {
                Destroy(gameObject);
            }
            else if (Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Player")))
            {
                GameObject.Find("Player").GetComponent<PlayerController>().TakeDamage(gameObject, dmg);
                Destroy(gameObject);
            }
        }
    }
}