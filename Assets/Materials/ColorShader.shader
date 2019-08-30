Shader "Custom/ColorShader"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		const static int defaultNumColors = 8;
		int _NumColors;
		float3 _Colors[defaultNumColors];
		float _Heights[defaultNumColors];
		float _MinHeight;
		float _MaxHeight;

        struct Input
        {
			float3 worldPos;
        };

		float inverseLerp(float min, float max, float value)
		{
			return saturate((value - min) / (max - min));
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float heightPercent = inverseLerp(_MinHeight, _MaxHeight, IN.worldPos.y);
			for (int i = 0; i < _NumColors; ++i)
			{
				float drawStrength = saturate(sign(heightPercent - _Heights[i]));
				o.Albedo = o.Albedo * (1 - drawStrength) + _Colors[i] * drawStrength;
			}
        }
        ENDCG
    }
    FallBack "Diffuse"
}
