Shader "Custom/studentShader"
{
	Properties
	{
		_Albedo("Albedo (RGB), Alpha (A)", 2D) = "white" {}

		_AngeryColor("AngeryColor",Color) = (1,1,1,1)

		[NoScaleOffset]_Metallic("Metallic (R), Occlusion (G), Emission (B), Smoothness (A)", 2D) = "black" {}

		[NoScaleOffset]_Normal("Normal (RGB)", 2D) = "bump" {}

		[PerRendererData]_Angeryness("angeryness",Range(0,1)) = 0.0
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

			sampler2D _Albedo;
			sampler2D _Normal;
			sampler2D _Metallic;
			fixed4 _AngeryColor;
			float _Angeryness;

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				fixed4 albedo = tex2D(_Albedo, IN.uv_Albedo) ;

				albedo = lerp(albedo, _AngeryColor, _Angeryness);

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
