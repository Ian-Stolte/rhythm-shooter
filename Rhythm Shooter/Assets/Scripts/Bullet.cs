using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum bulletType
    {
        KICK,
        SNARE,
        HIHAT
    }
    public bulletType type;
    public Vector3 direction;
    public float speed;
    public float dmg;
    private float distanceTraveled;
    
    [SerializeField] private GameObject dmgTxtPrefab;

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
            else if (Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Enemy")))
            {
                GameObject obj = Physics2D.OverlapCircle(transform.position, 1, LayerMask.GetMask("Enemy")).gameObject;
                obj.GetComponent<EnemyBehavior>().health -= dmg;
                DamageNumbers(obj);
                Color c = obj.GetComponent<SpriteRenderer>().color;
                float alpha = 0.5f + 0.5f*(obj.GetComponent<EnemyBehavior>().health/obj.GetComponent<EnemyBehavior>().maxHealth);
                obj.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, alpha);
                Destroy(gameObject);
            }
        }
    }

    private void DamageNumbers(GameObject enemy)
    {
        GameObject dmgTxt = null;
        /*if (type == bulletType.KICK)
        {*/
            if (enemy.GetComponent<EnemyBehavior>().kickDmg != null)
            {
                if (enemy.GetComponent<EnemyBehavior>().kickDmg.GetComponent<DestroyAfterDelay>().timer > 0.3f)
                    dmgTxt = enemy.GetComponent<EnemyBehavior>().kickDmg;   
            }
            if (dmgTxt == null)
            {
                GameObject obj = Instantiate(dmgTxtPrefab, enemy.transform.position + new Vector3(-0.5f, 0.8f, 0), Quaternion.identity);
                enemy.GetComponent<EnemyBehavior>().kickDmg = obj;
                obj.GetComponent<TMPro.TextMeshPro>().text = "" + dmg;
                //obj.GetComponent<TMPro.TextMeshPro>().color = GetComponent<SpriteRenderer>().color;
            }
        /*}
        else if (type == bulletType.SNARE)
        {
            if (enemy.GetComponent<EnemyBehavior>().snareDmg != null)
            {
                if (enemy.GetComponent<EnemyBehavior>().snareDmg.GetComponent<DestroyAfterDelay>().timer > 0.3f)
                    dmgTxt = enemy.GetComponent<EnemyBehavior>().snareDmg;   
            }
            if (dmgTxt == null)
            {
                GameObject obj = Instantiate(dmgTxtPrefab, enemy.transform.position + new Vector3(0.5f, 0.8f, 0), Quaternion.identity);
                enemy.GetComponent<EnemyBehavior>().snareDmg = obj;
                obj.GetComponent<TMPro.TextMeshPro>().text = "" + dmg;
                obj.GetComponent<TMPro.TextMeshPro>().color = GetComponent<SpriteRenderer>().color;
            }
        }*/
        if (dmgTxt != null)
        {
            dmgTxt.GetComponent<TMPro.TextMeshPro>().text = "" + (int.Parse(dmgTxt.GetComponent<TMPro.TextMeshPro>().text) + dmg);
            //dmgTxt.GetComponent<DestroyAfterDelay>().timer = dmgTxt.GetComponent<DestroyAfterDelay>().lifetime;
            //dmgTxt.GetComponent<Animator>().Play("Text Fade");
        }
    }
}
