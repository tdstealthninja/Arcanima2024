Shader "Custom/CelShader" 
{
	Properties 
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Albedo", 2D) = "white" {}
		_RampTex("Ramp Texture", 2D) = "white" {}
	}
	SubShader 
	{
		Tags 
		{
			"RenderType" = "Opaque"
		}
		LOD 200

		CGPROGRAM
		#pragma surface surf CelShading
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		sampler2D _RampTex;

		half4 LightingCelShading(SurfaceOutput s, half3 lightDir, half atten) 
		{
			half diffuse = dot(s.Normal, lightDir);
			atten = atten * diffuse;

			float satAtten = saturate(atten);
			float2 lookUpPos = (satAtten, satAtten);
			atten = (tex2D(_RampTex, lookUpPos));

			float lowVal = tex2D(_RampTex, float2(0,0));

			if(atten < lowVal)
			{
				atten = 0;
			}

			half4 c;
			c.rgb = _LightColor0.rgb * atten * s.Albedo;
			c.a = s.Alpha;
			return c;
		}

		struct Input 
		{
			float4 color : COLOR;
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) 
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Albedo *= IN.color.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}