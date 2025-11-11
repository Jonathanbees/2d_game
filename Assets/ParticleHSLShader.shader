Shader "Custom/ParticleHSL"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1) // Color base (Blanco)
        _MainTex ("Texture", 2D) = "white" {} // Textura de partícula (por defecto blanca)
        _HueShift ("Hue Shift", Range(0, 1)) = 0 // Desplazamiento de tono (0-1)
        _Saturation ("Saturation", Range(0, 2)) = 1 // Saturación (0 = gris, 1 = normal, 2 = doble)
        _Lightness ("Lightness", Range(0, 2)) = 1 // Brillo (0 = negro, 1 = normal, 2 = doble)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha // Alpha Blending
        ZWrite Off // No escribir en el Z-Buffer (importante para partículas transparentes)
        Cull Off // No eliminar caras

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
                float4 color : COLOR; // Color de vértice de Particle System
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR; // Color de vértice pasado al fragment shader
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _HueShift;
            float _Saturation;
            float _Lightness;

            // Función de conversión RGB a HSL (simplificada)
            // Retorna HSL en el rango [0,1]
            float3 rgb2hsl(float3 c)
            {
                float M = max(c.r, max(c.g, c.b));
                float m = min(c.r, min(c.g, c.b));
                float C = M - m;

                float L = (M + m) / 2.0;
                float S = (L > 0 && L < 1) ? C / (1.0 - abs(2.0 * L - 1.0)) : 0.0;

                float H = 0;
                if (C > 0)
                {
                    if (M == c.r) H = fmod((c.g - c.b) / C, 6.0);
                    else if (M == c.g) H = (c.b - c.r) / C + 2.0;
                    else H = (c.r - c.g) / C + 4.0;
                    H /= 6.0;
                }
                return float3(H, S, L);
            }

            // Función de conversión HSL a RGB (simplificada)
            float3 hsl2rgb(float3 hsl)
            {
                float C = (1.0 - abs(2.0 * hsl.z - 1.0)) * hsl.y;
                float X = C * (1.0 - abs(fmod(hsl.x * 6.0, 2.0) - 1.0));
                float m = hsl.z - C / 2.0;
                float3 rgb_prime = 0;

                if (hsl.x >= 0 && hsl.x < (1.0/6.0)) rgb_prime = float3(C, X, 0);
                else if (hsl.x >= (1.0/6.0) && hsl.x < (2.0/6.0)) rgb_prime = float3(X, C, 0);
                else if (hsl.x >= (2.0/6.0) && hsl.x < (3.0/6.0)) rgb_prime = float3(0, C, X);
                else if (hsl.x >= (3.0/6.0) && hsl.x < (4.0/6.0)) rgb_prime = float3(0, X, C);
                else if (hsl.x >= (4.0/6.0) && hsl.x < (5.0/6.0)) rgb_prime = float3(X, 0, C);
                else if (hsl.x >= (5.0/6.0) && hsl.x < 1.0) rgb_prime = float3(C, 0, X);

                return rgb_prime + m;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color; // Pasa el color de vértice
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color * i.color; // Multiplica por color de vértice

                // Convertir a HSL
                float3 hsl_color = rgb2hsl(col.rgb);

                // Aplicar Hue Shift
                hsl_color.x = fmod(hsl_color.x + _HueShift, 1.0); // Asegura que Hue se mantenga en 0-1

                // Aplicar Saturación
                hsl_color.y *= _Saturation;

                // Aplicar Lightness
                hsl_color.z *= _Lightness;

                // Convertir de nuevo a RGB
                col.rgb = hsl2rgb(hsl_color);

                return col;
            }
            ENDCG
        }
    }
}