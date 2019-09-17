Shader "Custom/foodShader"
{
	Properties
	{
		_Albedo("Albedo (RGB), Alpha (A)", 2D) = "white" {}
		
		_FriedAlbedo("Fried (RGB),Alpha(A)",2D) = "white"{}

		_BoiledAlbedo("Boiled(RGB),Alpha(A)",2D) = "white"{}

		[NoScaleOffset]_Metallic("Metallic (R), Occlusion (G), Emission (B), Smoothness (A)", 2D) = "black" {}
		
		[NoScaleOffset]_Normal("Normal (RGB)", 2D) = "bump" {}

		[PerRendererData]_Friedness("Friedness",Range(0,1.0)) = 0.0

		[PerRendererData]_Boiledness("Cookness",Range(0,1.0)) = 0.0

		[PerRendererData]_Burntness("Burntness",Range(0,1.0)) = 0.0
	}

		SubShader
	{
		Tags
		{
			"Queue" = "Geometry"
			"RenderType" = "Opaque"
		}

		CGINCLUDE
		#define _GLOSSYENV 1
		ENDCG

		CGPROGRAM
		#pragma target 3.0
		#include "UnityPBSLighting.cginc"
		#pragma surface surf Standard
		#pragma exclude_renderers gles

		struct Input
		{
			float2 uv_Albedo;
		};

		sampler2D _Albedo,_FriedAlbedo,_BoiledAlbedo;
		sampler2D _Normal;
		sampler2D _Metallic;
		float _Friedness, _Boiledness, _Burntness;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 albedo = tex2D(_Albedo, IN.uv_Albedo);

			albedo = lerp(albedo, tex2D(_FriedAlbedo, IN.uv_Albedo), _Friedness);

			albedo = lerp(albedo, tex2D(_BoiledAlbedo, IN.uv_Albedo), _Boiledness);


			//get distance to center

			float diffToCenter = abs(0.5f - _Burntness);


			float BurnColor = 1 - _Burntness;

	
			
			
			float4 black = float4(BurnColor, BurnColor, BurnColor, BurnColor);
			albedo = albedo *black;

			fixed4 metallic = tex2D(_Metallic, IN.uv_Albedo);
			fixed3 normal = UnpackScaleNormal(tex2D(_Normal, IN.uv_Albedo), 1);

			o.Albedo = albedo.rgb;
			o.Alpha = albedo.a;
			o.Normal = normal;
			o.Smoothness = metallic.a;
			o.Metallic = metallic.r;
			o.Occlusion = metallic.g;
			o.Emission = metallic.b;

		}
		ENDCG
	}

		FallBack "Diffuse"
}
