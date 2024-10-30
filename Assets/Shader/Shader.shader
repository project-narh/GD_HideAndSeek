Shader "Custom/ReplaceNonBlackWithColor_StrongColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ReplaceColor ("Replace Color", Color) = (1, 0, 0, 1)
        _EmissionStrength ("Emission Strength", Range(0, 1)) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            fixed4 _ReplaceColor;
            float _EmissionStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // ���� ���� �� ��� ���
                fixed3 lightDir = normalize(float3(0.5, 0.5, -0.5));
                float NdotL = max(0, dot(i.worldNormal, lightDir));

                // �ؽ�ó�� ���� ����
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // ���� ������ ���� ��ü ���� ����
                fixed4 finalColor;
                if (texColor.r > 0.05 || texColor.g > 0.05 || texColor.b > 0.05)
                {
                    // ��ü ������ ���ϰ� �����ϰ� ���� ���� �߰�
                    finalColor = _ReplaceColor * NdotL;
                }
                else
                {
                    // ��ο� ���������� �⺻ �ؽ�ó�� �߱� ȿ�� �߰�
                    finalColor = texColor * NdotL;
                    finalColor.rgb += _EmissionStrength * texColor.rgb;
                }

                return finalColor;
            }
            ENDCG
        }
    }
}
