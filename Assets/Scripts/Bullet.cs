using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isFliped = false;
    public float bulletSpeed = 20f;

    [HideInInspector]
    public GameObject parentPlane;
    Rigidbody2D rb2d;
    SpriteRenderer sprite;
    //  탄퍼짐 정도
    float bullet_dispersion = 0.05f;
    Vector3 rand_vec;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();        
        rand_vec = new Vector3(sprite.transform.right.x + Random.Range(-bullet_dispersion, bullet_dispersion), 
                    sprite.transform.right.y + Random.Range(-bullet_dispersion, bullet_dispersion), 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCreated(bool planeFlip)
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();        
        rand_vec = new Vector3(sprite.transform.right.x + Random.Range(-bullet_dispersion, bullet_dispersion), 
                    sprite.transform.right.y + Random.Range(-bullet_dispersion, bullet_dispersion), 0);
        if(planeFlip)
            isFliped = true;
        else
            isFliped = false;

        Invoke("DestroyBullet", 2f);
    }

    public void DestroyBullet()
    {
        BulletPool.ReturnObject(this.gameObject);
    }
    private void FixedUpdate()
    {
        //rb2d.velocity = sprite.transform.right * bulletSpeed;
        if(!isFliped)
        {
            rb2d.velocity = rand_vec * bulletSpeed;
        }
        else
        {            
            rb2d.velocity = rand_vec * bulletSpeed * -1f;
        }        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Ground")
        {
            DestroyBullet();
        }
        else if(other.tag == "Plane")
        {
            if(other.gameObject != parentPlane)
            {
                PlaneControl plane = other.gameObject.GetComponent<PlaneControl>();
                plane.Hited();
                DestroyBullet();
            }                
        }
    }
}
