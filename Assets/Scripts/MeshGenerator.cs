using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateMesh(float[,] noiseMap, float meshMultiplier)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float verticeY = noiseMap[x, y] * meshMultiplier;
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, verticeY, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if(verticeY > meshData.maxHeight)
                {
                    meshData.maxHeight = verticeY;
                }
                else if(verticeY < meshData.minHeight)
                {
                    meshData.minHeight = verticeY;
                }

                if(x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }
        return meshData;
    }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;
    public float minHeight;
    public float maxHeight;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        minHeight = float.MaxValue;
        maxHeight = -float.MaxValue;
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
