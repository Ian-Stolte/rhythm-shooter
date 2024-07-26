using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject directionIndicator;
    [SerializeField] private float mouseAngle;
    private Vector3 bulletDir;

    [SerializeField] private GameObject bulletPrefab;
    private RhythmManager rhythm;

    void Start()
    {
        directionIndicator = GameObject.Find("Direction Indicator");
        rhythm = GameObject.Find("Rhythm Manager").GetComponent<RhythmManager>();
    }

    void Update()
    {
        mouseAngle = GetMouseRot();
        directionIndicator.transform.RotateAround(transform.position, new Vector3(0, 0, 1), mouseAngle - directionIndicator.transform.rotation.eulerAngles.z);
        if (Input.GetKeyDown(KeyCode.Space) && (rhythm.beat == 1 || rhythm.beat == 3))
        {
            FireBullet();
        }
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

    private void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position + bulletDir, Quaternion.identity, GameObject.Find("Bullets").transform);
        bullet.GetComponent<Bullet>().direction = bulletDir;
    }
}
