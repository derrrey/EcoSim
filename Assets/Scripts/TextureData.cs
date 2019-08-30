using UnityEngine;

[CreateAssetMenu()]
public class TextureData : UpdatableData
{
    public Color[] colors;
    [Range(0.0f, 1.0f)]
    public float[] heights;
    [Range(0.0f, 1.0f)]
    public float[] blendings;

    public void ApplyToMaterial(Material material, float minHeight, float maxHeight)
    {
        material.SetInt("_NumColors", colors.Length);
        material.SetColorArray("_Colors", colors);
        material.SetFloatArray("_Heights", heights);
        material.SetFloatArray("_Blendings", blendings);
        material.SetFloat("_MinHeight", minHeight);
        material.SetFloat("_MaxHeight", maxHeight);
    }
}
