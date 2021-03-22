using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundRepeat : MonoBehaviour
{
    [HideInInspector]
   public float speed=2;
    public float BaseSpeed=2;
    private float height;
    Vector3 startPos;
    public BoxCollider2D bc;
    public bool Canrepeat;

    //creating instnace
    public static backgroundRepeat instance;
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
        //geting starting position as reference for repeating 
        startPos = transform.position;
        height = bc.size.y/2;
    }

    void Update()
    {
        //on gameplay canrepeat turns true  and sprite constantly translates down
        if (Canrepeat)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
            //if sprites goes for a certain position we reset sprite
            if (transform.position.y < startPos.y - height)
            {
                transform.position = startPos;
            }
        }
    }
}
