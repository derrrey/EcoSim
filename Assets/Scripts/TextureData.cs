using UnityEngine;
using System.Linq;

[CreateAssetMenu()]
public class TextureData : UpdatableData
{
    public Layer[] layers;

    public void ApplyToMaterial(Material material, float minHeight, float maxHeight)
    {
        Texture2DArray textureArray = CreateTextureArrayFromLayers();

        material.SetInt("_NumLayers", layers.Length);
        material.SetTexture("_TextureArray", textureArray);
        material.SetColorArray("_Colors", layers.Select(layer => layer.baseColor).ToArray());
        material.SetFloatArray("_Heights", layers.Select(layer => layer.layerHeight).ToArray());
        material.SetFloatArray("_Blendings", layers.Select(layer => layer.layerBlending).ToArray());
        material.SetFloatArray("_DrawStrengths", layers.Select(layer => layer.drawStrength).ToArray());
        material.SetFloatArray("_Scales", layers.Select(layer => layer.layerScale).ToArray());
        material.SetFloat("_MinHeight", minHeight);
        material.SetFloat("_MaxHeight", maxHeight);
    }

    Texture2DArray CreateTextureArrayFromLayers()
    {
        Texture2DArray textureArray = new Texture2DArray(layers[0].texture.width, layers[0].texture.height,
            layers.Length, layers[0].texture.format, true);
        for(int i = 0; i < layers.Length; ++i)
        {
            textureArray.SetPixels(layers[i].texture.GetPixels(), i);
        }
        textureArray.Apply();
        return textureArray;
    }
}

[System.Serializable]
public class Layer
{
    public Color baseColor;
    [Range(0.0f, 1.0f)]
    public float layerHeight;
    [Range(0.0f, 1.0f)]
    public float layerBlending;
    [Range(0.0f, 1.0f)]
    public float drawStrength;
    public float layerScale;
    public Texture2D texture;
}
