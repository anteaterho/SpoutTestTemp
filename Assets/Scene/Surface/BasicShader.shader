Shader "Hidden/ShaderToy/TextureRefractionExample"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader
	{
		Pass
	{
		CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag

#include "UnityCG.cginc"

	uniform sampler2D _MainTex;
	uniform float _Speed;

	float4 frag(v2f_img i) : COLOR
	{
		float twoPi = 6.283185307179586;

	float2 uv = i.uv.xy;

	float uvMod = 0.8;
	uv.y = (uv.y * uvMod) + (1 - uvMod) / 2;

	// refract
	uv.y = uv.y + (1 - uvMod) / 2 * sin(uv.y * twoPi + _Time.y * _Speed);

	return tex2D(_MainTex, uv);
	}
		ENDCG
	}
	}
}