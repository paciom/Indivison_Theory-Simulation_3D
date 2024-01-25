Shader "Custom/Billboard" {
    Properties {
        _MainTex("Texture", 2D) = "white" {}
    }

    SubShader {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma vertex vert_billboard
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fog

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v) {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            v2f vert_billboard(appdata v) {
                v2f o;
                o.uv = v.uv;
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

                float4x4 viewMatrix = unity_CameraInvViewMatrix;
                float3 billboardRight = float3(viewMatrix.m00, viewMatrix.m10, viewMatrix.m20);
                float3 billboardUp = float3(viewMatrix.m01, viewMatrix.m11, viewMatrix.m21);

                float3 adjustedPosition = worldPos.xyz - _WorldSpaceCameraPos;
                float3 rotatedPosition = _WorldSpaceCameraPos + billboardRight * adjustedPosition.x + billboardUp * adjustedPosition.y;

                o.vertex = UnityObjectToClipPos(rotatedPosition);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }

            ENDCG
        }
    }
}
