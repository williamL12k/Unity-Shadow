using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake_Tool : MonoBehaviour
{
    private static Shake_Tool instance;
    public static Shake_Tool Instance
    {
        get 
        {
            if(instance == null)
                instance=Transform.FindObjectOfType<Shake_Tool>();
            return instance; 
        }
    }

    bool isShake = false;

    public void CameraShake(float durtion,float strong)
    {
        if(!isShake)
        {
            StartCoroutine(Shake(durtion, strong));
        }
    }

    public void HitPause(int durtion)
    {
        StartCoroutine(Pause(durtion));
    }

    IEnumerator Pause(int durtion)
    {
        float pauseTime = durtion / 60f;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }

    IEnumerator Shake(float durtion, float strong)
    {
        isShake = true;
        
        Vector3 startPosition= this.transform.position;
        while(durtion>0f)
        {
            this.transform.position = Random.insideUnitSphere * strong + startPosition;
            durtion -= Time.deltaTime;
            yield return null;
        }
        isShake=false;
    }
}
