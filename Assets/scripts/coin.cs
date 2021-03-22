using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    private GameManager go;

     void Start()
    {
        go = FindObjectOfType(typeof(GameManager)) as GameManager;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            go.GetComponent<GameManager>().UpdateScore(1);
           
            Destroy(gameObject);
        }
    }
}
