using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneControl : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public SpriteRenderer sprite;

    public float speed;
    public float rotationSpeed;
    float angle = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = sprite.transform.right * speed;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(0, 0, h);

        if (direction != Vector3.zero)
        {
            Debug.Log("Input:" + h);
            //float angle = Mathf.Atan2(rb2d.velocity.x, rb2d.velocity.y) * Mathf.Rad2Deg;

            angle += -h * 5;

            // 즉시 회전
            //transform.rotation = Quaternion.Euler(0, 0, angle);

            // 부드러운 회전 
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.fixedDeltaTime);      
        }
    }
}
