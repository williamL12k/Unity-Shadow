using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScripts : MonoBehaviour
{
    private static AnimationScripts an_instance;
    public static AnimationScripts An_Instance
    {
        get
        {
            if (an_instance == null)
                an_instance = Transform.FindObjectOfType<AnimationScripts>();
            return an_instance;
        }
    }

    public bool atTime = false;
    public bool an_Finish = false;

   

    void Finish01_isAttack()
    {
        this.transform.parent.GetComponent<Enemy01Controller>().isAttack = false;
    }

    void Finish02_canMove()
    {
        this.GetComponent<Animator>().SetBool("Attack", false);
        this.transform.GetComponent<AIPath>().maxSpeed = 1f;
        this.transform.parent.GetComponent<Enemy02_Controller>().endMoving = true;
        this.transform.parent.GetComponent<Enemy02_Controller>().hasCreat = false;

    }
    void SetatTimeTrue()
    {
        atTime = true;
    }

    void SetatTimeFalse()
    {
        atTime = false;
    }
    void AnimationFinish()
    {
        an_Finish = true;
    }
}
