// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
//对于游戏内用的装饰物，静态的就是这个了shader。。
Shader "QinYou/CharactarStatic" {
	Properties{
		
		//============
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Lightmap("Lightmap", 2D) = "white" {}
		//_ShadowPlane("Shadow Plane", Vector) = (0,1,0,2)
	//_ShadowColor("Shadow Color",Color) = (0.2, 0.2, 0.2, 1)
	//_ShadowPara("Shadow Param",Vector) = (-0.006,60,3,0.02)
		_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		
		//[HideInInspector][KeywordEnum(OFF, ON)] _SPEC_ON("打开镜面光", Float) = 0
		//[HideInInspector]_SpecularColor("Specular Color", Color) = (1,1,1,1)
		[HideInInspector]_SpecPower("Specular Power", Range(0.1,30)) = 1

		//[HideInInspector][KeywordEnum(OFF, ON)] _EMISSIVE("是否自发光?", Float) = 0
		[HideInInspector]_NoiseTex("噪声图 (RGB)", 2D) = "black" {}
		[HideInInspector]_Glow("自发光颜色", Color) = (1, 1, 1, 1)
		[HideInInspector]_NoiseScale("扰动强度", Range(0, 8.0)) = 3.0


		//[HideInInspector][KeywordEnum(OFF, ON)] _FRESNEL("fresnel开关?", Float) = 0
		[HideInInspector]_Cubemap("Environment Cubemap", Cube) = "_Skybox" {}
		[HideInInspector]_RimColor("边缘颜色", Color) = (1, 1, 1, 1)
		[HideInInspector]_RimPower("边缘强度", Range(0, 8.0)) = 3.0

		[HideInInspector]_Cutoff("Cutoff", float) = 0.5

		[HideInInspector][Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 2
		[HideInInspector] _Mode("Mode", Float) = 0.0
		[HideInInspector] _ZWrite("ZWrite", Float) = 1
		[HideInInspector] _SrcBlend("SrcBlend", Float) = 1.0
		[HideInInspector] _DstBlend("DstBlend", Float) = 0.0
		//

		
		[HideInInspector]_WaveSpeed("Wave Speed", float) = 1.0
        [HideInInspector]_HeightFactor("Height Factor", float) = 1.0
		[HideInInspector]_HeightCutoff("Height Cutoff", float) = 1.2
		[HideInInspector]_WindSpeed("Wind Speed", vector) = (1, 1, 1, 1)

        [HideInInspector]_WindTex("Wind Texture", 2D) = "white" {}
	}


	SubShader{
	Tags{ "RenderType" = "Opaque"}
	Pass{

	Tags{ "IgnoreProjector" = "True"  "LightMode" = "ForwardBase"}// 
		//Tags{}// 
		Cull[_Cull]
		ZWrite[_ZWrite]
		Blend[_SrcBlend][_DstBlend]
			
		CGPROGRAM

		#include "UnityCG.cginc"
		#include "autolight.cginc"
		#include "lighting.cginc"
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_instancing
		#pragma multi_compile_fog
		#pragma multi_compile_fwdbase

		//#pragma alphatest:_Cutoff
		//#pragma fragmentoption ARB_precision_hint_fastest	
		#pragma shader_feature _WIND_ON//是不是开启风

		#pragma shader_feature _SPEC_ON//镜面反射
		//
		//是否开启alphaTest
		#pragma shader_feature _ALPHATEST_ON
		//
		#pragma multi_compile _ _EMISSIVE_ON

		#pragma multi_compile _ _FRESNEL_ON
		

		
		half SHOWFOW;
		 uniform float4 _TimeEditor;
		sampler2D _MainTex;
		half4 _MainTex_ST;
		sampler2D _Lightmap;
      // Tiling/Offset for _Lightmap, used by TRANSFORM_TEX in vertex shader
		float4 _Lightmap_ST;

		sampler2D _NoiseTex;
		
		half4 _NoiseTex_ST;
		half4 _TintColor;
		half4 _Glow;
		//
		half4 _RimColor;
		half _RimPower;
		half _NoiseScale;
		//

		half4 _SpecularColor;
		half _SpecPower;
		
		sampler2D _ControlTex2;
		half _Cutoff;
		half2 _ControlTex2_TexelSize;

		//
		sampler2D _WindTex;
        half4 _WindTex_ST;		
        half _WaveSpeed;
        half _HeightFactor;
		half _HeightCutoff;
        half4 _WindSpeed;
		struct appdata {
			half4 vertex : POSITION;
			half2 texcoord : TEXCOORD0;
			half2 texcoord1 : TEXCOORD1;
			#if _WIND_ON
			half4 color : COLOR;
			#endif
			half3 normal : NORMAL;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f {
			float4 pos : SV_POSITION;
			half2 texcoord : TEXCOORD0;
			half2 uvLM : TEXCOORD1;//显然需要使用烘培纹理。。。
			half3 viewDir  : TEXCOORD2;//视线方向
			half3 normal : TEXCOORD3;
			half4 wpos:TEXCOORD4;//世界坐标
			
			UNITY_FOG_COORDS(5)
			half4 color : COLOR;
			//UNITY_VERTEX_INPUT_INSTANCE_ID
		};
	fixed3 DecodeLogLuv(fixed4 vLogLuv)
   {
       fixed3x3 InverseM = fixed3x3(
          6.0014, -2.7008, -1.7996,
          -1.3320, 3.1029, -5.7721,
          0.3008, -1.0882, 5.6268 );
       fixed Le = vLogLuv.z * 255 +
		vLogLuv.w;
			   fixed3 Xp_Y_XYZp;
			   Xp_Y_XYZp.y = exp2((Le - 127)
		/ 2);
			   Xp_Y_XYZp.z = Xp_Y_XYZp.y /
		vLogLuv.y;
			   Xp_Y_XYZp.x = vLogLuv.x *
		Xp_Y_XYZp.z;
			   fixed3 vRGB = mul(Xp_Y_XYZp,
		InverseM);

		return max(vRGB, 0);
		}
	v2f vert(appdata v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		o.pos = UnityObjectToClipPos(v.vertex);
		o.wpos = mul(unity_ObjectToWorld, v.vertex);

		half2 uv_BumpTex = o.wpos.xz *_ControlTex2_TexelSize * 8;
		half4 splat_control2 = tex2Dlod(_ControlTex2, half4(uv_BumpTex,0,0));//优化，改成顶点

		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.uvLM = TRANSFORM_TEX(v.texcoord1.xy, _Lightmap);
		half3 worldNormal = UnityObjectToWorldNormal(v.normal);//, (half3x3)unity_ObjectToWorld));
		half3 viewDir = normalize(_WorldSpaceCameraPos.xyz - o.wpos);
		//这里是光照
		o.color.rgb =1;
#if _WIND_ON
		float4 worldPos = o.wpos;
        // normalize position based on world size
        float2 samplePos = worldPos.xz/128.0;//归一化位置
        // scroll sample position based on time
        samplePos += _Time.x * _WindSpeed.xy;//位置进行偏移
        // sample wind texture
        float windSample = tex2Dlod(_WindTex, float4(samplePos, 0, 0));//repeat采样。
                
        // 0 animation below _HeightCutoff
        float heightFactor = v.color.y > _HeightCutoff;
		// make animation stronger with height
		heightFactor = heightFactor * pow(v.color.y, _HeightFactor);

        // apply wave animation
        //o.pos.z += sin(_WaveSpeed*windSample) * heightFactor;
        o.pos.x += cos(_WaveSpeed*windSample) * heightFactor;
#endif
#if _SPEC_ON
		half3 worldLight = normalize(_WorldSpaceLightPos0.xyz);
		half diff = clamp(dot(worldNormal, worldLight), 0, 1) *0.5 + 0.5;
		float3 reflectionVector = normalize((2.0 * worldNormal * diff) - worldLight);
		//Calculate the Phong specular
		float spec = pow(max(0, dot(reflectionVector, viewDir)), _SpecPower);

		o.color.rgb += _LightColor0.rgb * spec;;
#endif

		o.color.a= max(SHOWFOW, splat_control2.r);

		o.normal = worldNormal;//mul((half3x3)UNITY_MATRIX_IT_MV, v.normal);
		o.viewDir = viewDir;

		UNITY_TRANSFER_FOG(o, o.pos);
		return o;
	}
	
	//==============================fragment================
	half4 frag(v2f i) : SV_Target
	{
		half4 col = tex2D(_MainTex, i.texcoord);
		col *= _TintColor * 2;
		col.rgb *= i.color.rgb*i.color.a;
		
		
#if _ALPHATEST_ON
		clip(col.a - _Cutoff);
#endif
//烘培的情况下
//DecodeLightmap (
	fixed3 lm = DecodeLightmap(tex2D(_Lightmap, i.uvLM.xy));//安卓平台要乘2
	//fixed3 lm = DecodeLogLuv(tex2D(_Lightmap, i.uvLM.xy));//安卓平台要乘2
	col.rgb*=lm;
	//自发光
#if _EMISSIVE_ON	
	float4 node_9331 = _Time + _TimeEditor;
	float2 node_40 = ((i.texcoord*(_NoiseScale*-4.0+4.0)))+node_9331.g*float2(0.03,-0.03);
	float4 _Noise_var = tex2D(_NoiseTex,TRANSFORM_TEX(node_40, _NoiseTex));
	float3 emissive = (col.a*((_Glow.rgb*_Noise_var.rgb)+(_Glow.rgb*col.a)));
	col.rgb+=emissive;
#endif
#if _FRESNEL_ON
	half rim = 1 - saturate(dot(i.viewDir, i.normal));
	half fresnelTerm = pow(rim , _RimPower);//菲涅耳因数
	col.rgb =col.rgb * (1 - fresnelTerm) + fresnelTerm * _RimColor;
#endif
	
	UNITY_APPLY_FOG(i.fogCoord, col);

	return col;
	}
		ENDCG
	}
	
	Pass{
		//Shadow
		Name "ShadowCaster"
		Tags{ "LightMode" = "ShadowCaster" }
		 ZWrite On ZTest LEqual Cull Off

		 CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma target 2.0
        #pragma multi_compile_shadowcaster
		#pragma multi_compile_instancing
        #include "UnityCG.cginc"
		uniform half _Cutoff;
		uniform half _Mode;
		uniform sampler2D _MainTex;
		uniform half4 _MainTex_ST;
        struct v2f {
			float2  uv : TEXCOORD1;
            UNITY_VERTEX_OUTPUT_STEREO
			V2F_SHADOW_CASTER;
			//UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        v2f vert( appdata_base v )
        {
            v2f o;
            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
            TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
			o.uv = v.texcoord;//TRANSFORM_TEX(, _MainTex);
            return o;
        }

        float4 frag( v2f i ) : SV_Target
        {
			half4 texcol = tex2D(_MainTex, i.uv);
			if(_Mode!=0)
				clip(texcol.a - _Cutoff);//
			SHADOW_CASTER_FRAGMENT(i)
        }
        ENDCG
		}
	}

	
	Fallback off
	//Fallback "Legacy Shaders/Transparent/Cutout/Diffuse"
	//Fallback "Transparent/Cutout/Diffuse"
	CustomEditor "CharactarStaticShaderGUI"
}
