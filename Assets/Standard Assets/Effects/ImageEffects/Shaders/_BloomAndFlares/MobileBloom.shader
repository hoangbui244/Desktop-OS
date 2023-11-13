// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Hidden/FastBloom" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	_Bloom("Bloom (RGB)", 2D) = "black" {}
	}

		CGINCLUDE

#include "UnityCG.cginc"

		sampler2D _MainTex;
	sampler2D _BloomAdditive;
	sampler2D _Bloom;
	sampler2D _BloomRefract;
	sampler2D _RadialBlurTex;

	uniform half4 _MainTex_TexelSize;

	uniform half4 _Parameter;
	uniform half4 _OffsetsA;
	uniform half4 _OffsetsB;

#define ONE_MINUS_THRESHHOLD_TIMES_INTENSITY _Parameter.w
#define THRESHHOLD _Parameter.z

	struct v2f_simple
	{
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;

#if UNITY_UV_STARTS_AT_TOP
		half2 uv2 : TEXCOORD1;
#endif
	};

	v2f_simple vertBloom(appdata_img v)
	{
		v2f_simple o;

		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord;

#if UNITY_UV_STARTS_AT_TOP
		o.uv2 = v.texcoord;
		if (_MainTex_TexelSize.y < 0.0)
			o.uv.y = 1.0 - o.uv.y;
#endif

		return o;
	}

	struct v2f_tap
	{
		float4 pos : SV_POSITION;
		half2 uv20 : TEXCOORD0;
		half2 uv21 : TEXCOORD1;
		half2 uv22 : TEXCOORD2;
		half2 uv23 : TEXCOORD3;
#if UNITY_UV_STARTS_AT_TOP
		half2 uvAddBloom : TEXCOORD4;
#endif
	};

	v2f_tap vert4Tap(appdata_img v)
	{
		v2f_tap o;

		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv20 = v.texcoord + _MainTex_TexelSize.xy;
		o.uv21 = v.texcoord + _MainTex_TexelSize.xy * half2(-0.5h, -0.5h);
		o.uv22 = v.texcoord + _MainTex_TexelSize.xy * half2(0.5h, -0.5h);
		o.uv23 = v.texcoord + _MainTex_TexelSize.xy * half2(-0.5h, 0.5h);

#if UNITY_UV_STARTS_AT_TOP
		o.uvAddBloom = o.uv20;
		if (_MainTex_TexelSize.y < 0.0)
			o.uvAddBloom.y = 1 - o.uvAddBloom.y;
#endif
		return o;
	}

	fixed4 fragBloom(v2f_simple i) : SV_Target
	{
		//return tex2D(_Bloom, i.uv);
#if UNITY_UV_STARTS_AT_TOP
		fixed4 color = tex2D(_MainTex, i.uv2);
	return color + tex2D(_Bloom, i.uv);
#else
		fixed4 color = tex2D(_MainTex, i.uv);
	return color + tex2D(_Bloom, i.uv);
#endif
	}
		v2f_simple vertBloomRefract(appdata_img v)
	{
		v2f_simple o;

		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord;

#if UNITY_UV_STARTS_AT_TOP
		o.uv2 = v.texcoord;
		if (_MainTex_TexelSize.y < 0.0)
			o.uv.y = 1.0 - o.uv.y;
#endif
		//o.uv2.x = saturate(o.uv2.x);
		//o.uv2.y = saturate(o.uv2.y);
		return o;
	}
	fixed4 fragBloomRefract(v2f_simple i) : SV_Target
	{
#if UNITY_UV_STARTS_AT_TOP
		fixed4 refract = tex2D(_BloomRefract, i.uv);
	float2 uvOffset = (refract.rg)*0.03;
	//uvOffset.x = (refract.r - 0.5)*0.03;
	//uvOffset.y = (refract.g - 0.5)*0.03;
	fixed4 color = tex2D(_MainTex, i.uv2 + uvOffset);
	fixed4 colorBefore = tex2D(_MainTex, i.uv2);
	fixed4 oc;
	//uvOffset = uvOffset + i.uv2;
	oc.r = uvOffset.x * 1000;
	oc.g = uvOffset.y * 1000;
	oc.b = 0;
	oc.a = 1;
	//return oc;
	//return refract;
	//return color*0.5+ colorBefore*0.5;
	//return color;
	//return colorBefore;
	return color + tex2D(_Bloom, i.uv + uvOffset);
#else

		fixed4 color = tex2D(_MainTex, i.uv);
	return color + tex2D(_Bloom, i.uv);
#endif
	}

		fixed4 func_down_sample_standard(v2f_tap i) : SV_Target
	{
		fixed4 color = tex2D(_MainTex, i.uv20);
	color += tex2D(_MainTex, i.uv21);
	color += tex2D(_MainTex, i.uv22);
	color += tex2D(_MainTex, i.uv23);
	return max(color / 4 - THRESHHOLD, 0) * ONE_MINUS_THRESHHOLD_TIMES_INTENSITY;
	}
		fixed4 func_down_sample(v2f_tap i)
	{
		//return 1;
		fixed4 color = tex2D(_MainTex, i.uv20);
		//color = color * lerp(1.0, color.a, 0.9);//AlphaMask 用户输入
		//color.rgb = color.rgb*(0.3+color.a);
		//fixed4 FilterValue = fixed4(0.9, 0.6,0.6,0);
		float lum = color.r*0.3 + color.g*0.6 + color.b*0.1;
		lum *= color.a;
		lum -= THRESHHOLD;
		color *= lum;
		//color = color.a;
		//fixed4 FilterValue = THRESHHOLD;// fixed4(0.8, 0.8, 0.8, 0);
		//color = max(half4(0, 0, 0, 0), color - FilterValue); //FilterValue 用户输入
		//color.rgb = color.rgb*color.a;
		return color;
	}
	//
	fixed4 fragDownsample(v2f_tap i) : SV_Target
	{
		return func_down_sample_standard(i);
	return func_down_sample(i);
	}

		fixed4 fragDownsampleAddBloom(v2f_tap i) : SV_Target
	{
		fixed4 bloom = func_down_sample(i);
	bloom = func_down_sample_standard(i);
	float2 uv = i.uv20;
#if UNITY_UV_STARTS_AT_TOP
	uv = i.uvAddBloom;
#endif
	fixed4 bloomAdd = tex2D(_BloomAdditive, uv);
	bloom.rgb += bloomAdd.rgb*bloomAdd.a;
	//bloom.rgb = bloomAdd.a;
	//bloom.a = bloom.r*0.3 + bloom.g*0.6 + bloom.b*0.1;
	//bloom.rgb = bloom.rgb*bloom.a;
	return bloom;
	}
		//径向模糊
	uniform float _RadialBlurSampleDist;
	uniform float _RadialBlurSampleStrength;
	fixed4 fragRadialBlur(v2f_tap i) : COLOR
	{
		//计算辐射中心点位置  
		fixed2 dir = 0.5 - i.uv20;
	//计算取样像素点到中心点距离  
	fixed dist = length(dir);
	dir /= dist;
	dir *= _RadialBlurSampleDist;

	fixed4 sum = tex2D(_MainTex, i.uv20 - dir*0.01);
	sum += tex2D(_MainTex, i.uv20 - dir*0.02);
	sum += tex2D(_MainTex, i.uv20 - dir*0.03);
	sum += tex2D(_MainTex, i.uv20 - dir*0.05);
	sum += tex2D(_MainTex, i.uv20 - dir*0.08);
	sum += tex2D(_MainTex, i.uv20 + dir*0.01);
	sum += tex2D(_MainTex, i.uv20 + dir*0.02);
	sum += tex2D(_MainTex, i.uv20 + dir*0.03);
	sum += tex2D(_MainTex, i.uv20 + dir*0.05);
	sum += tex2D(_MainTex, i.uv20 + dir*0.08);
	sum *= 0.1;

	return sum;
	}

		fixed4 fragRadialBlur__(v2f_tap i) : SV_Target
	{
		//设置径向模糊的中心位置，一般来说都是图片重心（0.5，0.5）  
		//fixed2 center = fixed2(_CenterX,_CenterY);
		fixed2 center = fixed2(0.5,0.5);

	//计算像素与中心点的距离，距离越远偏移越大  
	fixed2 uv = i.uv20 - center;
	fixed3 col1 = fixed3(0,0,0);
	for (fixed j = 0; j < 8; j++)
	{
		//根据设置的level对像素进行叠加，然后求平均值  
		col1 += tex2D(_MainTex, uv*(1 - 0.01*j) + center).rgb;
	}

	fixed4 col = fixed4(col1.rgb / 8,1);

	return col;
	}

		fixed4 fragBloomWithRadialBlurCombine(v2f_simple i) : SV_Target
	{
#if UNITY_UV_STARTS_AT_TOP
		fixed4 col = tex2D(_MainTex, i.uv2);
#else
		fixed4 col = tex2D(_MainTex, i.uv);
#endif
	col = col + tex2D(_Bloom, i.uv);
	fixed4  blur = tex2D(_RadialBlurTex, i.uv);
	fixed dist = length(0.5 - i.uv);
	col = lerp(col, blur,saturate(_RadialBlurSampleStrength*dist));
	return col;
	}
		//牛逼的blur
		struct v2fBlur {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float4 uv01 : TEXCOORD1;
		float4 uv23 : TEXCOORD2;
		float4 uv45 : TEXCOORD3;
		float4 uv67 : TEXCOORD4;
		float4 uv89 : TEXCOORD5;
	};
	v2fBlur vertBlur_Tgame(appdata_img v)
	{
		fixed2 _Offsets = fixed2(_Parameter.x, _Parameter.x);
		v2fBlur o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv.xy = v.texcoord.xy;
		o.uv01 = v.texcoord.xyxy + _Offsets.xyxy * float4(1, 1, -1, -1) * _MainTex_TexelSize.xyxy / 6.0;
		o.uv23 = v.texcoord.xyxy + _Offsets.xyxy * float4(2, 2, -2, -2) * _MainTex_TexelSize.xyxy / 6.0;
		o.uv45 = v.texcoord.xyxy + _Offsets.xyxy * float4(3, 3, -3, -3) * _MainTex_TexelSize.xyxy / 6.0;
		o.uv67 = v.texcoord.xyxy + _Offsets.xyxy * float4(4, 4, -4, -4) * _MainTex_TexelSize.xyxy / 6.0;
		o.uv89 = v.texcoord.xyxy + _Offsets.xyxy * float4(5, 5, -5, -5) * _MainTex_TexelSize.xyxy / 6.0;
		return o;
	}
	float4 fragBlur_Tgame(v2fBlur i) : SV_Target
	{
		half4 color = half4 (0, 0, 0, 0);
		color += 0.225 * tex2D(_MainTex, i.uv);
		color += 0.150 * tex2D(_MainTex, i.uv01.xy);
		color += 0.150 * tex2D(_MainTex, i.uv01.zw);
		color += 0.110 * tex2D(_MainTex, i.uv23.xy);
		color += 0.110 * tex2D(_MainTex, i.uv23.zw);
		color += 0.075 * tex2D(_MainTex, i.uv45.xy);
		color += 0.075 * tex2D(_MainTex, i.uv45.zw);
		color += 0.0525 * tex2D(_MainTex, i.uv67.xy);
		color += 0.0525 * tex2D(_MainTex, i.uv67.zw);
		return color;
	}
		struct v2f_opts {
		half4 pos : SV_POSITION;
		half2 uv0 : TEXCOORD0;
		half2 uv1 : TEXCOORD1;
		half2 uv2 : TEXCOORD2;
		half2 uv3 : TEXCOORD3;
		half2 uv4 : TEXCOORD4;
		half2 uv5 : TEXCOORD5;
		half2 uv6 : TEXCOORD6;
	};
	v2f_opts vertBloomStretch(appdata_img v) {
		half stretchWidth = ONE_MINUS_THRESHHOLD_TIMES_INTENSITY;
		float s = 1.70;
		fixed2 offsets = fixed2(_Parameter.x*s, 0);//fixed2(1, 1);
		v2f_opts o;
		o.pos = UnityObjectToClipPos(v.vertex);
		half b = stretchWidth;
		o.uv0 = v.texcoord.xy;
		o.uv1 = v.texcoord.xy + b * 2.0 * offsets.xy;  //offsets用户输入权值
		o.uv2 = v.texcoord.xy - b * 2.0 * offsets.xy;
		o.uv3 = v.texcoord.xy + b * 4.0 * offsets.xy;
		o.uv4 = v.texcoord.xy - b * 4.0 * offsets.xy;
		o.uv5 = v.texcoord.xy + b * 6.0 * offsets.xy;
		o.uv6 = v.texcoord.xy - b * 6.0 * offsets.xy;
		return o;
	}
	half4 fragBloomStretch(v2f_opts i) : COLOR{
		half4 color = tex2D(_MainTex, i.uv0);
		color = max(color, tex2D(_MainTex, i.uv1));
		color = max(color, tex2D(_MainTex, i.uv2));
		color = max(color, tex2D(_MainTex, i.uv3));
		color = max(color, tex2D(_MainTex, i.uv4));
		color = max(color, tex2D(_MainTex, i.uv5));
		color = max(color, tex2D(_MainTex, i.uv6));
		return color;
	}
		// weight curves

	static const half curve[7] = { 0.0205, 0.0855, 0.232, 0.324, 0.232, 0.0855, 0.0205 };  // gauss'ish blur weights

	static const half4 curve4[7] = { half4(0.0205,0.0205,0.0205,0), half4(0.0855,0.0855,0.0855,0), half4(0.232,0.232,0.232,0),
		half4(0.324,0.324,0.324,1), half4(0.232,0.232,0.232,0), half4(0.0855,0.0855,0.0855,0), half4(0.0205,0.0205,0.0205,0) };

	struct v2f_withBlurCoords8
	{
		float4 pos : SV_POSITION;
		half4 uv : TEXCOORD0;
		half2 offs : TEXCOORD1;
	};

	struct v2f_withBlurCoordsSGX
	{
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
		half4 offs[3] : TEXCOORD1;
	};

	v2f_withBlurCoords8 vertBlurHorizontal(appdata_img v)
	{
		v2f_withBlurCoords8 o;
		o.pos = UnityObjectToClipPos(v.vertex);

		o.uv = half4(v.texcoord.xy, 1, 1);
		o.offs = _MainTex_TexelSize.xy * half2(1.0, 0.0) * _Parameter.x;

		return o;
	}

	v2f_withBlurCoords8 vertBlurVertical(appdata_img v)
	{
		v2f_withBlurCoords8 o;
		o.pos = UnityObjectToClipPos(v.vertex);

		o.uv = half4(v.texcoord.xy, 1, 1);
		o.offs = _MainTex_TexelSize.xy * half2(0.0, 1.0) * _Parameter.x;

		return o;
	}

	half4 fragBlur8(v2f_withBlurCoords8 i) : SV_Target
	{
		half2 uv = i.uv.xy;
		half2 netFilterWidth = i.offs;
		half2 coords = uv - netFilterWidth * 3.0;

		half4 color = 0;
		for (int l = 0; l < 7; l++)
		{
			half4 tap = tex2D(_MainTex, coords);
			color += tap * curve4[l];
			coords += netFilterWidth;
		}
		return color;
	}


		v2f_withBlurCoordsSGX vertBlurHorizontalSGX(appdata_img v)
	{
		v2f_withBlurCoordsSGX o;
		o.pos = UnityObjectToClipPos(v.vertex);

		o.uv = v.texcoord.xy;
		half2 netFilterWidth = _MainTex_TexelSize.xy * half2(1.0, 0.0) * _Parameter.x;
		half4 coords = -netFilterWidth.xyxy * 3.0;

		o.offs[0] = v.texcoord.xyxy + coords * half4(1.0h, 1.0h, -1.0h, -1.0h);
		coords += netFilterWidth.xyxy;
		o.offs[1] = v.texcoord.xyxy + coords * half4(1.0h, 1.0h, -1.0h, -1.0h);
		coords += netFilterWidth.xyxy;
		o.offs[2] = v.texcoord.xyxy + coords * half4(1.0h, 1.0h, -1.0h, -1.0h);

		return o;
	}

	v2f_withBlurCoordsSGX vertBlurVerticalSGX(appdata_img v)
	{
		v2f_withBlurCoordsSGX o;
		o.pos = UnityObjectToClipPos(v.vertex);

		o.uv = half4(v.texcoord.xy, 1, 1);
		half2 netFilterWidth = _MainTex_TexelSize.xy * half2(0.0, 1.0) * _Parameter.x;
		half4 coords = -netFilterWidth.xyxy * 3.0;

		o.offs[0] = v.texcoord.xyxy + coords * half4(1.0h, 1.0h, -1.0h, -1.0h);
		coords += netFilterWidth.xyxy;
		o.offs[1] = v.texcoord.xyxy + coords * half4(1.0h, 1.0h, -1.0h, -1.0h);
		coords += netFilterWidth.xyxy;
		o.offs[2] = v.texcoord.xyxy + coords * half4(1.0h, 1.0h, -1.0h, -1.0h);

		return o;
	}

	half4 fragBlurSGX(v2f_withBlurCoordsSGX i) : SV_Target
	{
		half2 uv = i.uv.xy;

		half4 color = tex2D(_MainTex, i.uv) * curve4[3];

		for (int l = 0; l < 3; l++)
		{
			half4 tapA = tex2D(_MainTex, i.offs[l].xy);
			half4 tapB = tex2D(_MainTex, i.offs[l].zw);
			color += (tapA + tapB) * curve4[l];
		}

		return color;

	}

		ENDCG

		SubShader {
		ZTest Off Cull Off ZWrite Off Blend Off

			// 0
			Pass{

			CGPROGRAM
#pragma vertex vertBloom
#pragma fragment fragBloom

			ENDCG

		}

			// 1//降解析度
			Pass{

			CGPROGRAM

#pragma vertex vert4Tap
#pragma fragment fragDownsample

			ENDCG

		}

			// 2
			Pass{
			ZTest Always
			Cull Off

			CGPROGRAM

#pragma vertex vertBlurVertical
#pragma fragment fragBlur8
			//#pragma vertex vertBlur_Tgame
			//#pragma fragment fragBlur_Tgame

			ENDCG
		}

			// 3	
			Pass{
			ZTest Always
			Cull Off

			CGPROGRAM

#pragma vertex vertBlurHorizontal
#pragma fragment fragBlur8
			//#pragma vertex vertBloomStretch
			//#pragma fragment fragBloomStretch

			ENDCG
		}

			// alternate blur
			// 4
			Pass{
			ZTest Always
			Cull Off

			CGPROGRAM

#pragma vertex vertBlurVerticalSGX
#pragma fragment fragBlurSGX

			ENDCG
		}

			// 5
			Pass{
			ZTest Always
			Cull Off

			CGPROGRAM

#pragma vertex vertBlurHorizontalSGX
#pragma fragment fragBlurSGX

			ENDCG
		}
			// 6
			Pass{

			CGPROGRAM
#pragma vertex vertBloomRefract
#pragma fragment fragBloomRefract

			ENDCG

		}
			//7
			Pass{

			CGPROGRAM

#pragma vertex vert4Tap
#pragma fragment fragDownsampleAddBloom

			ENDCG

		}
			//8 径向模糊
			Pass{

			CGPROGRAM

#pragma vertex vert4Tap
#pragma fragment fragRadialBlur

			ENDCG

		}
			//9 径向模糊合并
			Pass{

			CGPROGRAM

#pragma vertex vertBloom
#pragma fragment fragBloomWithRadialBlurCombine

			ENDCG

		}

			// 10 高斯模糊
			Pass{
			ZTest Always
			Cull Off

			CGPROGRAM

#pragma vertex vertBlurVertical
#pragma fragment fragBlur8

			ENDCG
		}

			// 11	高斯模糊
			Pass{
			ZTest Always
			Cull Off

			CGPROGRAM

#pragma vertex vertBlurHorizontal
#pragma fragment fragBlur8

			ENDCG
		}
	}

	FallBack Off
}
