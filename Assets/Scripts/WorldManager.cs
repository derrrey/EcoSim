using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField]
    GameObject meshObject = null;

    // World options
    [Header("World Options")]
    [SerializeField]
    int worldWidth = 0;
    [SerializeField]
    int worldHeight = 0;
    [SerializeField]
    float scale = 0.0f;
    [SerializeField]
    int octaves = 0;
    [SerializeField, Range(0.0f, 1.0f)]
    float persistance = 0.0f;
    [SerializeField]
    float lacunarity = 0.0f;
    public bool autoUpdate = false;
    [SerializeField]
    float meshMultiplier = 0.0f;
    public TextureData textureData = null;

    // The world
    float[,] noiseMap;

    // Start is called before the first frame update
    void Start()
    {
        GenerateAndDisplayMap();
    }

    public void GenerateAndDisplayMap()
    {
        noiseMap = NoiseMap.GenerateNoiseMap(worldWidth, worldHeight, scale, octaves, persistance, lacunarity);

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();
        MeshData meshData = MeshGenerator.GenerateMesh(noiseMap, meshMultiplier);
        Mesh mesh = meshData.CreateMesh();
        MeshCollider meshCollider = meshObject.GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = meshObject.AddComponent<MeshCollider>();
        }
        meshCollider.sharedMesh = mesh;
        mapDisplay.DrawMesh(mesh, mapDisplay.CreateTexture(worldWidth, worldHeight));
        textureData.ApplyToMaterial(meshObject.GetComponent<Renderer>().sharedMaterial, meshData.minHeight, meshData.maxHeight);
    }

    private void OnValidate()
    {
        if (worldWidth <= 0)
            worldWidth = 1;
        if (worldHeight <= 0)
            worldHeight = 1;
        if (octaves < 0)
            octaves = 0;
        if (lacunarity < 1)
            lacunarity = 1;
        if (textureData != null)
        {
            textureData.OnValuesUpdated -= GenerateAndDisplayMap;
            textureData.OnValuesUpdated += GenerateAndDisplayMap;
        }
    }
}
