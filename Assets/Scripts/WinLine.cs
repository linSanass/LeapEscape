using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLine : MonoBehaviour
{
    private string playerTag = "Player";
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == playerTag)
        {
            Debug.Log("胜利");
        }
    }
}
