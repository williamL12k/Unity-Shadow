using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Ctrol : MonoBehaviour
{
    public Vector2 target;
    [SerializeField]
    float speed;
    private void FixedUpdate()
    {
        if (target != null)
        {
            this.transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.x+5f,target.y+5f), Time.deltaTime * speed);
            if (Vector2.Distance(transform.position, target) < 0.2f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            Debug.Log("Hit Player");
        }
            Destroy(gameObject);
    }
}
