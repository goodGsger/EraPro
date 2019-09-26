Shader "Role_Self-Illumin_alpha_diff" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_MaskTex ("Mask", 2D) = "white" {}
	//_Illum ("Illumin (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	//_EmissionLM ("Emission (Lightmapper)", Float) = 0
	_Test ("Test",Range(0,1)) = 0
	_RimColor ("Rim Color", Color) = (1,1,1,1)
	_RimPower ("Rim Power", Range(0.5,8.0)) = 1.0
}
SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout" }
	LOD 200
	Cull off
	//Alphatest Greater 0.2
	
CGPROGRAM
#pragma surface surf Lambert alphatest:_Cutoff

sampler2D _MainTex;
sampler2D _MaskTex;
//sampler2D _Illum;
fixed4 _Color;
float _Test;
float4 _RimColor;
float _RimPower;

struct Input {
	float2 uv_MainTex;
	//float2 uv_Illum;
	float3 viewDir;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	//fixed4 c = tex * _Color;
	//o.Albedo = tex.rgb;
	if(_Test < 0.5)
	{
		o.Emission = tex.rgb; //* tex2D(_Illum, IN.uv_Illum).a;
	}
	else
	{
		half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
		//o.Emission = c.rgb + _RimColor.rgb * pow (rim, _RimPower);
		o.Emission = tex.rgb + _RimColor.rgb * rim;
	}
	fixed4 mask = tex2D(_MaskTex, IN.uv_MainTex);
	o.Alpha = tex.a * mask.r;
}
ENDCG
} 
FallBack "Self-Illumin/VertexLit"
}
