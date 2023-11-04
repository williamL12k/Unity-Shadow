using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TerrainTools;

public class Enemy01Controller : MonoBehaviour
{
    public Transform[] Points;
    Vector2 center;
    [SerializeField]
    float height=2f;
    public Transform Player;
    public Transform My_Enemy;
    public Animator animator;
    public float MoveSpeed;
    int nowPoint = 0;
    public bool isAttack = false;
    // Start is called before the first frame update
    void Start()
    {
        My_Enemy = this.transform.GetChild(0).transform;
        animator=this.transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!isAttack)
        {
            if (!animator.GetBool("isMove"))
                animator.SetBool("isMove", true);
            print(CatchPlayer());
            if (!CatchPlayer())
            {
                Lookat(Points[nowPoint].position);
                Patrol();
            }
            else
            {
                Debug.Log("Is Catch Player");
                Lookat(Player.position);
                if (Vector2.Distance(My_Enemy.transform.position, Player.transform.position) < 1.55f)
                {
                    isAttack = true;
                    animator.SetTrigger("Attack");
                }
                else
                    TrackPlayer();
            }
        }
        else
            animator.SetBool("isMove", false);

    }

    void TrackPlayer()
    {
        if(Player != null)
        {
            if(CatchPlayer())
            {
                My_Enemy.position = Vector2.MoveTowards(My_Enemy.position, Player.position, MoveSpeed * Time.deltaTime);
            }
        }
    }

    void Patrol()
    {
        if (Vector2.Distance(My_Enemy.position, Points[nowPoint].position) < 0.1f)
        {
            nowPoint++;
            if (nowPoint > Points.Length - 1)
            {
                nowPoint = 0;
            }
        }
        My_Enemy.position = Vector2.MoveTowards(My_Enemy.position, Points[nowPoint].position, MoveSpeed * Time.deltaTime);
    }

    bool CatchPlayer()
    {
        float width = Mathf.Abs(Points[0].transform.position.x - Points[1].transform.position.x);
        center = (Points[0].transform.position + Points[1].transform.position) / 2f;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(center, new Vector2(width, height),5f);
        //print(colliders.Length);
        if (colliders.Length != 0)
            foreach (Collider2D collider in colliders)
            {

                if (collider.tag == "Player")
                {
                    Player = collider.transform;
                    return true;
                }
            }
        return false;
    }

    void Lookat(Vector2 Point)
    {
        CatchPlayer();
        if (Point.x < My_Enemy.position.x)
            My_Enemy.transform.localScale = new Vector3(7f, 7f, 7f);
        else
            My_Enemy.transform.localScale = new Vector3(-7f, 7f, 7f);
    }

    private void OnDrawGizmos()
    {
        float width = Mathf.Abs( Points[0].transform.position.x -  Points[1].transform.position.x);
        center =( Points[0].transform.position + Points[1].transform.position)/ 2f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center,new Vector2(width, height));
    }
}
