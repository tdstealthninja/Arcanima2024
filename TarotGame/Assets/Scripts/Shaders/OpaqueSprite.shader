Shader "Unlit/OpaqueSprite"
{
	Properties
	{
		_Color( "Tint", Color ) = ( 1, 1, 1, 1 )
		_MainTex( "Texture", 2D ) = "white" {}
	}
		SubShader
	{
		Tags
		{
			"RenderType" = "TransparentCutout" // allows the depth shader to see the object
			"Queue" = "Transparent" // renders the sprite at the normal time to render sprites
		}
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite on
		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};
			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};
			v2f vert( appdata v )
			{
				v2f o;
				o.position = UnityObjectToClipPos( v.vertex );
				o.uv = TRANSFORM_TEX( v.uv, _MainTex );
				o.color = v.color;
				return o;
			}
			fixed4 frag( v2f i ) : SV_TARGET
			{
				fixed4 col = tex2D( _MainTex, i.uv );
				col *= _Color;
				col *= i.color;

				return col;
			}
			ENDCG
		}
	}
}