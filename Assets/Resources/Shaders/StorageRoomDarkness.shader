Shader "UI/StorageRoomDarkness"
{
    Properties
    {
        _LightCenter ("Light Center", Vector) = (0.5, 0.5, 0, 0)
        _LightRadius ("Light Radius", Float) = 0.15
        _EdgeSoftness ("Edge Softness", Float) = 0.05
        _LightIntensity ("Light Intensity", Range(0, 1)) = 1
        _DarknessOpacity ("Darkness Opacity", Range(0, 1)) = 0.92
        _ScreenAspect ("Screen Aspect", Float) = 1.7778
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float4 _LightCenter;
            float _LightRadius;
            float _EdgeSoftness;
            float _LightIntensity;
            float _DarknessOpacity;
            float _ScreenAspect;

            v2f vert(appdata input)
            {
                v2f output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                return output;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                // Fragment SV_POSITION is the actual render-target pixel coordinate. Convert
                // it to the same bottom-left coordinate system used by WorldToScreenPoint.
                float2 screenUV = input.vertex.xy / _ScreenParams.xy;
                #if UNITY_UV_STARTS_AT_TOP
                    screenUV.y = 1.0 - screenUV.y;
                #endif
                float2 delta = screenUV - _LightCenter.xy;
                delta.x *= _ScreenAspect;
                float distanceFromPlayer = length(delta);
                float innerRadius = max(0.0, _LightRadius - _EdgeSoftness);
                float lightAmount = 1.0 - smoothstep(innerRadius, _LightRadius,
                    distanceFromPlayer);
                float alpha = _DarknessOpacity * (1.0 - lightAmount * _LightIntensity);
                return fixed4(0.0, 0.0, 0.0, alpha);
            }
            ENDCG
        }
    }
}
