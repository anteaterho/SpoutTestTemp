Shader "Hidden/ShaderToy/Heart"
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

	struct v2f {
		float4 position : SV_POSITION;
	};

	v2f vert(float4 v:POSITION) : SV_POSITION{
		v2f o;
	o.position = mul(UNITY_MATRIX_MVP, v);
	return o;
	}

	uniform sampler2D _MainTex;
	uniform float _Speed;

	fixed3 frag(v2f i) : SV_Target{

		/*
		float2 uv = -1.0 + 2.0*i.position.xy / _ScreenParams.xy;
		uv.x *= _ScreenParams.x / _ScreenParams.y;
		*/
		//float2 p = (2*i.position.xy - i.position.xy) / min(_ScreenParams.x,_ScreenParams.y);
		float2 p = (-1.0 + 2.0*i.position.xy / _ScreenParams.xy);
		p = float2(p.x + 0.01, p.y + -0.3);
		// Background
		fixed3 bcol = fixed3(0.0, 0.8, 0.7 - 0.07*p.y)*(1.0 - 0.25*length(p));

		//animate
		float tt = fmod(_Time*20.0, 1.5) / 1.5;
		float ss = pow(tt, .2)*0.5 + 0.5;
		ss = 1.0 + ss*0.5*sin(tt*6.2831*3.0 + p.y*0.5)*exp(-tt*4.0);
		p *= float2(0.5, 1.5) + ss*float2(0.5, -0.5);

		// shape
		float a = atan2(p.x, p.y) / 3.141593;
		float r = length(p);
		float h = abs(a);
		float d = (13.0*h - 22.0*h*h + 10.0*h*h*h) / (6.0 - 5.0*h);

		// color
		float s = 1.0 - 0.5*clamp(r / d, 0.0, 1.0);
		s = 0.75 + 0.75*p.x;
		s *= 1.0 - 0.25*r;
		s = 0.5 + 0.6*s;
		s *= 0.5 + 0.5*pow(1.0 - clamp(r / d, 0.0, 1.0), 0.1);
		float3 hcol = float3(1.0, 0.5*r, 0.3)*s;

		float3 col = lerp(bcol, hcol, smoothstep(-0.01, 0.01, d - r));
	

		return col;
	}

		ENDCG
	}
	}
}