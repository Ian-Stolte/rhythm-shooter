using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private GameObject directionIndicator;
    private float mouseAngle;
    private Vector3 bulletDir;

    public float maxHealth;
    public float health;
    //[SerializeField] private float knockback;
    [SerializeField] private float speed;
    public bool paused;

    [SerializeField] private GameObject kickBullet;
    [SerializeField] private GameObject snareBullet;
    [SerializeField] private GameObject hiHatBullet;
    private RhythmManager rhythm;
    [SerializeField] private BoxCollider2D bar;

    [SerializeField] private GameObject gameOver;

    void Start()
    {
        paused = true;
        health = maxHealth;
        directionIndicator = GameObject.Find("Direction Indicator");
        rhythm = GameObject.Find("Rhythm Manager").GetComponent<RhythmManager>();
    }

    void Update()
    {
        if (!paused)
        {
            mouseAngle = GetMouseRot();
            directionIndicator.transform.RotateAround(transform.position, new Vector3(0, 0, 1), mouseAngle - directionIndicator.transform.rotation.eulerAngles.z);
            if (health <= 0)
            {
                StartCoroutine(GameOver());
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                paused = true;
                GameObject.Find("Audio Manager").GetComponent<AudioManager>().currentSong.source.Pause();
                gameOver.SetActive(true);
                gameOver.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "Paused";
                gameOver.GetComponent<CanvasGroup>().alpha = 1;
                gameOver.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 1;
                Time.timeScale = 0;
            }
            else if (gameOver.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text == "Paused")
            {
                paused = false;
                GameObject.Find("Audio Manager").GetComponent<AudioManager>().currentSong.source.Play();
                gameOver.SetActive(false);
                gameOver.GetComponent<CanvasGroup>().alpha = 0;
                Time.timeScale = 1;
            }
        }
    }

    void FixedUpdate()
    {
        if (!paused)
        {
            float moveMag = Mathf.Sqrt(Mathf.Pow(Input.GetAxisRaw("Horizontal"), 2) + Mathf.Pow(Input.GetAxisRaw("Vertical"), 2));
            if (moveMag > 0)
                transform.position += new Vector3(Input.GetAxisRaw("Horizontal")*speed*0.02f/moveMag, Input.GetAxisRaw("Vertical")*speed*0.02f/moveMag, 0);
        }
    }

    private float GetMouseRot()
    {
        Vector3 mousePos = Input.mousePosition;
        Rect canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>().rect;
        Vector3 canvasScale = GameObject.Find("Canvas").GetComponent<RectTransform>().localScale;
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        float camWidth = cam.orthographicSize*cam.aspect;
        float camHeight = cam.orthographicSize;
        float playerXPct = (transform.position.x + camWidth) / (camWidth*2);
        float playerYpct = (transform.position.y + camHeight) / (camHeight*2);
        float mouseXChange = mousePos.x - playerXPct*canvasRect.width*canvasScale.x;
        float mouseYChange = mousePos.y - playerYpct*canvasRect.height*canvasScale.y;
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
        else if (type == "HiHat")
        {
            bullet = Instantiate(hiHatBullet, transform.position + bulletDir, Quaternion.identity, GameObject.Find("Bullets").transform);
        }
        if (bullet != null)
            bullet.GetComponent<Bullet>().direction = bulletDir;
    }

    public void TakeDamage(GameObject g, float dmg)
    {   
        health -= dmg;
        GetComponent<Animator>().Play("TakeDamage");
        GameObject.Find("HP Bar").GetComponent<Image>().fillAmount = health/maxHealth;
        rhythm.multiplier = 1;
        //TODO: knockback (without a rigidbody??)
        //GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(transform.position - g.transform.position)*knockback);
    }

    private IEnumerator GameOver()
    {
        paused = true;
        StartCoroutine(GameObject.Find("Audio Manager").GetComponent<AudioManager>().FadeOutAll(1));
        StartCoroutine(rhythm.Fade(gameOver, false));
        gameOver.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "Game Over";
        yield return new WaitForSeconds(1);
        StartCoroutine(rhythm.Fade(gameOver.transform.GetChild(2).gameObject, false));
        foreach (Transform child in GameObject.Find("Enemies").transform)
            Destroy(child.gameObject);
    }
}
