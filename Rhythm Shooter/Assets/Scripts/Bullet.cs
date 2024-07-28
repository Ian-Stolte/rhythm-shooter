using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float dmg;
    private float distanceTraveled;

    void FixedUpdate()
    {
        transform.position += direction*speed;
        distanceTraveled += Vector3.Magnitude(direction)*speed;
        if (distanceTraveled > 20)
        {
            Destroy(gameObject);
        }
        else if (Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Enemy")))
        {
            GameObject obj = Physics2D.OverlapCircle(transform.position, 1, LayerMask.GetMask("Enemy")).gameObject;
            obj.GetComponent<EnemyBehavior>().health -= dmg;
            obj.GetComponent<SpriteRenderer>().color = obj.GetComponent<EnemyBehavior>().dmgColor; //TODO: slowly change color as enemy takes dmg
            Destroy(gameObject);
        }
    }
}
