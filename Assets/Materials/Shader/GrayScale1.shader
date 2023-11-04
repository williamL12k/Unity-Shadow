Shader "Unlit/GrayScale1"
{
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _grayColor("color",Color)= (1,1,1,1)//�Զ���Ҷ���ɫ
    }
    SubShader {
        Pass {  
ZTest Always
Cull Off
ZWrite Off
            
            CGPROGRAM  
            #pragma vertex vert  
            #pragma fragment frag  
            
#include "UnityCG.cginc"  
 
sampler2D _MainTex;
float3 _grayColor;
            
struct v2f
{
    float4 pos : SV_POSITION;
    half2 uv : TEXCOORD0;
};
            
v2f vert(appdata_img v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord;
    return o;
}
            
fixed4 frag(v2f i) : SV_Target
{
    fixed4 renderTex = tex2D(_MainTex, i.uv);
    float gray = dot(renderTex.rgb, _grayColor); //�ûҺ�Ļ�ɫֵ
    fixed4 targetColor = fixed4(gray, gray, gray, renderTex.a); //ֻ���ܻҶ�rgb��alphaʹ��Դͼ��ֵ
    fixed4 finalColor = lerp(renderTex, targetColor, 1); //��ԭ������ɫ���ɫ����ֵ����
    return finalColor;
}
            ENDCG
        }  
    }
Fallback Off
}
