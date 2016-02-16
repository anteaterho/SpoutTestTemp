Shader "Custom/MixSurfaceShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		
		_MainTex ("Albedo1 (RGB)", 2D) = "white" {}
		_MainTex1("Albedo2 (RGB)", 2D) = "white" {}
		_MainTex2("Albedo3 (RGB)", 2D) = "white" {}
		_MainTex3("Albedo3 (RGB)", 2D) = "white" {}

		_Alpha1("Alpha1", Range(0,1)) = 0.0
		_Alpha2("Alpha2", Range(0,1)) = 0.0
		_Alpha3("Alpha3", Range(0,1)) = 0.0

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex1;
		sampler2D _MainTex2;
		sampler2D _MainTex3;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex1;
			float2 uv_MainTex2;
			float2 uv_MainTex3;
		};

		half _Glossiness;
		half _Metallic;

		half _Alpha1;
		half _Alpha2;
		half _Alpha3;

		fixed4 _Color;

		fixed4 myLerp(fixed4 a, fixed4 b, float k)
		{
			return (1 - k)*a + b*k;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 c1 = tex2D(_MainTex1, IN.uv_MainTex1) * _Color;
			fixed4 c2 = tex2D(_MainTex2, IN.uv_MainTex2) * _Color;
			fixed4 c3 = tex2D(_MainTex3, IN.uv_MainTex3) * _Color;

			fixed4 r1 = myLerp(c, c1, _Alpha1);
			fixed4 r2 = myLerp(r1, c2, _Alpha2);
			fixed4 r3 = myLerp(r2, c3, _Alpha3);

			o.Albedo = r3.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
