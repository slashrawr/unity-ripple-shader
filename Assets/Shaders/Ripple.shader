// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Ripple" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		//The height of the ripples
		_Scale ("Scale", float) = 0.5
		//The speed of the animation
		_Speed ("Speed", float) = 0.3
		 //The rate at which the ripple occurs - use this to give give a sense of liquid density (low = thick, high = thin)
		_Frequency ("Frequency", float) = 5
		//Diamter and number of ripples
		_RippleDiameter("RippleDiameter", Range(0,5)) = 3 
	}

	SubShader 
	{
		
		Tags{"RenderType" = "Transparent" "Queue"="Transparent" "IgnoreProjector" = "True"}
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		Pass {

			Tags{"LightMode" = "ForwardBase"}
		
		CGPROGRAM


			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			sampler2D _MainTex;
			float4  _MainTex_ST;
			float _Scale, _Speed, _Frequency, _RippleDiameter;
			half4 _Color;
			float _XImpacts[100];
			float _ZImpacts[100];
			float _YImpacts[100];
			float _Values[100];
			float _Amplitudes[100];
			int _maxRipples = 100;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				fixed4 diff : COLOR0;
				fixed4 color : COLOR1;
				float2 uv : TEXCOORD;
			};
			
			v2f vert(appdata v)
			{
				v2f o;
				o.uv = v.uv;

				//Ripple calculations
				[unroll]
				for (int i=0; i < _maxRipples; i++)
				{
					_Values[i] = _Scale * sin(_Time.w * _Speed * - _Frequency + (pow(v.vertex.x - _XImpacts[i],2)+pow(v.vertex.z - _ZImpacts[i],2))+pow(v.vertex.y - _YImpacts[i],2));

					//if the ripple hasnt reached max diameter then continue to... ripple
					if (sqrt(pow(v.vertex.x - _XImpacts[i], 2) + pow(v.vertex.z - _ZImpacts[i], 2) + pow(v.vertex.y - _YImpacts[i], 2)) < _RippleDiameter)
					{
						v.vertex.y += _Values[i] * _Amplitudes[i];
						v.normal.xyz += _Values[i] * _Amplitudes[i];
					}
				}

				//Lighting calculations
				float3 normalDirection = normalize(mul( float4(v.normal, 0.0), unity_WorldToObject ).xyz);
				float3 lightDirection;
				lightDirection = normalize( _WorldSpaceLightPos0.xyz);
				float3 diffuseReflection = _LightColor0.xyz * max(0.0, dot(normalDirection, lightDirection));
				o.color = float4(diffuseReflection,1.0);

				o.vertex = UnityObjectToClipPos(v.vertex);

				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				//Tiling
				float2 uv = TRANSFORM_TEX(i.uv, _MainTex);
				//Texture tinted with material color and light color
				fixed4 c = tex2D (_MainTex, uv + _MainTex_ST.xy) * i.color * _Color;

				return c;
			}

		ENDCG
		}
	} 
}