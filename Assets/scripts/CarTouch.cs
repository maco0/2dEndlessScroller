using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTouch : MonoBehaviour
{
    private Vector3 TouchPos;
    private Rigidbody2D rb;
    private Vector3 dir;
    [HideInInspector]
    public float speed=2;

    public bool Hit;
    public static CarTouch instance;


    float xBounds = 1.3f;
   

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
    }

    // Update is called once per frame
    void Update()
    {
       //getting the input of touch
            if (Input.touchCount > 0)
            {
            //getting the first touch input
                Touch touch = Input.GetTouch(0);
            //converting touch position to world point from screen point
                TouchPos = Camera.main.ScreenToWorldPoint(touch.position);
            //disabling the touch position on z axis
                TouchPos.z = 0;
            //getting the direction
                dir = (TouchPos - transform.position);
                //moving car
                rb.velocity = new Vector2(dir.x, 0) * speed;
            //clamping the transform of the car with x bounds , disabling car from going out of bounds
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, -xBounds, xBounds), transform.position.y);
                if (touch.phase == TouchPhase.Ended)
                {
                   //setting car velocity to zero
                    rb.velocity = Vector2.zero;
                }
            }
        
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "obstacle")
        {

            Hit = true;
            Destroy(collision.gameObject);
        }

    }

}
