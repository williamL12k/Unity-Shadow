using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeilMap_B_Controller : MonoBehaviour
{
    public static bool Type = true;
    bool type;
    public GameObject F_Ground;
    public GameObject B_Ground;
    private void Start()
    {
        type = Type;
    }
    void Update()
    {
        if (type != Type)
        {
            if (Type)
            {
                SetTypeGround("Floor", "Untagged");
            }
            else
            {
                SetTypeGround("Untagged", "Floor");
            }
        }
    }

    void SetTypeGround(string F_num, string B_num)
    {
        F_Ground.tag = F_num;
        B_Ground.tag = B_num;
        type = Type;
    }
}
