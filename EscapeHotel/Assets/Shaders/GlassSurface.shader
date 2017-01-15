Shader "Custom/GlassSurface" { // from Alastrair Aitchison's blog
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Transarency (A only)", Color) = (0.5, 0.5, 0.5, 1)
		_Cube ("Reflection Cubemap", Cube) = "_Skybox" { /*TexGen CubeReflect*/}
		_ReflectColor ("Reflection Color", Color) = (1, 1, 1, 0.5)
		_ReflectBrightness ("Reflection Brightness", Float) = 1.0
		_SpecularMap("Specular / Reflection Map", 2D) = "white" {}
		_RimColor ("Rim Color", Color) = (0.26, 0.19, 0.16, 0.0)
	}
	SubShader {
		Tags { 
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
		}
		LOD 200
		
		CGPROGRAM
		// Use surface shader with function called "surf"
		// Use the inbuilt BlinnPhong lighting model
		// Use alpha blending
		#pragma surface surf BlinnPhong alpha

		sampler2D _MainTex;
		sampler2D _SpecularMap;
		samplerCUBE _Cube;
		fixed4 _ReflectColor;
		fixed _ReflectBrightness;
		fixed4 _Color;
		float4 _RimColor;

		struct Input {
			float2 uv_MainTex;
			float3 worldRefl; // Used for reflection map
			float3 viewDir; // Used for rim lighting
		};

		void surf (Input IN, inout SurfaceOutput o) {
			// Diffuse texture
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;

			// How transparent is the surface?
			o.Alpha = _Color.a * c.a;
			
			// Specular map
			half specular = tex2D(_SpecularMap, IN.uv_MainTex).r;
			o.Specular = specular;

			// Reflection map
			fixed4 reflcol = texCUBE(_Cube, IN.worldRefl);
			reflcol *= c.a;
			o.Emission = reflcol.rgb * _ReflectColor.rgb * _ReflectBrightness;
			o.Alpha = o.Alpha * _ReflectColor.a;

			// Rim
			// Rim lighting is emissive lighting based on angle between surface normal and view direction
			// You get more reflection at glancing angles
			half intensity = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			o.Emission += intensity * _RimColor.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
