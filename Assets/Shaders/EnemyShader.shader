//Samples the texture, converts color from rgba to hsva and applies hue, saturation, value and alpha
//
//
Shader "Sprite/EnemyShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_Whiteness("Whiteness", Range (0, 1)) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float2 texcoord  : TEXCOORD0;
			};

			sampler2D _MainTex;
			fixed4 _Color;
			float _Whiteness;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;

				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
			    float4 color = tex2D (_MainTex, IN.texcoord);
			    color.rgb += _Color.rgb * _Whiteness ;
			    color.rgb *= color.a;

				return color;
			}
		ENDCG
		}
	}
}
