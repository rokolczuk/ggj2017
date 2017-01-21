Shader "Unlit/Laser"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Amplitude ("Amplitude", float) = 1
		_Frequency ("Frequency", float) = 1
		_Speed ("Speed", float) = 1
		_Offset ("Offset", float) = 0

	}

	SubShader
	{
		Tags { "RenderType"="Transparent" }
		Blend One One
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
//				float phase = _Time * 20.0;
//	    		float offset = (v.vertex.x + (v.vertex.z * 0.2)) * 0.5;
				
	    		v.vertex.xyz += v.normal*sin( (_Time.y * _Speed + v.uv.y * _Frequency) + _Offset) * _Amplitude;

				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, float2(i.uv.x, i.uv.y));

				return col;
			}
			ENDCG
		}


//		Pass {
//		    CGPROGRAM
//
//		    #pragma vertex vert             
//		    #pragma fragment frag
//
//		    sampler2D _MainTex;
//			float4 _MainTex_ST;
//
//		    struct vertInput {
//		        float4 pos : POSITION;
//		        float2 uv : TEXCOORD0;
//		    };  
//
//		    struct vertOutput {
//		        float4 pos : SV_POSITION;
//		        float2 uv : TEXCOORD0;
//		    };
//
//		    vertOutput vert(vertInput input) {
//		        vertOutput o;
//		        o.pos = mul(UNITY_MATRIX_MVP, input.pos);
//		        o.uv = TRANSFORM_TEX(input.uv, _MainTex);
//		        return o;
//		    }
//
//		   
//
//		    half4 frag(vertOutput vert) : COLOR {
//		        return half4(0.0, 1.0, vert.uv.x), 0.0, 1.0); 
//		    }
//		    ENDCG
//		}

	}
}
