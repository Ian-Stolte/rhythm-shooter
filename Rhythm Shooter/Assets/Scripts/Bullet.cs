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
            Color c = obj.GetComponent<SpriteRenderer>().color;
            float alpha = 0.5f + 0.5f*(obj.GetComponent<EnemyBehavior>().health/obj.GetComponent<EnemyBehavior>().maxHealth);
            obj.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, alpha);
            Destroy(gameObject);
        }
    }
}
