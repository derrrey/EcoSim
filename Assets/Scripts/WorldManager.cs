using UnityEngine;

public class WorldManager : MonoBehaviour
{
    // Tiles
    [Header("Ground Tile Prefabs")]
    [SerializeField]
    Transform grassPrefab = null;
    [SerializeField]
    Transform waterPrefab = null;
    [SerializeField]
    Transform shorePrefab = null;
    Transform[] tilePrefabs;
    GameObject tileParent = null;

    [Header("Tree Prefabs")]
    [SerializeField]
    Transform oakPrefab = null;
    [SerializeField]
    Transform palmPrefab = null;
    GameObject treeParent = null;

    // World options
    [Header("World Options")]
    [SerializeField, Range(0.0f, 1.0f)]
    float waterThreshold = 0.0f;
    [SerializeField, Range(0.0f, 1.0f)]
    float shoreThreshold = 0.0f;
    [SerializeField, Range(0.0f, 1.0f)]
    float oakThreshold = 0.0f;
    [SerializeField, Range(0.0f, 1.0f)]
    float oakProb = 0.0f;
    [SerializeField, Range(0.0f, 1.0f)]
    float palmThreshold = 0.0f;
    [SerializeField, Range(0.0f, 1.0f)]
    float palmProb = 0.0f;
    static int worldSize = 64;

    // The world
    enum Tile { Grass, Water, Shore };
    struct TileWithPerlinNoise
    {
        public Tile tile;
        public float noise;
    }
    TileWithPerlinNoise[,] tileMap = new TileWithPerlinNoise[worldSize, worldSize];

    // Start is called before the first frame update
    void Start()
    {
        tileParent = new GameObject("WorldTiles");
        treeParent = new GameObject("WorldTrees");
        ConstructPrefabArray();
        GenerateTileMap();
        GenerateTrees();
        InstantiateTileMap();
    }

    // Constructs an array from the tile prefabs
    void ConstructPrefabArray()
    {
        tilePrefabs = new Transform[] { grassPrefab, waterPrefab, shorePrefab };
    }

    // Generates a tile map
    void GenerateTileMap()
    {
        for (int yCoord = 0; yCoord < worldSize; ++yCoord)
        {
            for (int xCoord = 0; xCoord < worldSize; ++xCoord)
            {
                SetTileForIndex(xCoord, yCoord);
            }
        }
    }

    // Sets a tile index based on perlin noise
    void SetTileForIndex(int xCoord, int yCoord)
    {
        float perlinNoise = Mathf.PerlinNoise(xCoord * 0.1f, yCoord * 0.1f);
        if (perlinNoise <= waterThreshold)
        {
            tileMap[xCoord, yCoord].tile = Tile.Water;
        }
        else if (perlinNoise <= shoreThreshold)
        {
            tileMap[xCoord, yCoord].tile = Tile.Shore;
        }
        else
        {
            tileMap[xCoord, yCoord].tile = Tile.Grass;
        }
        tileMap[xCoord, yCoord].noise = perlinNoise;
    }

    // Generates and instantiates trees
    void GenerateTrees()
    {
        for (int yCoord = 0; yCoord < worldSize; ++yCoord)
        {
            for (int xCoord = 0; xCoord < worldSize; ++xCoord)
            {
                float currentNoise = tileMap[xCoord, yCoord].noise;
                if(currentNoise <= palmThreshold && tileMap[xCoord, yCoord].tile != Tile.Water)
                {
                    float randomNumber = Random.value;
                    if(randomNumber >= 1.0f - palmProb)
                    {
                        InstantiateTreeOnCoordinates(palmPrefab, new Vector3(xCoord, 0, yCoord));
                    }
                }
                else if(currentNoise >= oakThreshold)
                {
                    float randomNumber = Random.value;
                    if (randomNumber >= 1.0f - oakProb)
                    {
                        InstantiateTreeOnCoordinates(oakPrefab, new Vector3(xCoord, 0, yCoord));
                    }
                }
            }
        }
    }

    // Instantiates the given tree prefab
    void InstantiateTreeOnCoordinates(Transform treePrefab, Vector3 coordinates)
    {
        Quaternion randomRotation = Random.rotation;
        randomRotation.x = 0;
        randomRotation.z = 0;
        Transform newTree = Instantiate(treePrefab, coordinates, randomRotation, treeParent.transform);
        float newScale = System.Math.Max(Random.value, 0.7f);
        newTree.localScale = new Vector3(newScale, newScale, newScale);
    }

    // Instantiates tile prefabs based on the generated tile map
    void InstantiateTileMap()
    {
        for (int tileIndex = 0; tileIndex < worldSize * worldSize; ++tileIndex)
        {
            int xCoord = tileIndex % worldSize;
            int yCoord = tileIndex / worldSize;

            Transform newTile = Instantiate(tilePrefabs[(int)tileMap[xCoord, yCoord].tile], new Vector3(xCoord, 0, yCoord), Quaternion.identity, tileParent.transform);

            SetColorBasedOnNoiseForTile(newTile, tileMap[xCoord, yCoord].noise);
        }
    }

    // Sets the v value of a tile based on the perlin noise
    void SetColorBasedOnNoiseForTile(Transform newTile, float noise)
    {
        Renderer tileRenderer = newTile.GetComponent<Renderer>();
        float colorH, colorS, colorV;
        Color.RGBToHSV(tileRenderer.material.GetColor("_Color"), out colorH, out colorS, out colorV);
        colorV -= (noise * 0.4f);
        tileRenderer.material.SetColor("_Color", Color.HSVToRGB(colorH, colorS, colorV));
    }
}
