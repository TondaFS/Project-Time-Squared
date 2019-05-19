Shader "_TIME/Timeline_2 textures"
{
	Properties
	{
		_PastTex ("Past Texture", 2D) = "gray" {}
		_PresentTex ("Present Texture", 2D) = "white" {}

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		CGPROGRAM
			
		#pragma surface surf Standard fullforwardshadows

		struct Input {
			float2 uv_PastTex;
			float2 uv_PresentTex;
			float3 worldPos;
		};

		uniform half _FieldRad; 
		uniform half4 _FieldPos;
		uniform half _BorderWidth;
		uniform fixed4 _BorderColor;

		sampler2D _PastTex;
		sampler2D _PresentTex;


		void surf (Input IN, inout SurfaceOutputStandard o) {

			
			half distX = abs(IN.worldPos.x - _FieldPos.x);
			half distY = abs(IN.worldPos.y - _FieldPos.y);
			half distZ = abs(IN.worldPos.z - _FieldPos.z);
			half dist = max(distX, max(distY, distZ));
			//half dist = distance( IN.worldPos, _FieldPos);

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
			
			half4 albedo_past = tex2D(_PastTex, IN.uv_PastTex);
			half4 albedo_present = tex2D(_PresentTex, IN.uv_PresentTex);

			o.Albedo = lerp( _BorderColor.rgb, lerp( albedo_past.rgb, albedo_present.rgb,alpha ), border_alpha );
			o.Alpha = lerp( albedo_past.a, albedo_present.a, alpha );
		}
		   
		ENDCG
		
	}
}
