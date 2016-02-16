Shader "Hidden/ShaderToy/CirclePattern"
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

#define NUM 9.0

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

	float hash1(float n)
	{
		return frac(sin(n)*43758.5453);
	}

	float2  hash2(float2  p)
	{
		p = float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)));
		return frac(sin(p)*43758.5453);
	}

	float4 voronoi(in float2 x, float mode)
	{
		float2 n = floor(x);
		float2 f = frac(x);

		float3 m = float3(8.0, 8.0, 8.0);
		float m2 = 8.0;
		for (int j = -2; j <= 2; j++)
			for (int i = -2; i <= 2; i++)
			{
				float2 g = float2(float(i), float(j));
				float2 o = hash2(n + g);

				// animate
				o = 0.5 + 0.5*sin(_Time*5.0 + 6.2831*o);

				float2 r = g - f + o;

				// euclidean		
				float2 d0 = float2(sqrt(dot(r, r)), 1.0);
				// manhattam		
				float2 d1 = float2(0.71*(abs(r.x) + abs(r.y)), 1.0);
				// triangular		
				float2 d2 = float2(max(abs(r.x)*0.866025 + r.y*0.5, -r.y),
				step(0.0, 0.5*abs(r.x) + 0.866025*r.y)*(1.0 + step(0.0, r.x)));

				float2 d = d0;
				if (mode<3.0) d = lerp(d2, d0, frac(mode));
				if (mode<2.0) d = lerp(d1, d2, frac(mode));
				if (mode<1.0) d = lerp(d0, d1, frac(mode));

				if (d.x<m.x)
				{
					m2 = m.x;
					m.x = d.x;
					m.y = hash1(dot(n + g, float2(7.0, 113.0)));
					m.z = d.y;
				}
				else if (d.x<m2)
				{
					m2 = d.x;
				}

			}
		return float4(m, m2 - m.x);
	}

	fixed3 frag(v2f i) : SV_Target{

		float mode = fmod((_Time / 5.0)*40.0,3.0);
		mode = floor(mode) + smoothstep(0.8, 1.0, frac(mode));

		float2 p = (-1.0 + 2.0*i.position.xy / _ScreenParams.xy);
		p = float2(p.x + 0.01, p.y + -0.3);
		
		float4 c = voronoi(float2(p.x*10.0, p.y*10.0), mode);

		//float3 col = float3(0.0, 0.0, 0.0);
		float3 col = 0.5 + 0.5*sin(c.y*2.5 + float3(1.9, 1.7, 1.0));
		col *= sqrt(clamp(1.0 - c.x, 0.0, 1.0));
		col *= clamp(0.5 + (1.0 - c.z / 2.0)*0.5, 0.0, 1.0);
		col *= 0.4 + 0.6*sqrt(clamp(4.0*c.w, 0.0, 1.0));

		return col;
	}

		ENDCG
	}
	}
}