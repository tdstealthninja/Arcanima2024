Shader "Unlit/NormalPostprocessing"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_EdgeColor("Edge Color", Color) = (0,0,0,1)
		_Scale("Scale", float) = 2
		_DepthThreshhold("Depth Threshhold", float) = 0.2
		_NormalThreshold("Normal Threshhold", float) = 0.4
		_ColorThreshold("Color Threshhold", float) = 0.4
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_TexelSize;
				sampler2D _CameraDepthNormalsTexture;
				float4 _MainTex_ST;
				float4 _EdgeColor;
				float _Scale;
				float _DepthThreshhold;
				float _NormalThreshold;
				float _ColorThreshold;
				float4x4 _viewToWorld;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float scale = (_ScreenParams.z * _ScreenParams.w) / _Scale;

					// Get positions to check
					float halfScaleFloor = floor(scale * 0.5);
					float halfScaleCeil = ceil(scale * 0.5);

					float2 bottomLeftUV = i.uv - float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleFloor;
					float2 topRightUV = i.uv + float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleCeil;
					float2 bottomRightUV = i.uv + float2(_MainTex_TexelSize.x * halfScaleCeil, -_MainTex_TexelSize.y * halfScaleFloor);
					float2 topLeftUV = i.uv + float2(-_MainTex_TexelSize.x * halfScaleFloor, _MainTex_TexelSize.y * halfScaleCeil);

					// Get DepthNormal
					fixed4 depthNormBotLeft = tex2D(_CameraDepthNormalsTexture, bottomLeftUV);
					fixed4 depthNormBotRight = tex2D(_CameraDepthNormalsTexture, bottomRightUV);
					fixed4 depthNormTopLeft = tex2D(_CameraDepthNormalsTexture, topLeftUV);
					fixed4 depthNormTopRight = tex2D(_CameraDepthNormalsTexture, topRightUV);

					// Get decode DepthNormal
					float3 normBotLeft;
					float depthBotLeft;
					DecodeDepthNormal(depthNormBotLeft, depthBotLeft, normBotLeft);

					float3 normBotRight;
					float depthBotRight;
					DecodeDepthNormal(depthNormBotRight, depthBotRight, normBotRight);

					float3 normTopLeft;
					float depthTopLeft;
					DecodeDepthNormal(depthNormTopLeft, depthTopLeft, normTopLeft);

					float3 normTopRight = 0;
					float depthTopRight;
					DecodeDepthNormal(depthNormTopRight, depthTopRight, normTopLeft);

					// Perform depth calculation
					float depthFiniteDifference0 = depthTopLeft - depthBotRight;
					float depthFiniteDifference1 = depthBotLeft - depthTopRight;

					float edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * 100;
					edgeDepth = edgeDepth > _DepthThreshhold ? 0 : 1;

					// Perfrom normal calculation
					float3 normalFiniteDifference0 = normTopLeft - normBotRight;
					float3 normalFiniteDifference1 = normBotLeft - normTopRight;
					float3 normalFiniteDifference2 = normTopLeft - normBotLeft;

					float edgeNormal = sqrt(dot(normalFiniteDifference0, normalFiniteDifference0) + dot(normalFiniteDifference1, normalFiniteDifference1) + dot(normalFiniteDifference2, normalFiniteDifference2));
					edgeNormal = edgeNormal > _NormalThreshold ? 0 : 1;

					// Combine normal and depth edges
					float edge = min(edgeDepth, edgeNormal);

					// Get _MainTex color
					fixed4 color = tex2D(_MainTex, i.uv);

					// return
					return edge == 0 ? _EdgeColor : color;
				}
				ENDCG
			}
		}
}
