// Custom Inputs are X = Pan Offset, Y = UV Warp Strength, Z = Gravity
// Specular Alpha is used like a metalness control. High values are more like dielectrics, low are more like metals
// Subshader at the bottom is for Shader Model 2.0 and OpenGL ES 2.0 devices

Shader "Particles/Simplified FXVille Blood 2018"
{
	Properties
	{
		[Header (Color Controls)]
		[HDR] _BaseColor ("Base Color Mult", Color) = (1,1,1,1)
		_LightStr ("Lighting Strength", float) = 1.0
		_AlphaMin ("Alpha Clip Min", Range (-0.01, 1.01)) = 0.1
		_AlphaSoft ("Alpha Clip Softness", Range (0,1)) = 0.1
		_EdgeDarken ("Edge Darkening", float) = 1.0
		_ProcMask ("Procedural Mask Strength", float) = 1.0

		[Header (Mask Controls)]
		_MainTex ("Mask Texture", 2D) = "white" {}
		_MaskStr ("Mask Strength", float) = 0.7
		_Columns ("Flipbook Columns", Int) = 1
		_Rows ("Flipbook Rows", Int) = 1
		_ChannelMask ("Channel Mask", Vector) = (1,1,1,0)
		[Toggle] _FlipU("Flip U Randomly", float) = 0
		[Toggle] _FlipV("Flip V Randomly", float) = 0

		[Header (Noise Controls)]
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_NoiseAlphaStr ("Noise Strength", float) = 1.0
		_ChannelMask2 ("Channel Mask",Vector) = (1,1,1,0)
		_Randomize ("Randomize Noise", float) = 1.0

		[Header (UV Warp Controls)]
		_WarpTex ("Warp Texture", 2D) = "gray" {}
		_WarpStr ("Warp Strength", float) = 0.2

		[Header (Vertex Physics)]
		_FallOffset ("Gravity Offset", range(-1,0)) = -0.5
		_FallRandomness ("Gravity Randomness", float) = 0.25

		//specular stuff//
		[HDR] _SpecularColor ("Reflection Color Mult", Color) = (1,1,1,0.5)
		_ReflectionTex ("Reflection Texture", 2D) = "black" {}
		_ReflectionSat ("Reflection Saturation", float) = 0.5
		[NoScaleOffset] [Normal] _Normal ("Reflection Normalmap", 2D) = "bump" {}
		_FlattenNormal ("Flatten Reflection Normal", float) = 2.0 

	}
	Category {

		Tags 
		{
			"IgnoreProjector"="True"
			"Queue"="Transparent"
			"RenderType"="Transparent"
			"LightMode"="UniversalForward" 
		}

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off

		SubShader {
			Pass {
			
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				
				#include "UnityCG.cginc"

				/// Properties ///
				/// Color Controls ///
				fixed4 _BaseColor;
				fixed _AlphaMin;
				fixed _AlphaSoft;
				fixed _EdgeDarken;
				fixed _ProcMask;

				/// Mask Controls ///
				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed _MaskStr;
				fixed4 _ChannelMask;
				fixed _Columns;
				fixed _Rows;
				fixed _FlipU;
				fixed _FlipV;

				/// Noise Controls ///
				sampler2D _NoiseTex;
				float4 _NoiseTex_ST;
				fixed _NoiseAlphaStr;
				fixed _NoiseColorStr;
				fixed4 _ChannelMask2;
				fixed _Randomize;

				struct appdata_t 
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 color : COLOR;
					float4 texcoord0 : TEXCOORD0; // Z is Random, W is Lifetime
					UNITY_VERTEX_INPUT_INSTANCE_ID

				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					UNITY_VERTEX_OUTPUT_STEREO
				};


				v2f vert (appdata_t v)
				{
					v2f o;

					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

					o.vertex = UnityObjectToClipPos(v.vertex);

					float2 UVflip = round(frac(float2(v.texcoord0.z * 13, v.texcoord0.z * 8))); 	//random 0 or 1 in x and y
					UVflip = UVflip * 2 - 1; 														//random -1 or 1 in x and y
					UVflip = lerp(1, UVflip, float2(_FlipU, _FlipV));
					
					// o.uv.xy is original UVs, o.uv.zw is randomized and panned //
					o.uv.xy = TRANSFORM_TEX(v.texcoord0.xy * UVflip, _MainTex);
					o.uv.zw = o.uv.xy * half2(_Columns, _Rows) + v.texcoord0.z * half2(3,8) * _Randomize;
					o.uv.zw *= _NoiseTex_ST.xy;
					o.uv.zw += _NoiseTex_ST.zw * v.texcoord0.w;

					o.color = v.color;
					o.color.a += _AlphaMin;

					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}
				
				fixed4 frag (v2f i) : SV_Target
				{
					
					fixed4 col = i.color;
					
					// ////// Sample The Mask //////
					fixed4 mask = tex2D(_MainTex, i.uv.xy);
					mask = saturate(lerp(1, mask, _MaskStr));

					////// Make And Edge Mask So Nothing Spills Off The Quad //////
					half2 tempUV = frac(i.uv.xy * half2(_Columns, _Rows)) - 0.5;
					tempUV *= tempUV * 4;
					half edgeMask = saturate(tempUV.x + tempUV.y);
					edgeMask *= edgeMask;
					edgeMask = 1- edgeMask;
					edgeMask = lerp(1.0,  edgeMask, _ProcMask);
					mask *= edgeMask;
					
					col.a *= saturate(dot(mask, _ChannelMask));

					
					////// Sample The Noise //////
					half4 noise4 = tex2D(_NoiseTex, i.uv.zw);
					half noise = dot(noise4, _ChannelMask2);
					noise = saturate(lerp(1,noise,_NoiseAlphaStr));

					////// Alpha Clip //////
					col.a *= noise * i.color.a;
					half preClipAlpha = col.a;
					half clippedAlpha =  saturate((preClipAlpha * i.color.a - _AlphaMin)/(_AlphaSoft));
					col.a = clippedAlpha;
					preClipAlpha = lerp(0.5, (min(preClipAlpha * 0.9 + 0.1,1.0)) * clippedAlpha, _EdgeDarken);
					col.xyz *= preClipAlpha * _BaseColor;

					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
		CustomEditor "SpecularToggleEditor"
	}
}
