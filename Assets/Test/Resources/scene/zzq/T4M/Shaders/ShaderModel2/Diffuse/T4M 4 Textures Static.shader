Shader "T4MShaders/ShaderModel2/Diffuse/T4M 4 Textures Static" {
Properties {
	_Splat0 ("Layer 1", 2D) = "white" {}
	_Splat1 ("Layer 2", 2D) = "white" {}
	_Splat2 ("Layer 3", 2D) = "white" {}
	_Splat3 ("Layer 4", 2D) = "white" {}
	_Control ("Control (RGBA)", 2D) = "white" {}
	_Lightmap("Lightmap", 2D) = "white" {}
	//_MainTex ("Never Used", 2D) = "white" {}
}
                
SubShader {
	Tags {
		"SplatCount" = "4"
		"RenderType" = "Opaque"
		"LightMode" = "ForwardBase"
	}
CGPROGRAM
//#pragma target 3.0
#pragma surface surf Unlit nometa noshadow noforwardadd nolightmap
//#pragma fragmentoption ARB_precision_hint_fastest
//#include "./CgIncludes/LightingCG.cginc"
//#pragma exclude_renderers xbox360 ps3
        inline half4 LightingUnlit (SurfaceOutput s, fixed3 lightDir, fixed atten)  
        {  
            half4 c = half4(1,1,1,1);  
            c.rgb = s.Albedo;  
            c.a = s.Alpha;  
            return c;  
        } 
struct Input {
	float2 uv_Control : TEXCOORD0;
	
	float2 uv_Splat0 : TEXCOORD1;
	float2 uv_Splat1 : TEXCOORD2;
	float2 uv_Splat2 : TEXCOORD3;
	float2 uv_Splat3 : TEXCOORD4;
	float2 uv2_Lightmap : TEXCOORD5;
};
 
sampler2D _Control,_Lightmap;
sampler2D _Splat0,_Splat1,_Splat2,_Splat3;
 
void surf (Input IN, inout SurfaceOutput o) {
	fixed4 splat_control = tex2D (_Control, IN.uv_Control).rgba;
		
	fixed3 lay1 = tex2D (_Splat0, IN.uv_Splat0);
	fixed3 lay2 = tex2D (_Splat1, IN.uv_Splat1);
	fixed3 lay3 = tex2D (_Splat2, IN.uv_Splat2);
	fixed3 lay4 = tex2D (_Splat3, IN.uv_Splat3);
	o.Alpha = 0.0;
	o.Albedo.rgb =  0.65*DecodeLightmap(tex2D(_Lightmap, IN.uv2_Lightmap))*
	(lay1 * splat_control.r + lay2 * splat_control.g + lay3 * splat_control.b + lay4 * splat_control.a);
}
ENDCG 
}
// Fallback to Diffuse
Fallback "Diffuse"
}