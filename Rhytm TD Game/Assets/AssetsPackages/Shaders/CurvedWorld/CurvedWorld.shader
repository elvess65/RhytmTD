﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/CurvedWorld" {
    Properties {
        // Diffuse texture
        _MainTex ("Base (RGB)", 2D) = "white" {}
        // Degree of curvature
        _Curvature ("Curvature", Float) = 0.001
        _CameraOffset ("Offset", Float) = 0.0

        _LineCenter ("LineCenter", Float) = 0.0
        _LineWidth ("LineWidth", Float) = 0.0
        _LineColor ("Line Color", Color) = (1.0, 0.0, 1.0)
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
 
        CGPROGRAM
        // Surface shader function is called surf, and vertex preprocessor function is called vert
        // addshadow used to add shadow collector and caster passes following vertex modification
        #pragma surface surf Lambert vertex:vert addshadow
 
        // Access the shaderlab properties

        uniform sampler2D _MainTex;
        uniform float _Curvature;
        uniform float _CameraOffset;
        uniform float _LineCenter;
        uniform float _LineWidth;
        uniform float4 _LineColor;
 
        // Basic input structure to the shader function
        // requires only a single set of UV texture mapping coordinates

        struct Input {
            float2 uv_MainTex;
        };

        float4 CurveVertex(float4 v)
        {
            // Transform the vertex coordinates from model space into world space
            float4 vv = mul( unity_ObjectToWorld, v );
 
            // Now adjust the coordinates to be relative to the camera position
            vv.xyz -= _WorldSpaceCameraPos.xyz - _CameraOffset;
 
            // Reduce the y coordinate (i.e. lower the "height") of each vertex based
            // on the square of the distance from the camera in the z axis, multiplied
            // by the chosen curvature factor

            //float curvature = ((vv.z * vv.z) + (vv.x * vv.x)) * - _Curvature / 100.0;
            float curvature = (vv.z * vv.z) * - _Curvature / 100.0f;

            vv = float4( 0.0f, curvature, 0.0f, 0.0f );
 
            // Now apply the offset back to the vertices in model space
            v += mul(unity_WorldToObject, vv);

            return v;
        }

        float3 PlotRoad(float uvX)
        {
            float3 lineBGColor = float3(0.0, 0.0, 0.0);
            float lineCenter = _LineCenter / 10.0;

            float y = smoothstep(lineCenter - _LineWidth, lineCenter, uvX) - smoothstep(lineCenter, lineCenter + _LineWidth, uvX);
            return (1.0 - y) * lineBGColor + y * _LineColor;
        }

        void vert( inout appdata_full v)
        {
            v.vertex = CurveVertex(v.vertex);
        }
 
        // This is just a default surface shader
        void surf (Input IN, inout SurfaceOutput o)
{
            half4 c = tex2D (_MainTex, IN.uv_MainTex);

            float3 resultLineColor = PlotRoad(IN.uv_MainTex.x);

            o.Albedo = c.rgb + resultLineColor;
            o.Alpha = c.a;
        }
        ENDCG
    }
    // FallBack "Diffuse"
}