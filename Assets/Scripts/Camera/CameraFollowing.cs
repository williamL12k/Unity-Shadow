using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    Vector3 distance;
    public GameObject Obj;
    public Transform pointsGather;
    [SerializeField]
    Transform[] points;
    public float smoothSpeed=0.5f;
    // Start is called before the first frame update
    void Start()
    {
        distance = this.transform.position - Obj.transform.position;
        for(int i=0;i<pointsGather.childCount;i++) 
        {
            points[i] = pointsGather.GetChild(i).transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        smoothTransform(SetTargetPos(distance + Obj.transform.position));
    }

    Vector3 SetTargetPos(Vector3 targetPos)
    {
       // print(targetPos);
        Vector3 pos = Vector3.zero;

        float clampedX = Mathf.Clamp(targetPos.x, Mathf.Min(points[0].position.x, points[1].position.x, points[2].position.x, points[3].position.x), 
            Mathf.Max(points[0].position.x, points[1].position.x, points[2].position.x, points[3].position.x));
        float clampedY = Mathf.Clamp(targetPos.y, Mathf.Min(points[0].position.y, points[1].position.y, points[2].position.y, points[3].position.y),
            Mathf.Max(points[0].position.y, points[1].position.y, points[2].position.y, points[3].position.y));

        pos = new Vector3(clampedX, clampedY, -5);
        return pos;
    }

    void smoothTransform(Vector3 targetPos)
    {
       // print(targetPos);
        //print(Vector3.Distance(this.transform.position, targetPos));
        if(Vector3.Distance(this.transform.position,targetPos)>0.01f)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, targetPos, smoothSpeed);

        }
    }
}
