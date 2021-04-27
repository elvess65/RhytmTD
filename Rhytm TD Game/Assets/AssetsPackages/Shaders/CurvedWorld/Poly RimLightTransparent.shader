Shader "Custom/Poly RimLightTransparent"
{
	Properties
	{
		_InnerColor("Inner Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimWidth("Rim Width", Range(0.2,20.0)) = 3.0
		_RimGlow("Rim Glow Multiplier", Range(0.0,9.0)) = 1.0

		_Curvature("Curvature", Float) = 0.3
		_CameraOffset("Camera Offset", Float) = 0.0
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

		Cull Back
		Lighting Off
		Blend One One

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert

		#include "CurveVertex.hlsl"

		struct Input
		{
			float3 viewDir;
		};

		float4 _InnerColor;
		float4 _RimColor;
		float _RimWidth;
		float _RimGlow;

		void vert(inout appdata_full v)
		{
			v.vertex = CurveVertex(v.vertex);
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = _InnerColor.rgb;
			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Emission = _RimColor.rgb * _RimGlow * pow(rim, _RimWidth);
		}

		ENDCG
	}

	Fallback "Diffuse"
}