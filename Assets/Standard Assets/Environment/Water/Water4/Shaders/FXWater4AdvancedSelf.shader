// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "FX/Water4_Self" {
Properties {
	_ReflectionTex ("Internal reflection", 2D) = "white" {}
	
	_MainTex ("Fallback texture", 2D) = "black" {}
	_ShoreTex ("Shore & Foam texture ", 2D) = "black" {}
	_BumpMap ("Normals ", 2D) = "bump" {}
	
	_DistortParams ("Distortions (Bump waves, Reflection, Fresnel power, Fresnel bias)", Vector) = (1.0 ,1.0, 2.0, 1.15)
	//_InvFadeParemeter ("Auto blend parameter (Edge, Shore, Distance scale)", Vector) = (0.15 ,0.15, 0.5, 1.0)
	
	_BumpTiling ("Bump Tiling", Vector) = (1.0 ,1.0, -2.0, 3.0)
	_BumpDirection ("Bump Direction & Speed", Vector) = (1.0 ,1.0, -1.0, 1.0)
	
	//_BaseColor ("Base color", COLOR)  = ( .54, .95, .99, 0.5)
	//_ReflectionColor ("Reflection color", COLOR)  = ( .54, .95, .99, 0.5)
	_SpecularColor ("Specular color", COLOR)  = ( .72, .72, .72, 1)

	_WorldLightDir ("Specular light direction", Vector) = (0.0, 0.1, -0.5, 0.0)
	
	_Foam ("Foam (intensity, cutoff)", Vector) = (0.1, 0.375, 0.0, 0.0)

	WaterWaveParam("波形",Vector) = (0.05,0.12,0.125,0.033)
	WaterWaveSpeedDirHeight("流速,流动方向,波形高缩放",Vector) = (0.001,1,1,0.15)
	WaterBrightColor_Opacity("亮色,透明距离",COLOR)=(1,1,1,0.125)
	WaterDarkColor_Refract("深水颜色,清晰度",COLOR) = (0.007,0.231,0.298,1)
	WaterRelect("最低反射率,反射倍数",Vector)=(0,1,0,0)
}


CGINCLUDE

	#include "UnityCG.cginc"
	#include "WaterInclude.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	// interpolator structs
	
	struct v2f
	{
		float4 pos : SV_POSITION;
		float4 normalInterpolator : TEXCOORD0;
		float4 viewInterpolator : TEXCOORD1;
		float4 bumpCoords : TEXCOORD2;
		float4 screenPos : TEXCOORD3;
		float4 grabPassPos : TEXCOORD4;
		UNITY_FOG_COORDS(5)
	};


	// textures
	sampler2D _BumpMap;
	sampler2D _ReflectionTex;
	//sampler2D _RefractionTex;
	sampler2D _GrabBlurTexture;
	sampler2D _ShoreTex;
	sampler2D_float _CameraDepthTexture;

	// colors in use
	//uniform float4 _RefrColorDepth;
	uniform float4 _SpecularColor;
	//uniform float4 _BaseColor;
	//uniform float4 _ReflectionColor;
	
	// edge & shore fading
	//uniform float4 _InvFadeParemeter;

	// specularity
	uniform float _Shininess;
	uniform float4 _WorldLightDir;

	// fresnel, vertex & bump displacements & strength
	uniform float4 _DistortParams;
	//uniform float _FresnelScale;
	uniform float4 _BumpTiling;
	uniform float4 _BumpDirection;
	// foam
	uniform float4 _Foam;

	uniform float4 WaterWaveSpeedDirHeight;

	uniform float4 WaterWaveParam;
	uniform float4 WaterDarkColor_Refract;
	uniform float4 WaterRelect;
	uniform float4 WaterBrightColor_Opacity;
	
	// shortcuts
	#define PER_PIXEL_DISPLACE _DistortParams.x
	#define REALTIME_DISTORTION _DistortParams.y
	#define FRESNEL_POWER _DistortParams.z
	#define VERTEX_WORLD_NORMAL i.normalInterpolator.xyz
	#define FRESNEL_BIAS _DistortParams.w
	//#define NORMAL_DISPLACEMENT_PER_VERTEX _InvFadeParemeter.z
	#define _Water_SunShine_
#ifdef WATER_EDGEBLEND_ON
	#define _Water_Depth_
#endif
#ifdef WATER_REFLECTIVE
	#define _Water_Reflect_
#endif
	#define _Water_Refract_
	//
	// HQ VERSION
	//
	
	v2f vert(appdata_full v)
	{
		v2f o;
		
		half3 worldSpaceVertex = mul(unity_ObjectToWorld,(v.vertex)).xyz;
		half3 vtxForAni = (worldSpaceVertex).xzz;
		

		// one can also use worldSpaceVertex.xz here (speed!), albeit it'll end up a little skewed
		half2 tileableUv = mul(unity_ObjectToWorld,(v.vertex)).xz;
		
		o.bumpCoords.xyzw = (tileableUv.xyxy + _Time.xxxx * _BumpDirection.xyzw) * _BumpTiling.xyzw;

		o.viewInterpolator.xyz = worldSpaceVertex - _WorldSpaceCameraPos;

		o.pos = UnityObjectToClipPos(v.vertex);

		ComputeScreenAndGrabPassPos(o.pos, o.screenPos, o.grabPassPos);
		
		o.normalInterpolator.xyz = float4(0,1,0,0);
		
		o.viewInterpolator.w = o.pos.w*_ProjectionParams.w;
		o.normalInterpolator.w = 1;//GetDistanceFadeout(o.screenPos.w, DISTANCE_SCALE);

		
		UNITY_TRANSFER_FOG(o,o.pos);
		return o;
	}
	float GetFresnel(float NdotI, float bias, float power)
	{
		float facing = (1.0 - NdotI);
		return bias + (1-bias)*(float)pow(facing, power);
	}
	half4 frag( v2f i ) : SV_Target
	{
		float3 SunLightDir = _WorldLightDir.xyz;//float3( 0.70,-0.3,0.0 );
		float3 SunColor = _SpecularColor.rgb;//float3( 0.50,0.50,0.0 );
		float3 cWaterDeep = (float3)WaterDarkColor_Refract.rgb;
		float3 cShallowWater = WaterBrightColor_Opacity.rgb;

		half3 worldNormal = PerPixelNormal(_BumpMap, i.bumpCoords, VERTEX_WORLD_NORMAL, PER_PIXEL_DISPLACE);
		half3 viewVector = normalize(i.viewInterpolator.xyz);
		half4 distortOffset = half4(worldNormal.xz * REALTIME_DISTORTION * 10.0, 0, 0);
		half4 screenWithOffset = i.screenPos + distortOffset;
		half4 grabWithOffset = i.grabPassPos + distortOffset;
		//计算费聂耳
		float NdotI = (float)abs(dot(viewVector.xyz, worldNormal.xyz));
		float fFresnel = (float)GetFresnel(NdotI, (float)WaterRelect.x, 5)*(float)WaterRelect.y;
		//
		float facing = (float)saturate(dot(viewVector.xyz, worldNormal));
		float fInsideReflection = facing;
		//fInsideReflection = (float)saturate(-viewVector.z*1000.0);
#ifdef _Water_SunShine_
		float3 mirrorEye = (float3)( 2 * dot( viewVector.xyz, worldNormal) * worldNormal - viewVector.xyz );
		float dotSpec =  (float)saturate(dot( mirrorEye.xyz, SunLightDir ) * 0.5 + 0.5);
		float fFoamGloss=1.0;
		float3 vSunGlow = (float3)(fFresnel *( ( pow( dotSpec, 256 ) )*(fFoamGloss*1.8+0.2))* SunColor.xyz);
		vSunGlow += (float3)(vSunGlow *saturate(fFoamGloss-0.05)* SunColor.xyz);
#endif
		float softIntersect = 1;
#ifdef _Water_Depth_
		half depthScene = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos));
		depthScene = LinearEyeDepth(depthScene);
		//float depthScene = tex2Dproj(m_sceneDepthMap,oScreenProj).r*g_PS_NearFarClipDist.y;
		float depthWater = depthScene-i.screenPos.w;
		softIntersect = saturate(depthWater*WaterBrightColor_Opacity.w);
#endif
		//水体颜色
		float3 cWaterVolume = cWaterDeep*softIntersect;

		//反射
		float3 cReflect =1;
#ifdef _Water_Reflect_
		//float2 tReflect = ssTex;
		//tReflect = tReflect + worldNormal.xy * (0.33 )*0.5;
		//cReflect = tex2D(m_reflectMap, tReflect).rgb;
		cReflect = tex2Dproj(_ReflectionTex, UNITY_PROJ_COORD(screenWithOffset)).rgb;
#else
		cReflect = lerp(WaterBrightColor_Opacity.rgb,cWaterDeep,facing);
#endif
		cReflect = cReflect*cShallowWater.rgb;

		float3 final=1;
		//折射
		float3 cRefract =cWaterVolume;
#ifdef _Water_Refract_
		float2 tRefract = UNITY_PROJ_COORD(i.screenPos);                    
		float2 tRefractOffset = worldNormal.xy *(0.1+ 0.1*fInsideReflection)*saturate( softIntersect*4); // Difraction amount always 0.1
#ifdef _Water_Depth_
		const float fRefrMaskBias = 0.0001;
		float vvvvvv = i.viewInterpolator.w + fRefrMaskBias;
		//依据深度来决定是否折射
		float depthSceneAfterRefra = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos));//tex2D(m_sceneDepthMap,tRefract+tRefractOffset).r;
		float refraMask = vvvvvv<depthSceneAfterRefra;
		tRefractOffset*=refraMask;
#endif
		cRefract = tex2Dproj(_GrabBlurTexture, UNITY_PROJ_COORD(grabWithOffset));
		//tex2Dproj(_GrabBlurTexture, UNITY_PROJ_COORD(i.grabPassPos)).rgb;//tex2D(_GrabBlurTexture,tRefract+tRefractOffset).rgb;
		//增加水体颜色
		cRefract = lerp(cRefract.rgb,cWaterVolume.rgb,saturate(softIntersect*WaterDarkColor_Refract.w));
#endif
		final= lerp(cRefract,cReflect.rgb,saturate(fFresnel +saturate(1-NdotI*2)*fInsideReflection)); 
	//final += (g_PS_SkyColor.xyz + g_PS_SunColor.xyz )  * cFoamFinal ; // 2 alu
#ifdef _Water_SunShine_
		final += vSunGlow;
#endif
		half4 baseColor = 1;
#ifdef _Water_Refract_
	//如果包含了水面折射,则是关闭alpha blend,所以这里需要直接混合
		//final.xyz = lerp( cRefract, final.xyz, softIntersect * saturate(i.screenPos.w));
		baseColor.a = softIntersect;
#else
		baseColor.a = softIntersect;
#endif
		baseColor.rgb = final;
		//debug
		//baseColor.rgb =  LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)))/300;
#ifdef _Water_Depth_
		//baseColor.rgb = depthWater;
		//baseColor.rgb = softIntersect;
#endif
		//baseColor.a = 1;
		//baseColor.rgb = cRefract;//tex2Dproj(_GrabBlurTexture, UNITY_PROJ_COORD(i.grabPassPos)).rgb;
		//end debug
		UNITY_APPLY_FOG(i.fogCoord, baseColor);
		return baseColor;
	}
ENDCG

Subshader
{
	Tags {"RenderType"="Transparent" "Queue"="Transparent"}
	
	Lod 500
	ColorMask RGB
	
	//GrabPass { "_RefractionTex" }
	
	Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Cull Off
		
			CGPROGRAM
		
			//#pragma target 3.0
		
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
		
			//#pragma multi_compile WATER_VERTEX_DISPLACEMENT_ON WATER_VERTEX_DISPLACEMENT_OFF
			//#pragma multi_compile WATER_EDGEBLEND_ON WATER_EDGEBLEND_OFF
			#pragma multi_compile WATER_REFLECTIVE WATER_SIMPLE
			#pragma multi_compile WATER_EDGEBLEND_ON WATER_EDGEBLEND_OFF
		
			ENDCG
	}
}


Fallback "Transparent/Diffuse"
}
