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
		_ScreenTexture("Screen Texture - leave blank", 2D) = "white" {}
		// Texture to overlay screen with
		_OverlayTexture("OVerlay Texture", 2D) = "white" {}
		// Color to add
		_Color ("Color", Color) = (0, 0, 0, 1)
		// CRT scanline size
		_LineSize("LineSize", Range(0, 10)) = 1
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
		Tags{ "IgnoreProjector" = "True" "Queue" = "Overlay" }

		// Diable any fog
		Fog{ Mode Off }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
	
			#include "UnityCG.cginc"

			// Input variables
			sampler2D _ScreenTexture;
			sampler2D _OverlayTexture;
			fixed4 _Color;
			half _LineSize;

			// Vertex incoming data
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			// Vertex to fragment data
			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 uv : TEXCOORD0;
			};

			// Vertex shader
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = ComputeScreenPos(o.vertex);
				return o;
			}

			// Fragment shader
			fixed4 frag (v2f i) : COLOR
			{
				// Drop every other pixel line
				fixed p = i.uv.y / i.uv.w;
				if ((uint) (p * _ScreenParams.y / floor(_LineSize)) % 2 == 0) discard;

				// Apply texture overlay
				fixed4 col = tex2D(_OverlayTexture, i.uv);
				col *= _Color;
				return col;
			}
			ENDCG
		}
	}
}
