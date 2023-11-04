using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Camera : MonoBehaviour
{
    public Material curMaterial;

    //��Ļ������������shader����
    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        if (curMaterial != null)
        {
            Graphics.Blit(sourceTexture, destTexture, curMaterial);
        }
        else
        {
            Graphics.Blit(sourceTexture, destTexture);
        }
    }
}
