Shader "Custom/MoneyJar"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

		// money jar properties
		_MoneyTex("Money Texture", 2D) = "yellow" {}
		_LineColour ("Line Colour", Color) = (1, 1, 0, 1)
		_MoneyAmount ("Money Amount", Range(0, 1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _MoneyTex;

        struct Input
        {
            float2 uv_MainTex;
			float4 vertexPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color, _LineColour;

		half _MoneyAmount;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.vertexPos = v.vertex;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
			fixed4 c;

			if ((IN.vertexPos.y + 1) / 2 <= _MoneyAmount)
				c = tex2D(_MoneyTex, IN.uv_MainTex) * _Color;
			else
				c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			float distanceToLine = (IN.vertexPos.y + 1) / 2 - _MoneyAmount;

			if (distanceToLine < 0.01 && distanceToLine > 0)
				o.Emission = _LineColour;

			o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
