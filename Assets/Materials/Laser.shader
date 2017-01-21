Shader "Unlit/Laser"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Amplitude ("Amplitude", float) = 1
		_Frequency ("Frequency", float) = 1
		_Speed ("Speed", float) = 1
		_Offset ("Offset", float) = 0
		_Color ("Color", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags { "RenderType"="Transparent" }
		Blend One One
		ZWrite off
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Amplitude;
			float _Frequency;
			float _Speed;
			float _Offset;
			float4 _Color;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
	    		v.vertex.xyz += v.normal*sin( (_Time.y * _Speed + v.uv.y * _Frequency) + _Offset) * _Amplitude;

				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				
				fixed4 col = tex2D(_MainTex, float2(i.uv.x, 0.5f)) * _Color;
				return col;
			}
			ENDCG
		}


	}
}
