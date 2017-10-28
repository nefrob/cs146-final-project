﻿// From: http://wiki.unity3d.com/index.php?title=Shield, small additions by Robert Neff

Shader "Custom/ForceShieldShader" {

	// Define shader properties
	Properties {
		// _name("unity editor name", type) = value
		_Color("_Color", Color) = (0.0, 1.0, 0.0, 1.0)
		
		_Inside("_Inside", Range(0.0, 2.0)) = 0.0
		
		_Rim("_Rim", Range(0.0, 2.0)) = 1.2
		
		_Texture("_Texture", 2D) = "white" {}
		
		_Speed("_Speed", Range(0.5, 10.0)) = 0.5
		
		_Tile("_Tile", Range(1.0, 10.0)) = 5.0
		
		_Strength("_Strength", Range(0.0, 10.0)) = 1.5
	}

	// Shader code
	SubShader {
		// Shader passes
		// Background (1000), Geometry(2000), Transparent(3000), Overlay(4000)
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

		Cull Back // cull back side
		ZWrite On // write pixels to depth buffer
		ZTest LEqual // type of depth testing

		// Start shader code
		CGPROGRAM
		// Use Blinn Phong lighting model
		#pragma surface surf BlinnPhongEditor alpha vertex:vert

	    // Input variables
		fixed4 _Color;
		sampler2D _CameraDepthTexture;
		fixed _Inside;
		fixed _Rim;
		sampler2D _Texture;
		fixed _Speed;
		fixed _Tile;
		fixed _Strength;

		// Variables to set
		struct EditorSurfaceOutput {
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half3 Gloss;
			half Specular;
			half Alpha;
		};

		// Lighting definition
		inline half4 LightingBlinnPhongEditor_PrePass(EditorSurfaceOutput s, half4 light) {
			half3 spec = light.a * s.Gloss;

			half4 c;

			c.rgb = (s.Albedo * light.rgb + light.rgb * spec);

			c.a = s.Alpha + Luminance(spec);

			return c;
		}

		// Lighting definition continued
		inline half4 LightingBlinnPhongEditor(EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			viewDir = normalize(viewDir);
			half3 h = normalize(lightDir + viewDir);

			half diff = max(0, dot(s.Normal, lightDir));

			float nh = max(0, dot(s.Normal, h));
			float3 spec = pow(nh, s.Specular*128.0) * s.Gloss;

			half4 res;
			res.rgb = _LightColor0.rgb * (diff * atten * 2.0);
			res.w = spec * Luminance(_LightColor0.rgb);

			return LightingBlinnPhongEditor_PrePass(s, res);
		}

		// Input to surf
		struct Input {
			float4 screenPos;
			float3 viewDir;
			float2 uv_Texture;
		};

		// Vertex shader computation?
		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
		}

		// Compute surface shading
		void surf(Input IN, inout EditorSurfaceOutput o) {
			o.Albedo = fixed3(0.0,0.0,0.0);
			o.Normal = fixed3(0.0,0.0,1.0);
			o.Emission = 0.0;
			o.Gloss = 0.0;
			o.Specular = 0.0;
			o.Alpha = 1.0;

			float4 ScreenDepthDiff0 = 
				LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)).r) - IN.screenPos.z;
			float4 Saturate0 = fixed4(0.3, 0.3, 0.3, 1.0);
			float4 Fresnel0_1_NoInput = fixed4(0, 0, 1, 1);
			float dNorm = 1.0 - dot(normalize(float4(IN.viewDir, 1.0).xyz), normalize(Fresnel0_1_NoInput.xyz));
			float4 Fresnel0 = float4(dNorm,dNorm,dNorm,dNorm);
			float4 Step0 = step(Fresnel0,float4(1.0, 1.0, 1.0, 1.0));
			float4 Clamp0 = clamp(Step0,_Inside.xxxx,float4(1.0, 1.0, 1.0, 1.0));
			float4 Pow0 = pow(Fresnel0,(_Rim).xxxx);
			float4 Multiply5 = _Time * _Speed.xxxx;
			float4 UV_Pan0 = 
				float4((IN.uv_Texture.xyxy).x,(IN.uv_Texture.xyxy).y + Multiply5.x,
				(IN.uv_Texture.xyxy).z, (IN.uv_Texture.xyxy).w);
			float4 Multiply1 = UV_Pan0 * _Tile.xxxx;
			float4 Tex2D0 = tex2D(_Texture, Multiply1.xy);
			float4 Multiply2 = Tex2D0 * _Strength.xxxx;
			float4 Multiply0 = Pow0 * Multiply2;
			float4 Multiply3 = Clamp0 * Multiply0;
			float4 Multiply4 = Saturate0 * Multiply3;
			o.Emission = Multiply3.xyz * _Color.rgb;
			o.Alpha = Multiply3.w * _Color.a;
		}
		// End CG code
		ENDCG
	}
		Fallback "Diffuse"
}