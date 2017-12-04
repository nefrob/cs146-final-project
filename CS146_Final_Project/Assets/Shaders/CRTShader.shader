/*
* File:   CRT Scanline shader
* Author: Robert Neff
* Date:   10/28/17
*/

Shader "Custom/CRTShader"
{
	// Define shader properties
	Properties
	{
		// _name("unity editor name", type) = value

		// Screen texture to CRT split
	    _MainTex("Screen Texture - leave blank", 2D) = "white" {}
		// Texture to overlay screen with
		_OverlayTexture("Overlay Texture", 2D) = "white" {}
		// Color to add
		_Color ("Color", Color) = (0, 0, 0, 1)
		// CRT scanline size
		_LineSize("LineSize", Range(0, 10)) = 1
		// Y texture scroll speed
		_YScrollSpeed("Y-Scroll Speed", Range(-5, 5)) = 0
	}
	
	// Shader code
	SubShader
	{
		// No culling or depth
		Cull Off // cull back side
		ZWrite Off // write pixels to depth buffer
		ZTest Always // type of depth testing
		Blend SrcAlpha OneMinusSrcAlpha // blend mode

		// Shader passes, Overlay = 4000
		Tags { "IgnoreProjector" = "True" "Queue" = "Overlay" }

		// Diable any fog
		Fog { Mode Off }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			// Input variables
			sampler2D _MainTex;
			sampler2D _OverlayTexture;
			fixed4 _Color;
			half _LineSize;
			float _YScrollSpeed;

			// Vertex incoming data
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			// Vertex to fragment data
			struct v2f
			{
				float4 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			// Vertex shader
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = ComputeScreenPos(o.vertex);
				return o;
			}

			// Fragment shader
			fixed4 frag (v2f i) : SV_Target
			{
				// Drop every other pixel line
				fixed p = i.uv.y / i.uv.w;
				if ((int) (p * _ScreenParams.y / floor(_LineSize)) % 2 == 0) discard;
				
				// Apply texture overlay
				float2 movingUV = float2(i.uv.x, i.uv.y + (_YScrollSpeed *_Time.y));
				fixed4 col = tex2D(_OverlayTexture, movingUV);
				col *= _Color;
				col += tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}