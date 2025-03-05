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
    float smoothness;
};

// Translate a [0, 1] smoothness value to an exponent
float GetSmoothnessPower(float rawSmoothness)
{
    return exp2(10 * rawSmoothness + 1);
}

#ifndef SHADERGRAPH_PREVIEW
// Calculates diffuse lighting, both color and normals
void CustomLightHandling(CustomLightingData d, Light light, out float diffuse, out float specular, out float3 color, float attenuation)
{
    attenuation = light.distanceAttenuation * light.shadowAttenuation;
    
    diffuse = saturate((dot(d.normalWS, light.direction) * 0.5 + 0.5) * attenuation);
    float specularDot = saturate(dot(d.normalWS, normalize(light.direction + d.viewDirectionWS)));
    specular = pow(specularDot, GetSmoothnessPower(d.smoothness)) * diffuse;
    
    color = light.color;
}
#endif

// Return the color for the current pixel of custom lighting based upon the input data
void CalculateCustomLighting(CustomLightingData d, out float diffuse, out float specular, out float3 color)
{
#ifdef SHADERGRAPH_PREVIEW
    // In preview, estimate diffuse + specular
    float3 lightDir = float3(0.5, 0.5, 0);
        
    diffuse = saturate(dot(d.normalWS, lightDir) * 0.5 + 0.5);
    
    float specularDot = saturate(dot(d.normalWS, normalize(lightDir + d.viewDirectionWS)));
    specular = pow(specularDot, GetSmoothnessPower(d.smoothness)) * diffuse;
    
    color = (diffuse + specular);
#else
    // Get main light. Located in Universal RP/ShaderLibrary/Lighting.hlsl
    Light mainLight = GetMainLight(d.shadowCoord, d.positionWS, 1);
    
    diffuse = 0;
    specular = 0;
    color = 0;
    // Shade the main light
    float thisDiffuse = 0;
    float thisSpecular = 0;
    float3 thisColor = 0;
    float thisAttenuation = 0;
    CustomLightHandling(d, mainLight, thisDiffuse, thisSpecular, thisColor, thisAttenuation);
    diffuse += thisDiffuse;
    specular += thisSpecular;
    color = thisColor;
    
    float highestAttenuation = thisDiffuse;
    
    #ifdef _ADDITIONAL_LIGHTS
        // Shade additional cone and point lights. Functions in URP/ShaderLibrary/Lighting.hlsl
        uint numAdditionalLights = GetAdditionalLightsCount();
        for (uint lightI = 0; lightI < numAdditionalLights; lightI++)
        {
            Light light = GetAdditionalLight(lightI, d.positionWS, 1);
    
            CustomLightHandling(d, light, thisDiffuse, thisSpecular, thisColor, thisAttenuation);
            diffuse += thisDiffuse;
            specular += thisSpecular;
    
            if (thisDiffuse > highestAttenuation)
            {
                highestAttenuation = thisDiffuse;
                color = thisColor;
            }
        }
#endif
    
    
#endif
}

// Above as a function that can be called from the ShaderGraph
void CalculateCustomLighting_float(float3 Position, float3 Normal, float3 ViewDirection,
    float Smoothness,
    out float Diffuse, out float Specular, out float3 Color)
{

    CustomLightingData d;
    d.positionWS = Position;
    d.normalWS = Normal;
    d.viewDirectionWS = ViewDirection;
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

    CalculateCustomLighting(d, Diffuse, Specular, Color);
}

#endif