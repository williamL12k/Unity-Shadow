using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class Enemy02_Controller : MonoBehaviour
{
    public AIDestinationSetter setter;
    public Transform pos;
    public bool endMoving = true;
    public Transform Ground;
    public Transform FirePoint;
    public GameObject Bullet;
    public Transform my_Enemy;
    [SerializeField]
    Transform Player;
    [SerializeField]
    float moveRange = 3f;
    //bool isCatchPlayer = false;
    List<Vector3> points = new List<Vector3>();
    [SerializeField]
    float findRange = 3f;
    public Animator animator;
    AnimatorStateInfo stateInfo;
    public bool hasCreat = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = my_Enemy.GetComponent<Animator>();
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        my_Enemy = this.transform.GetChild(0);
        setter = my_Enemy.GetComponent<AIDestinationSetter>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!stateInfo.IsName("Attack"))
        {
            if (!CatchPlayer())
            {
                patrol();
            }
            else
            {
                if (!WheatherOccluded())
                {
                    patrol();
                }
                else
                {
                    StartCoroutine( Lookat(Player.position, 5f, 0.2f));
                    my_Enemy.GetComponent<AIPath>().maxSpeed = 0f;

                    StartCoroutine(CreatBullet());


                }
            }
        }
        //if (!CatchPlayer())
        //{
        //    SetGroundPoint();
        //    if (endMoving)
        //    {
        //         endMoving = false;
        //        SetPos();
        //        if (pos != null)
        //        {
        //            setter.target = pos;
        //        }
        //    }
        //    if (Vector2.Distance(my_Enemy.position, pos.position) < 0.2f)
        //    {
        //        endMoving = true;
        //    }
        //    Lookat(pos.position, 5f);
        //}
        //else
        //{
        //    Lookat(Player.position, 5f);
        //    my_Enemy.GetComponent<AIPath>().maxSpeed = 0f;
        //    if (WheatherOccluded())
        //    {
        //        print("can hit");
        //        //animator.SetTrigger("Attack");
        //        //Invoke("CreatBullet", 0.5f);
        //    }
        //    else
        //    {

        //    }

       // }


        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    print("Test");
        //    GameObject obj = Instantiate(Bullet, this.transform, this.transform);
        //    obj.transform.position = FirePoint.position;
        //    Vector3 direction = Ground.position - transform.position;
        //    float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //    Quaternion rotation = Quaternion.Euler(-angle, 0, 0);

        //}
    }

    void patrol()
    {
        SetGroundPoint();
        if (endMoving)
        {
            endMoving = false;
            SetPos();
            if (pos != null)
            {
                setter.target = pos;
            }
        }
        if (Vector2.Distance(my_Enemy.position, pos.position) < 0.2f)
        {
            endMoving = true;
        }
        StartCoroutine( Lookat(pos.position, 5f,0.2f));
    }

    IEnumerator CreatBullet()
    {

        if (!hasCreat)
        {
            hasCreat = true;
            animator.SetBool("Attack", true);
            yield return new WaitForSeconds(0.6f);
            GameObject obj = Instantiate(Bullet, this.transform, this.transform);
            obj.transform.GetComponent<Bullet_Ctrol>().target = Player.position;
            obj.transform.position = FirePoint.position;
            Vector3 direction = Player.position - transform.position;
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(-angle, 0, 0);
        }
    }

    bool WheatherOccluded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Player.position - transform.position);
        if (hit.collider != null && hit.collider.gameObject != Player.gameObject)
        {
            Debug.Log("射线与Player之外的物体相交：" + hit.collider.gameObject.name);
            return false;
        }
        return true;
    }

    bool CatchPlayer()
    {
        float height = Mathf.Abs(my_Enemy.position.y + 5f - Ground.position.y);
        Vector2 center = (new Vector3(my_Enemy.position.x, my_Enemy.position.y + 5f, my_Enemy.position.z) + Ground.transform.position) / 2f;
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(center, new Vector2(findRange, height), 5f);
        foreach (Collider2D coll in collider2D)
        {
            if (coll.tag == "Player")
            {
                Player = coll.transform;
                return true;
            }
        }
        return false;
    }

    void SetPos()
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        GridNodeBase[] nodes = gridGraph.nodes;
        for (int j = 0; j < nodes.Length; j++)
        {
            if (Vector2.Distance((Vector3)nodes[j].position, my_Enemy.position) < moveRange)
            {
                if (nodes[j].Walkable)
                    points.Add((Vector3)nodes[j].position);
            }
        }
        //print(points.Count);
        int i = Random.Range(0, points.Count);
        pos.position = points[i];
        StartCoroutine(DelList());

        //**********************************************************************************************

        //int i = UnityEngine.Random.Range(0, nodes.Length);
        //if (!nodes[i].Walkable)
        //    SetPos();
        //else
        //    pos.position = (Vector3)nodes[i].position;
    }

    IEnumerator  Lookat(Vector2 Point, float range,float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (Point.x < my_Enemy.position.x)
            my_Enemy.transform.localScale = new Vector3(range, range, range);
        else
            my_Enemy.transform.localScale = new Vector3(-range, range, range);
    }

    void SetGroundPoint()
    {
        Ground.position = new Vector2(my_Enemy.position.x, Ground.position.y);
    }

    IEnumerator DelList()
    {
        points.Clear();
        yield return null;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(my_Enemy.position, findRange);
    //}

    private void OnDrawGizmos()
    {
        float height = Mathf.Abs(my_Enemy.position.y+5f - Ground.position.y);
        Vector2 center = (new Vector3(my_Enemy.position.x,my_Enemy.position.y+5f,my_Enemy.position.z) + Ground.transform.position) / 2f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, new Vector2(findRange, height));
    }
}
