Shader "Custom/TextureTransition" {
  
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_EdgeColor ("Edge Color", Color) = (1,1,1,1)
    _Blend ("Blend Value", Range(0,1)) = 0 
    _EdgeSize ("Edge Size", Range(0,1)) = 0.05
    _Noise ("Noise Texture", 2D) = "white"
	}

	SubShader {
    // So we can get alpha, use both Queue and RenderType
		Tags { }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade // alpha:fade allows a pute transparency, no lighting colors

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

    // Access to the Noie texture UVs
		struct Input {
			float2 uv_Noise;
		};

		fixed4 _Color;
		fixed4 _EdgeColor;
    float _Blend;
    float _EdgeSize;
    sampler2D _Noise;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
      // Will shift the noise texture UVs 
      fixed2 offset = _SinTime * 0.2;

      // Get the noise UVs colors, offset by sin time 
			half4 n = tex2D(_Noise, IN.uv_Noise + offset);

      // Oscillate the blend value to make it pulse 
      float blendOsc = _Blend + (_CosTime.w * 0.1);

      // Determine if we should draw any color (the blend amount plus the size of the edge band)
      float amt = step(blendOsc, n + _EdgeSize);

      // Determine whether we should draw the main color or the edge color 
      o.Albedo = lerp(_EdgeColor, _Color, step(blendOsc + _EdgeSize, n));

      // Draw transparent color if outside of the noise bounds
			o.Alpha = lerp(0, 1, amt);
		}
		ENDCG
	}
	FallBack "Diffuse"
}