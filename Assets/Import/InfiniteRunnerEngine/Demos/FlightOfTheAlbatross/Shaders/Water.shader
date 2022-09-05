Shader "More Mountains/Water" {
    Properties {
        _DeformationTexture ("DeformationTexture", 2D) = "white" {}
        _DeformationAmount ("DeformationAmount", Range(0, 0.1)) = 0.014
        _WaterTexture ("WaterTexture", 2D) = "white" {}
        _FlowSpeedX ("FlowSpeedX", Range(-1, 1)) = 0
        _FlowSpeedY ("FlowSpeedY", Range(-1, 1)) = 1
        _WaveTexture ("WaveTexture", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _DeformationTexture; uniform float4 _DeformationTexture_ST;
            uniform float _DeformationAmount;
            uniform sampler2D _WaterTexture; uniform float4 _WaterTexture_ST;
            uniform float _FlowSpeedX;
            uniform float _FlowSpeedY;
            uniform sampler2D _WaveTexture; uniform float4 _WaveTexture_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_4708 = _Time + _TimeEditor;
                float2 node_7468 = (o.uv0+node_4708.g*float2(0,1));
                float4 _WaveTexture_var = tex2Dlod(_WaveTexture,float4(TRANSFORM_TEX(node_7468, _WaveTexture),0.0,0));
                v.vertex.xyz += (float3(0,10,0)*_WaveTexture_var.rgb);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 node_4708 = _Time + _TimeEditor;
                float2 node_6374 = (i.uv0+node_4708.g*float2(0,0.1));
                float4 _DeformationTexture_var = tex2D(_DeformationTexture,TRANSFORM_TEX(node_6374, _DeformationTexture));
                float4 node_7626 = _Time + _TimeEditor;
                float2 node_3804 = (lerp(i.uv0,float2(_DeformationTexture_var.r,_DeformationTexture_var.r),_DeformationAmount)+(node_7626.g*float2(_FlowSpeedX,_FlowSpeedY)));
                float4 _WaterTexture_var = tex2D(_WaterTexture,TRANSFORM_TEX(node_3804, _WaterTexture));
                float3 diffuseColor = _WaterTexture_var.rgb;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _DeformationTexture; uniform float4 _DeformationTexture_ST;
            uniform float _DeformationAmount;
            uniform sampler2D _WaterTexture; uniform float4 _WaterTexture_ST;
            uniform float _FlowSpeedX;
            uniform float _FlowSpeedY;
            uniform sampler2D _WaveTexture; uniform float4 _WaveTexture_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_7309 = _Time + _TimeEditor;
                float2 node_7468 = (o.uv0+node_7309.g*float2(0,1));
                float4 _WaveTexture_var = tex2Dlod(_WaveTexture,float4(TRANSFORM_TEX(node_7468, _WaveTexture),0.0,0));
                v.vertex.xyz += (float3(0,10,0)*_WaveTexture_var.rgb);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 attenColor = _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 node_7309 = _Time + _TimeEditor;
                float2 node_6374 = (i.uv0+node_7309.g*float2(0,0.1));
                float4 _DeformationTexture_var = tex2D(_DeformationTexture,TRANSFORM_TEX(node_6374, _DeformationTexture));
                float4 node_7626 = _Time + _TimeEditor;
                float2 node_3804 = (lerp(i.uv0,float2(_DeformationTexture_var.r,_DeformationTexture_var.r),_DeformationAmount)+(node_7626.g*float2(_FlowSpeedX,_FlowSpeedY)));
                float4 _WaterTexture_var = tex2D(_WaterTexture,TRANSFORM_TEX(node_3804, _WaterTexture));
                float3 diffuseColor = _WaterTexture_var.rgb;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _WaveTexture; uniform float4 _WaveTexture_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 node_5136 = _Time + _TimeEditor;
                float2 node_7468 = (o.uv0+node_5136.g*float2(0,1));
                float4 _WaveTexture_var = tex2Dlod(_WaveTexture,float4(TRANSFORM_TEX(node_7468, _WaveTexture),0.0,0));
                v.vertex.xyz += (float3(0,10,0)*_WaveTexture_var.rgb);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
