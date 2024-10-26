Shader"Custom/BlankLargeTextureWithBlend"
{
    Properties
    {
        _MainTex1 ("Texture 1", 2D) = "white" {}  // 第一张贴图
        _MainTex2 ("Texture 2", 2D) = "white" {}  // 第二张贴图
        _MainTex3 ("Texture 3", 2D) = "white" {}  // 第三张贴图
        _MainTex4 ("Texture 4", 2D) = "white" {}  // 第四张贴图
        _BlendWidth ("Blend Width", Range(0, 0.5)) = 0.1  // 控制混合的宽度
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};

sampler2D _MainTex1; // 第一张贴图
sampler2D _MainTex2; // 第二张贴图
sampler2D _MainTex3; // 第三张贴图
sampler2D _MainTex4; // 第四张贴图
float _BlendWidth; // 混合区域的宽度

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    i.uv.x = i.uv.x * 2.075;
    i.uv.y = i.uv.y * 1.799;
    fixed4 col1 = tex2D(_MainTex1, i.uv - float2(0, 0.4)); // 采样第一张贴图
    fixed4 col2 = tex2D(_MainTex2, i.uv - float2(0.540, -0.01 + 0.4)); // 偏移采样第二张贴图
    fixed4 col3 = tex2D(_MainTex3, i.uv - float2(1.05, 0.025+0.4)); // 偏移采样第二张贴图
    fixed4 col4 = tex2D(_MainTex4, i.uv - float2(1.554, -0.010+0.4)); // 偏移采样第二张贴图
    fixed4 col5 = tex2D(_MainTex4, i.uv - float2(-0.52, -0.010 + 0.4)); // 偏移采样第二张贴图

    float blendFactor = 0.0;

                // 判断当前UV位置，混合接缝处
        if (i.uv.x > (1.0 - _BlendWidth) && i.uv.x < (1.5 - _BlendWidth))  // 接近右边界
        {
            blendFactor = smoothstep(1.0 - _BlendWidth, 0.8, i.uv.x);
            col1 = lerp(col1, col2, blendFactor); // 线性混合
        }
        else if (i.uv.x > (1.5 - _BlendWidth) && i.uv.x < (2 - _BlendWidth))
        {
            blendFactor = smoothstep(1.5 - _BlendWidth, 1.4, i.uv.x);
            col1 = lerp(col2, col3, blendFactor); // 线性混合
        }
        else if (i.uv.x > (2 - _BlendWidth))
        {
            blendFactor = smoothstep(2 - _BlendWidth, 1.8, i.uv.x);
            col1 = lerp(col3, col4, blendFactor); // 线性混合
        }
        else if (i.uv.x < (1.0 - _BlendWidth))
        {
            blendFactor = smoothstep(0.5 - _BlendWidth, 0.4, i.uv.x);
            col1 = lerp(col5, col1, blendFactor); // 线性混合
        }

        return col1; // 返回混合结果
}
            ENDCG
        }
    }
FallBack"Diffuse"
}
