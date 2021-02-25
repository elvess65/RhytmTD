Shader "Custom/UnlitSphereDissolveCutout"
{
	Properties
	{
		_Position("Position", Vector) = (0,0,0,0)
		_Radius("Radius", Float) = 2
		[Toggle]_Invert("Invert", Float) = 0
		_Noisespeed("Noise speed", Vector) = (0,0,0,0)
		_Borderradius("Border radius", Range( 0 , 2)) = 1
		_Bordernoisescale("Border noise scale", Range( 0 , 20)) = 0
		[HDR]_Bordercolor("Border color", Color) = (0.8602941,0.2087478,0.2087478,0)
		[NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
		_Albedo_tint("Albedo_tint", Color) = (1,1,1,1)
		[NoScaleOffset][Normal]_Normal("Normal", 2D) = "bump" {}
		[NoScaleOffset]_Emission("Emission", 2D) = "black" {}
		_Tiling("Tiling", Vector) = (1,1,0,0)
		_Offset("Offset", Vector) = (0,0,0,0)
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "false"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf NoLighting noambient
 
		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			 return fixed4(s.Albedo, s.Alpha);
		}

		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _Normal;
		uniform float2 _Tiling;
		uniform float2 _Offset;
		uniform float4 _Albedo_tint;
		uniform sampler2D _Albedo;
		uniform float4 _Emission_tint;
		uniform sampler2D _Emission;
		uniform float4 _Bordercolor;
		uniform float3 _Position;
		uniform float _Bordernoisescale;
		uniform float3 _Noisespeed;
		uniform float _Radius;
		uniform float _Borderradius;
		uniform sampler2D _Metallic;
		uniform float _Metallic_multiplier;
		uniform float _Smoothness;
		uniform float _Invert;
		uniform float _Cutoff = 0.5;


		float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }

		float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }

		float snoise( float3 v )
		{
			const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
			float3 i = floor( v + dot( v, C.yyy ) );
			float3 x0 = v - i + dot( i, C.xxx );
			float3 g = step( x0.yzx, x0.xyz );
			float3 l = 1.0 - g;
			float3 i1 = min( g.xyz, l.zxy );
			float3 i2 = max( g.xyz, l.zxy );
			float3 x1 = x0 - i1 + C.xxx;
			float3 x2 = x0 - i2 + C.yyy;
			float3 x3 = x0 - 0.5;
			i = mod3D289( i);
			float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
			float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
			float4 x_ = floor( j / 7.0 );
			float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
			float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 h = 1.0 - abs( x ) - abs( y );
			float4 b0 = float4( x.xy, y.xy );
			float4 b1 = float4( x.zw, y.zw );
			float4 s0 = floor( b0 ) * 2.0 + 1.0;
			float4 s1 = floor( b1 ) * 2.0 + 1.0;
			float4 sh = -step( h, 0.0 );
			float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
			float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
			float3 g0 = float3( a0.xy, h.x );
			float3 g1 = float3( a0.zw, h.y );
			float3 g2 = float3( a1.xy, h.z );
			float3 g3 = float3( a1.zw, h.w );
			float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
			g0 *= norm.x;
			g1 *= norm.y;
			g2 *= norm.z;
			g3 *= norm.w;
			float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
			m = m* m;
			m = m* m;
			float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
			return 42.0 * dot( m, px);
		}


		void surf(Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord90 = i.uv_texcoord * _Tiling + _Offset;
			float2 UVs116 = uv_TexCoord90;
			float3 Normal112 = UnpackNormal( tex2D( _Normal, UVs116 ) );
			o.Normal = Normal112;
			float4 Albedo111 = ( _Albedo_tint * tex2D( _Albedo, UVs116 ) );
			o.Albedo = Albedo111.rgb;
			float3 ase_worldPos = i.worldPos;
			float temp_output_15_0 = distance( _Position , ase_worldPos );
			float simplePerlin3D26 = snoise( ( _Bordernoisescale * ( ase_worldPos + ( _Noisespeed * _Time.y ) ) ) );
			float temp_output_39_0 = ( simplePerlin3D26 + _Radius );
			float temp_output_5_0 = step( ( 1.0 - saturate( ( temp_output_15_0 / temp_output_39_0 ) ) ) , 0.5 );
			float temp_output_32_0 = saturate( ( temp_output_15_0 / ( _Borderradius + temp_output_39_0 ) ) );
			float Border49 = ( temp_output_5_0 - step( ( 1.0 - temp_output_32_0 ) , 0.5 ) );
			float4 Emission113 = ( ( _Emission_tint * tex2D( _Emission, UVs116 ) ) + ( _Bordercolor * Border49 ) );
			o.Emission = Emission113.rgb;
			float4 Metallic114 = ( tex2D( _Metallic, UVs116 ) * _Metallic_multiplier );
			o.Alpha = 1;
			float Mask51 = lerp(temp_output_5_0,step( temp_output_32_0 , 0.5 ),_Invert);
			clip( Mask51 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "AdultLink.SphereDissolveEditor_cutout"
}