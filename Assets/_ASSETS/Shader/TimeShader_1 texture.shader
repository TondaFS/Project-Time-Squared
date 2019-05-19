Shader "_TIME/Timeline_1 texture"
{	

	Properties
	{
		[KeywordEnum(Past, Present, Stay)] _Timeline ("Timeline", Float) = 0
		_Tex ("Texture", 2D) = "gray" {}

	}
	SubShader
	{

		Tags { 
			"RenderType"="Opaque" 
		}
		Cull Off

		CGPROGRAM

		#pragma multi_compile _TIMELINE_PAST _TIMELINE_PRESENT _TIMELINE_STAY
		#pragma surface surf Standard fullforwardshadows

		struct Input {
			float2 uv_Tex;
			float3 worldPos;
		};

		uniform half _FieldRad; 
		uniform half4 _FieldPos;
		uniform half _BorderWidth;
		uniform fixed4 _BorderColor;

		sampler2D _Tex;
		

		void surf (Input IN, inout SurfaceOutputStandard o) {
			
			half distX = abs(IN.worldPos.x - _FieldPos.x);
			half distY = abs(IN.worldPos.y - _FieldPos.y);
			half distZ = abs(IN.worldPos.z - _FieldPos.z);
			half dist = max(distX, max(distY, distZ));
			//half dist = distance( IN.worldPos, _FieldPos);
			
			//#ifdef _TIMELINE_STAY
			//#endif

			#ifdef _TIMELINE_PAST
			clip( _FieldRad - dist );
			#endif

			#ifdef _TIMELINE_PRESENT
			clip( dist - _FieldRad );
			#endif

			half distSq = dist*dist;
			half farDistSq = _FieldRad + _BorderWidth/2;
			farDistSq *= farDistSq;
			half nearDistSq = _FieldRad - _BorderWidth/2;
			nearDistSq *= nearDistSq;
			
			half alpha = saturate((distSq-nearDistSq)/(farDistSq-nearDistSq));
			half border_alpha = abs(alpha*2-1);
			if(alpha > 0.5) {
				border_alpha = 1;
				alpha = 1;
			} 

			fixed4 albedo = tex2D(_Tex, IN.uv_Tex);

			o.Albedo = lerp(_BorderColor.rgb,lerp(albedo.rgb,albedo.rgb,alpha),border_alpha);
			// o.Albedo = albedo.rgb;
			o.Alpha = lerp(albedo.a,albedo.a,alpha);
			// o.Alpha = albedo.a;

		}

		
		   
		ENDCG
		
	}
}
