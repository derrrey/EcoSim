using UnityEngine;
using System;

public static class NoiseMap
{
    // Generates and returns a noise map
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int octaves, float persistance, float lacunarity, AnimationCurve animationCurve)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float minLocalValue = float.MaxValue;
        float maxLocalValue = float.MinValue;

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
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                noiseMap[xCoord, yCoord] = noiseHeight;

                if(noiseHeight > maxLocalValue)
                {
                    maxLocalValue = noiseHeight;
                }
                else if(noiseHeight < minLocalValue)
                {
                    minLocalValue = noiseHeight;
                }
            }
        }

        for (int yCoord = 0; yCoord < mapHeight; ++yCoord)
        {
            for (int xCoord = 0; xCoord < mapWidth; ++xCoord)
            {
                noiseMap[xCoord, yCoord] = animationCurve.Evaluate(Mathf.InverseLerp(minLocalValue, maxLocalValue, noiseMap[xCoord, yCoord]));
            }
        }

        return noiseMap;
    }
}
