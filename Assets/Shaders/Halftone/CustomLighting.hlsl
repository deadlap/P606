// Inspired by Ned Makes Games' Custom Unity Shader Light
// https://nedmakesgames.medium.com/creating-custom-lighting-in-unitys-shader-graph-with-universal-render-pipeline-5ad442c27276

// This prevents the script from being compiled twice
#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

// What data would this custom light include?
struct CustomLightingData
{
    // Position and orientation
    float3 positionWS;
    float3 normalWS;
    float3 viewDirectionWS;
    float4 shadowCoord;
    
    // Surface attributes
    float3 albedo;
    float smoothness;
};

// Translate a [0, 1] smoothness value to an exponent
float GetSmoothnessPower(float rawSmoothness)
{
    return exp2(10 * rawSmoothness + 1);
}

#ifndef SHADERGRAPH_PREVIEW
// Calculates diffuse lighting, both color and normals
float3 CustomLightHandling(CustomLightingData d, Light light)
{
    float3 radiance = light.color * (light.distanceAttenuation * light.shadowAttenuation);
    
    float diffuse = saturate(dot(d.normalWS, light.direction));
    float specularDot = saturate(dot(d.normalWS, normalize(light.direction + d.viewDirectionWS)));
    float specular = pow(specularDot, GetSmoothnessPower(d.smoothness)) * diffuse;
    
    float3 color = d.albedo * radiance * (diffuse + specular);
    
    return color;
}
#endif

// Return the color for the current pixel of custom lighting based upon the input data
float3 CalculateCustomLighting(CustomLightingData d)
{
#ifdef SHADERGRAPH_PREVIEW
    // In preview, estimate diffuse + specular
    float3 lightDir = float3(0.5, 0.5, 0);
    float intensity = saturate(dot(d.normalWS, lightDir)) +
        pow(saturate(dot(d.normalWS, normalize(d.viewDirectionWS + lightDir))), GetSmoothnessPower(d.smoothness));
    return d.albedo * intensity;
#else
    // Get main light. Located in Universal RP/ShaderLibrary/Lighting.hlsl
    Light mainLight = GetMainLight(d.shadowCoord, d.positionWS, 1);
    
    float3 color = 0;
    // Shade the main light
    color += CustomLightHandling(d, mainLight);
    
    #ifdef _ADDITIONAL_LIGHTS
        // Shade additional cone and point lights. Functions in URP/ShaderLibrary/Lighting.hlsl
        uint numAdditionalLights = GetAdditionalLightsCount();
        for (uint lightI = 0; lightI < numAdditionalLights; lightI++)
        {
            Light light = GetAdditionalLight(lightI, d.positionWS, 1);
            color += CustomLightHandling(d, light);
        }
    #endif
    
    return color;
#endif
}

// Above as a function that can be called from the ShaderGraph
void CalculateCustomLighting_float(float3 Position, float3 Normal, float3 ViewDirection,
    float3 Albedo, float Smoothness,
    out float3 Color)
{

    CustomLightingData d;
    d.positionWS = Position;
    d.normalWS = Normal;
    d.viewDirectionWS = ViewDirection;
    d.albedo = Albedo;
    d.smoothness = Smoothness;
    
#ifdef SHADERGRAPH_PREVIEW
    // In preview, there's no shadows or bakedGI
    d.shadowCoord = 0;
#else
    // Calculate the main light shadow coord
    // There are two types depending on if cascades are enabled
    #if SHADOWS_SCREEN
        float4 positionCS = TransformWorldToHClip(Position);
        d.shadowCoord = ComputeScreenPos(positionCS);
    #else
        d.shadowCoord = TransformWorldToShadowCoord(Position);
    #endif
#endif

    Color = CalculateCustomLighting(d);
}

#endif