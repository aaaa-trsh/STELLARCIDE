Shader "Custom/Glitch"
{
	Properties
	{
		_BaseColor("Base Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
			"RenderType" = "Opaque"
			"Queue" = "Geometry"
		}
	}

	Pass
	{
		HLSLPROGRAM
		
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		
		ENDHLSL
	}
}
