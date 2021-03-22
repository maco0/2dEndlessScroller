using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCoinMove : MonoBehaviour
{
    

    [HideInInspector]
    public float speed;
   
    public static ObstacleCoinMove instance;
    

    void Update()
    {
        speed = backgroundRepeat.instance.speed;
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (gameObject.transform.position.y < -3.7)
        {
            Destroy(gameObject);
        }
    }
}
