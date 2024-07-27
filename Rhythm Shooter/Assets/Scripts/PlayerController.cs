using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private GameObject directionIndicator;
    private float mouseAngle;
    private Vector3 bulletDir;

    [SerializeField] private float maxHealth;
    public float health;
    [SerializeField] private float knockback;

    [SerializeField] private GameObject kickBullet;
    [SerializeField] private GameObject snareBullet;
    private RhythmManager rhythm;
    [SerializeField] private BoxCollider2D bar;

    void Start()
    {
        health = maxHealth;
        directionIndicator = GameObject.Find("Direction Indicator");
        rhythm = GameObject.Find("Rhythm Manager").GetComponent<RhythmManager>();
    }

    void Update()
    {
        mouseAngle = GetMouseRot();
        directionIndicator.transform.RotateAround(transform.position, new Vector3(0, 0, 1), mouseAngle - directionIndicator.transform.rotation.eulerAngles.z);
        /*Bounds b = bar.bounds;
        if (Input.GetKeyDown(KeyCode.Space) && Physics2D.OverlapBox(b.center, b.extents*2, 0, LayerMask.GetMask("Kick")))
        {
            FireBullet();
            Destroy(Physics2D.OverlapBox(b.center, b.extents * 2, 0, LayerMask.GetMask("Kick")).gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && Physics2D.OverlapBox(b.center, b.extents*2, 0, LayerMask.GetMask("Snare")))
        {
            FireBullet();
            Destroy(Physics2D.OverlapBox(b.center, b.extents*2, 0, LayerMask.GetMask("Snare")).gameObject);
        }*/
    }

    private float GetMouseRot()
    {
        Vector3 mousePos = Input.mousePosition;
        Rect canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>().rect;
        Vector3 canvasScale = GameObject.Find("Canvas").GetComponent<RectTransform>().localScale;
        float mouseXChange = mousePos.x - 0.5f*canvasRect.width*canvasScale.x;
        float mouseYChange = mousePos.y - 0.5f*canvasRect.height*canvasScale.y;
        bulletDir = new Vector3(mouseXChange, mouseYChange, 0);
        bulletDir = Vector3.Normalize(bulletDir);
        return(Mathf.Atan2(mouseYChange, mouseXChange) * Mathf.Rad2Deg - 90);
    }

    public void FireBullet(string type)
    {
        GetComponent<Animator>().Play("Fire");
        GameObject bullet = null;
        if (type == "Kick")
        {
            bullet = Instantiate(kickBullet, transform.position + bulletDir, Quaternion.identity, GameObject.Find("Bullets").transform);
        }
        else if (type == "Snare")
        {
            bullet = Instantiate(snareBullet, transform.position + bulletDir, Quaternion.identity, GameObject.Find("Bullets").transform);
        }
        if (bullet != null)
            bullet.GetComponent<Bullet>().direction = bulletDir;
    }

    public void TakeDamage(GameObject g, float dmg)
    {   
        health -= dmg;
        GetComponent<Animator>().Play("TakeDamage");
        GameObject.Find("HP Bar").GetComponent<Image>().fillAmount = health/maxHealth;
        //TODO: knockback (without a rigidbody??)
        //GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(transform.position - g.transform.position)*knockback);
    }
}
