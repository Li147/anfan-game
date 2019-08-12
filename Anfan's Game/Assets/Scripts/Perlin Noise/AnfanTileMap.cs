using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AnfanTileMap : MonoBehaviour
{
    private float[,] terrainNoiseMap;
    public Tilemap tileMap;
    public Tilemap treeMap;

    public int mapWidth;
    public int mapHeight;
    public int seed;
    public float noiseScale;

    [Range(0, 10)]
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public Vector2 offset;
    public bool autoUpdate;

    public TileType[] tileTypes;
    public Tile treeTile;


    public void DisplayMap()
    {
        tileMap.ClearAllTiles();

        terrainNoiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, 
            persistance, lacunarity, offset);

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float currentHeight = terrainNoiseMap[x, y];

                for (int i = 0; i < tileTypes.Length; i++)
                {
                    if (currentHeight <= tileTypes[i].height)
                    {
                        tileMap.SetTile(new Vector3Int(x, y, 0), tileTypes[i].tile);
                        break;

                        
                    }
                }

            }
        }
    }


    public void DisplayMapGUI()
    {
        tileMap.ClearAllEditorPreviewTiles();

        terrainNoiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves,
            persistance, lacunarity, offset);

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float currentHeight = terrainNoiseMap[x, y];

                for (int i = 0; i < tileTypes.Length; i++)
                {
                    if (currentHeight <= tileTypes[i].height)
                    {
                        //tileMap.SetTile(new Vector3Int(x, y, 0), tileTypes[i].tile);
                        tileMap.SetEditorPreviewTile(new Vector3Int(x, y, 0), tileTypes[i].tile);
                        break;


                    }
                }

            }
        }
    }


    public void DisplayTrees()
    {
        terrainNoiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves,
            persistance, lacunarity, offset);

        treeMap.ClearAllTiles();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float currentHeight = terrainNoiseMap[x, y];


                if (currentHeight > tileTypes[1].height && currentHeight <= tileTypes[2].height)
                {
                    int random = Random.Range(0, 100);
                    if (random < 2)
                    {
                        treeMap.SetTile(new Vector3Int(x, y, 0), treeTile);
                    }

                }


            }
        }
    }

    public void DisplayTreesGUI()
    {
        terrainNoiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves,
            persistance, lacunarity, offset);

        treeMap.ClearAllEditorPreviewTiles();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float currentHeight = terrainNoiseMap[x, y];

            
                if (currentHeight > tileTypes[1].height && currentHeight <= tileTypes[2].height)
                {
                    int random = Random.Range(0, 100);
                    if (random < 2)
                    {
                        treeMap.SetEditorPreviewTile(new Vector3Int(x, y, 0), treeTile);
                    }

                }
                

            }
        }
    }

    public void ClearAll()
    {
        tileMap.ClearAllTiles();
        treeMap.ClearAllTiles();

        tileMap.ClearAllEditorPreviewTiles();
        treeMap.ClearAllEditorPreviewTiles();
    }

   

    private void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }

        if (mapHeight < 1)
        {
            mapHeight = 1;
        }

        if (lacunarity < 1)
        {
            lacunarity = 1; ;
        }
        if (octaves < 0)
        {
            octaves = 0; ; ;
        }
    }


}

[System.Serializable]
public struct TileType
{
    public string name;
    public float height;
    public Tile tile;
}
