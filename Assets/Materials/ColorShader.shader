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

		#include "UnityCG.cginc"

		const static int defaultNumLayers = 8;
		const static float epsilon = 1E-4;
		int _NumLayers;
		float3 _Colors[defaultNumLayers];
		float _Heights[defaultNumLayers];
		float _Blendings[defaultNumLayers];
		float _DrawStrengths[defaultNumLayers];
		float _Scales[defaultNumLayers];
		float _MinHeight;
		float _MaxHeight;

        struct Input
        {
			float3 worldPos;
			float3 worldNormal;
        };

		UNITY_DECLARE_TEX2DARRAY(_TextureArray);

		float inverseLerp(float min, float max, float value)
		{
			return saturate((value - min) / (max - min));
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float3 axes = abs(IN.worldNormal);
			axes /= axes.x + axes.y + axes.z;
			float heightPercent = inverseLerp(_MinHeight, _MaxHeight, IN.worldPos.y);
			for (int i = 0; i < _NumLayers; ++i)
			{
				float3 scaledTexturePos = IN.worldPos / _Scales[i];
				float3 textureX = UNITY_SAMPLE_TEX2DARRAY(_TextureArray, float3(scaledTexturePos.y, scaledTexturePos.z, i)) * axes.x;
				float3 textureY = UNITY_SAMPLE_TEX2DARRAY(_TextureArray, float3(scaledTexturePos.x, scaledTexturePos.z, i)) * axes.y;
				float3 textureZ = UNITY_SAMPLE_TEX2DARRAY(_TextureArray, float3(scaledTexturePos.x, scaledTexturePos.y, i)) * axes.z;
				float drawStrength = inverseLerp(-_Blendings[i] / 2 - epsilon, _Blendings[i] / 2, heightPercent - _Heights[i]);
				float3 color = textureX + textureY + textureZ;
				color = _Colors[i] * (1 - _DrawStrengths[i]) + color * _DrawStrengths[i];
				o.Albedo = o.Albedo * (1 - drawStrength) + color * drawStrength;
			}
        }
        ENDCG
    }
    FallBack "Diffuse"
}
