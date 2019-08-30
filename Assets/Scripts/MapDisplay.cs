using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public Texture2D CreateTexture(int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        return texture;
    }

    public void DrawMesh(Mesh mesh, Texture2D texture)
    {
        meshFilter.sharedMesh = mesh;
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
