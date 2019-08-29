using UnityEngine;
using System;

public static class NoiseMap
{
    // Generates and returns a noise map
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int octaves, float persistance, float lacunarity)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        for (int yCoord = 0; yCoord < mapHeight; ++yCoord)
        {
            for (int xCoord = 0; xCoord < mapWidth; ++xCoord)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; ++i)
                {
                    float sampleX = xCoord / scale * frequency;
                    float sampleY = yCoord / scale * frequency;
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                noiseMap[xCoord, yCoord] = Math.Min(noiseHeight, 1.0f);
            }
        }
        return noiseMap;
    }
}
