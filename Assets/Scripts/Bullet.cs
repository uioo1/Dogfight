using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
        Destroy(gameObject, 2f);
        rand_vec = new Vector3(sprite.transform.right.x + Random.Range(-bullet_dispersion, bullet_dispersion), 
                    sprite.transform.right.y + Random.Range(-bullet_dispersion, bullet_dispersion), 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        //rb2d.velocity = sprite.transform.right * bulletSpeed;
        rb2d.velocity = rand_vec * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Ground")
        {
            Destroy(gameObject);
        }
        else if(other.tag == "Plane")
        {
            if(other.gameObject != parentPlane)
            {
                PlaneControl plane = other.gameObject.GetComponent<PlaneControl>();
                plane.Hited();
                Destroy(gameObject);
            }                
        }
    }
}
