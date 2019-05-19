Shader "_TIME/FieldShader"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0,0,0,0)
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_DistortStr("Distort Strength", Range(0,1)) = 0.2
		_DistortSpeed("Distort Speed", Range(0,1)) = 0.2
	}

	SubShader
	{
		Blend One One
		ZWrite Off
		Cull Off

		Tags
		{
			"RenderType"="Transparent"
			"Queue"="Transparent"
		}

		GrabPass
		{
			"_GrabTex"
		}

		Pass
		{
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 screenuv : TEXCOORD1;				
				float3 viewDir : TEXCOORD2;
				float4 grabPos : TEXCOORD3;
				float depth : DEPTH;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};

			sampler2D _GrabTex;
			float4 _GrabTex_ST;
			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			float _DistortStr;
			float _DistortSpeed;
			//sampler2D _CameraDepthNormalsTexture;
			fixed4 _TintColor;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);				
				o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);

				o.screenuv = ((o.vertex.xy / o.vertex.w) + 1)/2;
				o.screenuv.y = 1 - o.screenuv.y;
				o.depth = -UnityObjectToViewPos(v.vertex).z *_ProjectionParams.w;

				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex)));

				o.grabPos = ComputeGrabScreenPos(o.vertex);

				return o;
			}	

			fixed4 frag (v2f i) : SV_Target
			{		
				//float screenDepth = DecodeFloatRG(tex2D(_CameraDepthNormalsTexture, i.screenuv).zw);
				//float diff = screenDepth - i.depth;
				//float intersect = 0;

				//if (diff > 0)
				//	intersect = 1 - smoothstep(0, _ProjectionParams.w * 0.5, diff); 

				float4 offset = tex2D(_NoiseTex, i.uv - _Time.xy * _DistortSpeed);
				i.grabPos.xy -= offset.xy * _DistortStr;
				fixed4 distortColor = tex2Dproj(_GrabTex, i.grabPos);

				//float rim = 1 - abs(dot(i.normal, normalize(i.viewDir)));
				//float glow = max(intersect, rim);
				//fixed4 intersectColor = fixed4(lllerp(_Color.rgb + distortColor, fixed3(1, 1, 1), pow(glow, 4)), 1);

				

				//fixed4 col = _Color * _Color.a + intersectColor * glow + distortColor;
				// fixed4 col = _Color * _Color.a + distortColor;+
				// fixed4 col = distortColor + _TintColor * _TintColor.a;
				fixed4 col = distortColor * _TintColor.a + _TintColor * _TintColor.a;
				return col;
			}
			ENDCG
		}
	}
}
