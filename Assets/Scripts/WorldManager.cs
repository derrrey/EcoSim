using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap, Mesh };
    [SerializeField]
    GameObject meshObject = null;

    // World options
    [Header("World Options")]
    public DrawMode drawMode;
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
    public TerrainType[] regions;

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

        Color[] colorMap = new Color[worldWidth * worldHeight];
        for (int y = 0; y < worldHeight; ++y)
        {
            for(int x = 0; x < worldWidth; ++x)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; ++i)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * worldWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();
        if(drawMode == DrawMode.NoiseMap)
        {
            mapDisplay.DrawTexture(mapDisplay.TextureFromNoiseMap(noiseMap));
        }
        else if(drawMode == DrawMode.ColorMap)
        {
            mapDisplay.DrawTexture(mapDisplay.TextureFromColorMap(colorMap, worldWidth, worldHeight));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            Mesh mesh = MeshGenerator.GenerateMesh(noiseMap).CreateMesh();
            MeshCollider meshCollider = meshObject.GetComponent<MeshCollider>();
            if (meshCollider == null)
            {
                meshCollider = meshObject.AddComponent<MeshCollider>();
            }
            meshCollider.sharedMesh = mesh;
            mapDisplay.DrawMesh(mesh, mapDisplay.TextureFromColorMap(colorMap, worldWidth, worldHeight));
        }
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
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
