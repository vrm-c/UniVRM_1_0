﻿Shader "UniVRM/MetallicRoughnessGltfToUnity"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SmoothnessOrRoughness("SmoothnessOrRoughness", Float) = 1.0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _SmoothnessOrRoughness;

			fixed4 frag (v2f i) : SV_Target
			{
				half4 col = tex2D(_MainTex, i.uv);
				float pixelRoughnessFactor = (col.g * _SmoothnessOrRoughness);
				float pixelSmoothness = 1.0f - sqrt(pixelRoughnessFactor);
				return half4(col.b, 0, 0, clamp(pixelSmoothness, 0, 1.0));
			}
			ENDCG
		}
	}
}
