using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public static float speed = 2f;

    [Header("Other Attribute")]
    public GameObject Camera;
    public float jumpForce = 5f;
    public int MaxJumpCount = 2;
    [SerializeField]
    float direction;
    [SerializeField]
    int attackID = 0;
    [SerializeField]
    bool isAttack = false;

    [Header("Shake Attribute")]
    [SerializeField]
    float shake_Durtion;
    [SerializeField]
    float shake_Strong;

    Animator animator;
    Rigidbody2D rigidbody2;
    float offets = 1.102f;
    int reversalCtrl = 1;
    int jumpCount = 0;



    [Header("Test")]
    //[SerializeField]
    //bool HitWall = false;
    [SerializeField]
    bool jumping = false;
    [SerializeField]
    bool climbing = false;
    [SerializeField]
    bool onFloor_replenish = false;
    [SerializeField]
    bool onWall_replenish = false;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        if (Camera == null)
            Camera = GameObject.FindGameObjectWithTag("MainCamera");
        animator = transform.GetChild(0).GetComponent<Animator>();
    }



    void Update()
    {
        direction = Input.GetAxis("Horizontal");

        //if (direction != 0.0)
        if(!isAttack||!onWall_replenish&&!onFloor_replenish)
        Move(direction);

        Jump(!onWall_replenish || onWall_replenish && onFloor_replenish);

        if (Input.GetKeyDown(KeyCode.I))
            StartCoroutine(Reversal());


        if(Input.GetMouseButtonDown(0))
            Attack();

        if (AnimationScripts.An_Instance.an_Finish)
        {
            print("finish");
            attackID = 0;
            SetAnimator("Attack", attackID);
            AnimationScripts.An_Instance.an_Finish = false;
            isAttack = false;

        }

        //
        //测试
        //..............................................................................................
        if (Input.GetKeyDown(KeyCode.T))
        {
            OffetsFinding(-transform.up*reversalCtrl, "Floor");
            OffetsFinding(transform.right * transform.localScale.x, "Wall");

                Vector2 currentVelocity = rigidbody2.velocity;
                rigidbody2.AddForce(-currentVelocity*2, ForceMode2D.Impulse);
            //rigidbody2.drag = 30f;
            //Shake_Tool.Instance.CameraShake(shake_Durtion, shake_Strong);
        }
        //.............................................................................................
        //else

        //if (jumping)
        //{
    }

    private void FixedUpdate()
    {
        onFloor_replenish = HitDetecter(-transform.up * reversalCtrl, 1.111f, "Floor");

        if (!jumping)
            onWall_replenish = HitDetecter(transform.right * this.transform.localScale.x, 0.811f, "Wall");
        //}

        if (onWall_replenish && !onFloor_replenish)
            SetAttribute("Onwall", false, true, false, 50f);
        else if (!onFloor_replenish)
            SetAttribute("Onwall", false, false, false, 0f);

        if (onFloor_replenish)
            SetAttribute("Onfloor", true, false, false, 0f);
        else if (!onWall_replenish)
            SetAttribute("Onfloor", false, false, false, 0f);
    }

    void Attack()
    {
        if (!isAttack)
        {
            isAttack = true;
            attackID = 1;
        }
        print("attime    "+AnimationScripts.An_Instance.atTime);
        print("finish    " + AnimationScripts.An_Instance.an_Finish);

        if (AnimationScripts.An_Instance.atTime)
        {

            AnimationScripts.An_Instance.atTime = false;
            attackID++;
        }
        
      

        SetAnimator("Attack", attackID);
    }



    void Move(float dir)
    {
        float in_speed = speed;
        SetAnimator("Speed", Mathf.Abs(dir));
        if (!Mathf.Approximately(dir, 0f))
            this.transform.localScale = new Vector3(dir < 0.0 ? -1 : 1, reversalCtrl, 1);

        if (dir != 0.0 && onWall_replenish)
            if (!HitDetecter(transform.right * this.transform.localScale.x, 0.811f, "Wall"))
                onWall_replenish = false;
        if (onWall_replenish)
        {
            if (dir != 0.0)
            {
                dir = Mathf.Clamp(dir, Mathf.Min(-this.transform.localScale.x, 0f), Mathf.Max(-transform.localScale.x, 0f));
                //print("Dir      " + dir);
                //rigidbody2.velocity = new Vector2((rigidbody2.velocity.x + 1)*dir * transform.localScale.x, (dir+1) * reversalCtrl);
               

            }
        }
        if (!onFloor_replenish && !onWall_replenish)
            in_speed *= 0.8f;

        this.transform.Translate(Vector3.right * in_speed * dir * 0.005f);
    }

    void Jump(bool type)
    {

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < MaxJumpCount)
        {
            Vector2 currentVelocity = rigidbody2.velocity;
            rigidbody2.AddForce(-currentVelocity * 2, ForceMode2D.Impulse);
            //地上的正常跳跃
            if (type)
            {
                rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, jumpForce * reversalCtrl);
                // SetAttribute("Onfloor", false, false, false, 0f);
                jumpCount++;
            }
            else
            {
                jumping = true;
                rigidbody2.velocity = new Vector2(-jumpForce  * transform.localScale.x/2f, (rigidbody2.velocity.y+reversalCtrl) *10f  /** reversalCtrl*/);
                onWall_replenish = false;
                onWall_replenish = false;
                //        print(new Vector2(-jumpForce * direction, rigidbody2.velocity.y));
            }
            // rigidbody2.velocity = new Vector2((rigidbody2.velocity.x + 1) * -2f * transform.localScale.x, jumpForce * reversalCtrl);
            //jumpCount++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wall")
        {
            onWall_replenish = true;
            jumping = false;
            jumpCount = 0;
        }
        if (collision.collider.tag == "Floor")
        {
            jumpCount = 0;
            onFloor_replenish = true;
        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{

    //    if (collision.collider.tag == "Wall")
    //    {
    //        rigidbody2.velocity = new Vector2(-jumpForce * direction, rigidbody2.velocity.y);
    //        print(new Vector2(-jumpForce * direction, rigidbody2.velocity.y));
    //    }
    //}

    void Reversal_ChangePos()
    {
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = transform.up * reversalCtrl; // 假设射线向右发射
        //Debug.Log("doing");

        // 创建射线
        RaycastHit2D[] hit = Physics2D.RaycastAll(rayOrigin, rayDirection, 100f);

        foreach (RaycastHit2D h in hit)
        {
            // 检测射线与物体的交点
            if (h.collider.tag == "Floor")
            {
                // 计算物体与射线起点的距离
                this.transform.position = new Vector2(h.point.x, h.point.y - (offets * reversalCtrl));

            }
        }
    }

    IEnumerator Reversal()
    {
        TeilMap_B_Controller.Type = !TeilMap_B_Controller.Type;
        // Shake_Tool.Instance.CameraShake(shake_Durtion, shake_Strong);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.02f);
        Camera.GetComponent<M_Camera>().enabled = !Camera.GetComponent<M_Camera>().enabled;
        Reversal_ChangePos();
        Time.timeScale = 1;
        Shake_Tool.Instance.CameraShake(shake_Durtion, shake_Strong);
        rigidbody2.gravityScale *= -1;
        this.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
        reversalCtrl *= -1;

    }



    void SetAttribute(string introduce, bool onFloor_Num,bool isOnWall_Num,bool climbing_Num,float drage_Num)
    {
        climbing = climbing_Num;
        rigidbody2.drag = drage_Num;
        //Debug.Log(introduce);
        SetAnimator("onFloor", onFloor_Num);
        SetAnimator("isOnWall", isOnWall_Num);
    }

    /// <Animator>
    /// bool Running
    /// int Attack
    /// bool isOnWall
    /// bool isClambing
    /// bool onFloor
    /// bool kill       (Death)
    /// float Speed     (MoveSpeed)
    /// </Animator>

    void SetAnimator(string name, float num)
    {
        switch (name)
        {
            case "Attack": animator.SetInteger(name, (int)num); break;
            case "Speed": animator.SetFloat(name, num); break;
                default:print("Number_Error"); break;
        }
    }

    void SetAnimator(string name, bool num)
    {
        switch(name)
        {
            case "Running": animator.SetBool(name, num);break;
            case "isOnWall": animator.SetBool(name, num); break;
            case "isClambing": animator.SetBool(name, num); break;
            case "onFloor": animator.SetBool(name, num); break;
            case "kill": animator.SetBool(name, num); break;
            default: print("Bool_Error"); break;
        }
    }



    //看人物的点到地面距离多少距离    (辅助帮助 OncolliderEnter 测试是否在地面)
    void OffetsFinding(Vector3 direction, string Tag)
    {
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = direction; // 假设射线向右发射

        // 创建射线
        RaycastHit2D[] hit = Physics2D.RaycastAll(rayOrigin, rayDirection, 100f);

        foreach (RaycastHit2D h in hit)
        {
            // 检测射线与物体的交点
            if (h.collider.tag == Tag)
            {
                // 计算物体与射线起点的距离
                float distance = Vector2.Distance(rayOrigin, h.point);
                Debug.Log(Tag + "  " + "距离：" + distance);
                return;
            }
        }
    }

    //Ground 1.111
    //Wall 0.581

    bool HitDetecter(Vector3 direction, float offets,string Tag)
    {
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = direction; // 假设射线向右发射

        // 创建射线
        RaycastHit2D[] hit = Physics2D.RaycastAll(rayOrigin, rayDirection, 100f);

        foreach (RaycastHit2D h in hit)
        {
            // 检测射线与物体的交点
            if (h.collider.tag == Tag)
            {
                //在墙上或者在地上
                float distance = Vector2.Distance(rayOrigin, h.point);
                // 计算物体与射线起点的距离
                // Debug.Log("距离：" + distance);
                if (distance <= offets)
                {
                    if (Tag == "Wall")
                    {
                        onWall_replenish = true;
                        jumping = false;
                        jumpCount = 0;
                    }
                    jumping = false;
                    return true;
                }
            }
        }
        return false;
    }
}
