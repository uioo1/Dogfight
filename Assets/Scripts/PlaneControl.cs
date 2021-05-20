using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneControl : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb2d;
    [HideInInspector]
    public SpriteRenderer sprite;
    public GameObject bullet_prefab;
    public GameObject gunPoint;

    public float speed;
    public float rotationSpeed;
    float angle = 0;
    float horizon_input;
    bool isShoot = false;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizon_input = Input.GetAxis("Horizontal");
        if(Input.GetButton("Fire1"))
        {
            isShoot = true;
            Shooting();
        }
    }

    void FixedUpdate()
    {
        rb2d.velocity = sprite.transform.right * speed;

        Vector3 direction = new Vector3(0, 0, horizon_input);

        if (direction != Vector3.zero)
        {
            Debug.Log("Input:" + horizon_input);
            //float angle = Mathf.Atan2(rb2d.velocity.x, rb2d.velocity.y) * Mathf.Rad2Deg;

            angle += -horizon_input * 5;

            // 즉시 회전
            //transform.rotation = Quaternion.Euler(0, 0, angle);

            // 부드러운 회전 
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.fixedDeltaTime);      
        }
    }

    void Shooting()
    {
        GameObject bullet = Instantiate(bullet_prefab, gunPoint.transform.position, gunPoint.transform.rotation);
    }
}
