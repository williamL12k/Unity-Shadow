using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01_HitCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("2222");
        if (collision.gameObject.tag == "Player")
        {
            print("Is Hit Player");
        }
    }
}
