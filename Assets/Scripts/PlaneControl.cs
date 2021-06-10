using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneControl : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb2d;
    [HideInInspector]
    public SpriteRenderer sprite;
    public AudioManager audioManager;
    public GameObject bullet_prefab;
    public GameObject gunPoint;
    public GameObject smoke_obj;
    public GameObject explode_child;
    public GameObject explode_prefab;
    Smoke smoke;

    public int planeHealth;
    public float speed;
    public float rotationSpeed;
    public float lowerRotationSpeed;
    float angle = 0;
    float horizon_input;
    bool isExploded = false;
    bool isFliped = false;
    bool waitShoot = false;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        smoke = smoke_obj.GetComponent<Smoke>();
    }

    // Update is called once per frame
    void Update()
    {
        horizon_input = Input.GetAxis("Horizontal");
        if(Input.GetButton("Fire1") && !waitShoot)
        {
            waitShoot = true;
            Shooting();
        }
        
        if(planeHealth <= 5)
        {
            smoke_obj.SetActive(true);
            if(planeHealth <= 0)
            {
                if(!isExploded)
                {
                    isExploded = true;
                    explode_child.SetActive(true);
                    audioManager.Play("explosion_plane");                    
                }
                
            }
        }
    }

    void FixedUpdate()
    {
        rb2d.velocity = sprite.transform.right * speed;
        if(planeHealth > 0)
        {            
            Vector3 direction = new Vector3(0, 0, horizon_input);
            if (direction != Vector3.zero && planeHealth > 0)
            {
                //Debug.Log("Input:" + horizon_input);
                //float angle = Mathf.Atan2(rb2d.velocity.x, rb2d.velocity.y) * Mathf.Rad2Deg;

                angle -= horizon_input * 5;

                // 즉시 회전
                //transform.rotation = Quaternion.Euler(0, 0, angle);
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                                            Quaternion.Euler(0, 0, angle), 
                                            rotationSpeed * Time.fixedDeltaTime);
            }
        }
        else if(planeHealth <= 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                                        Quaternion.Euler(0, 0, -85f), 
                                        Time.fixedDeltaTime);
            rb2d.gravityScale = 1f;
        }
    }

    void Shooting()
    {
        GameObject bullet = Instantiate(bullet_prefab, gunPoint.transform.position, gunPoint.transform.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        int i = Random.Range(0,3);
        if(i == 0)
            audioManager.Play("gun1_01");
        else if(i == 1)
            audioManager.Play("gun1_02");
        else
            audioManager.Play("gun1_03");
        bulletScript.parentPlane = this.gameObject;
        StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {        
        yield return new WaitForSeconds(0.1f);
        waitShoot = false;
    }

    public void Hited()
    {        
        planeHealth--;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Ground")
        {
            GameObject explosion = Instantiate(explode_prefab, transform.position, Quaternion.identity);
            audioManager.Play("explosion_ground");
            Destroy(explosion, 3f);
            Destroy(gameObject);
        }
        else if(other.tag == "Plane")
        {
            GameObject explosion = Instantiate(explode_prefab, transform.position, Quaternion.identity);
            audioManager.Play("explosion_ground");
            Destroy(explosion, 3f);
            Destroy(gameObject);   
        }
    }
}
