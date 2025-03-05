#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

void MainLight_float(float3 worldPos,
    out float3 direction, out float3 color, out float shadowAtten)
{
#ifdef SHADERGRAPH_PREVIEW
    direction = normalize(float3(-0.7, 0.7, -0.7));
    color = float3(1, 1, 1);
    shadowAtten = 1;
#else
    #if defined(UNIVERSAL_PIPELINE_CORE_INCLUDED) // Ensure we use URP
        float4 shadowCoord = TransformWorldToShadowCoord(worldPos);
        Light mainLight = GetMainLight(shadowCoord);
        direction = mainLight.direction;
        color = mainLight.color;
        shadowAtten = mainLight.shadowAttenuation;
    #else
        direction = normalize(float3(-0.7, 0.7, -0.7));
        color = float3(1, 1, 1);
        shadowAtten = 1;    
    #endif
#endif
}

void AddAdditionalLights_float(float smoothness, float3 worldPosition, float3 worldNormal, float3 worldView,
    float mainDiffuse, float3 mainSpecular, float3 mainColor,
    out float diffuse, out float3 specular, out float3 color)
{
    diffuse = mainDiffuse;
    specular = mainSpecular;
    color = (mainDiffuse + mainSpecular) * mainColor;
    
#ifndef SHADERGRAPH_PREVIEW
    #if defined(UNIVERSAL_PIPELINE_CORE_INCLUDED) // Ensure we use URP
        uint pixelLightCount = GetAdditionalLightsCount();
        
        //// This should allow for Forward+ rendering (It does not)
        //InputData inputData = (InputData)0;
        //inputData.positionWS = worldPosition;
    
        LIGHT_LOOP_BEGIN(pixelLightCount);
            // Get light color and direction
            Light light = GetAdditionalLight(lightIndex, worldPosition); // Possibly replace above two with this instead for Forward+
    
            // Calculate shadows
            light.shadowAttenuation = AdditionalLightRealtimeShadow(lightIndex, worldPosition, light.direction);
            float atten = light.distanceAttenuation * light.shadowAttenuation;
            
            // Calculate diffuse and specular
            // Redo this calculation to be half Lambert instead probably
            float NdotL = saturate(dot(worldNormal, light.direction));
            float thisDiffuse = atten * NdotL;
            float3 thisSpecular = LightingSpecular(thisDiffuse, light.direction, worldNormal, worldView, 1, smoothness);
    
            // Accumalate light
            diffuse += thisDiffuse;
            specular += thisSpecular;
            color += (thisDiffuse + thisSpecular) * light.color;
        LIGHT_LOOP_END
    
        // Exposure doesn't get blown out. Final color not oversaturated into white
        float total = diffuse + dot(specular, float3(0.333, 0.333, 0.333)); // Get total diffuse + desaturated specular
        color = total <= 0 ? mainColor : color / total;
#endif
#endif
}

// Same as above, but using halfs instead of floats
void MainLight_half(half3 worldPos,
    out half3 direction, out half3 color, out half shadowAtten)
{
#ifdef SHADERGRAPH_PREVIEW
    direction = normalize(half3(-0.7, 0.7, -0.7));
    color = half3(1, 1, 1);
    shadowAtten = 1;
#else
    #if defined(UNIVERSAL_PIPELINE_CORE_INCLUDED) // Ensure we use URP
        half4 shadowCoord = TransformWorldToShadowCoord(worldPos);
        Light mainLight = GetMainLight(shadowCoord);
        direction = mainLight.direction;
        color = mainLight.color;
        shadowAtten = mainLight.shadowAttenuation;
    #else
        direction = normalize(half3(-0.7, 0.7, -0.7));
        color = half3(1, 1, 1);
        shadowAtten = 1;   
    #endif
#endif
}

void AddAdditionalLights_half(half smoothness, half3 worldPosition, half3 worldNormal, half3 worldView,
    half mainDiffuse, half3 mainSpecular, half3 mainColor,
    out half diffuse, out half3 specular, out half3 color)
{
    diffuse = mainDiffuse;
    specular = mainSpecular;
    color = (mainDiffuse + mainSpecular) * mainColor;
    
#ifndef SHADERGRAPH_PREVIEW
    #if defined(UNIVERSAL_PIPELINE_CORE_INCLUDED) // Ensure we use URP
        uint pixelLightCount = GetAdditionalLightsCount();
        
        //// This should allow for Forward+ rendering (It does not)
        //InputData inputData = (InputData)0;
        //inputData.positionWS = worldPosition;
    
        LIGHT_LOOP_BEGIN(pixelLightCount);
            // Get light color and direction
            Light light = GetAdditionalLight(lightIndex, worldPosition); // Possibly replace above two with this instead for Forward+
    
            // Calculate shadows
            light.shadowAttenuation = AdditionalLightRealtimeShadow(lightIndex, worldPosition, light.direction);
            half atten = light.distanceAttenuation * light.shadowAttenuation;
            
            // Calculate diffuse and specular
            // Redo this calculation to be half Lambert instead probably
            half NdotL = saturate(dot(worldNormal, light.direction));
            half thisDiffuse = atten * NdotL;
            half3 thisSpecular = LightingSpecular(thisDiffuse, light.direction, worldNormal, worldView, 1, smoothness);
    
            // Accumalate light
            diffuse += thisDiffuse;
            specular += thisSpecular;
            color += (thisDiffuse + thisSpecular) * light.color;
        LIGHT_LOOP_END
    
        // Exposure doesn't get blown out
        half total = diffuse + dot(specular, half3(0.333, 0.333, 0.333)); // Get total diffuse + desaturated specular
        color = total <= 0 ? mainColor : color / total;
#endif
#endif
}

#endif