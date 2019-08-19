using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AnfanTileMap : MonoBehaviour
{
    private float[,] terrainNoiseMap;
    public Tilemap tileMap;
    public Tilemap treeMap;
    public Tilemap waterMap;

    public int mapWidth;
    public int mapHeight;
    private int seed;
    public float noiseScale;

    [Range(0, 10)]
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public Vector2 offset;
    public bool autoUpdate;

    public TileType[] tileTypes;
    public Tile waterTile;

    public Tile appleTree;
    public Tile woodTree;
    public Tile dfruitTree;
    public Tile ironRock;
    public Tile goldRock;
    public Tile rubyRock;
    public Tile sapphireRock;
    public Tile diamondRock;

    public void Awake()
    {
        seed = Random.Range(0, 10000);
        DisplayMap();
        DisplayResources();
    }


    public void Start()
    {
        //seed = Random.Range(0, 10000);
        //DisplayMap();
        //DisplayResources();
        
    }

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
                    //set a specific tile
                    if (currentHeight <= tileTypes[i].height)
                    {
                        Tile tile = tileTypes[i].tile;
                        Vector3Int pos = new Vector3Int(x, y, 0);

                        // sets the tile on the groundmap
                        tileMap.SetTile(pos, tile);

                        // sets the watertile on the watermap
                        if (tile.name == "water_plain_tile")
                        {
                            waterMap.SetTile(pos, waterTile);
                        }

                        break;

                        
                    }
                }

            }
        }
    }


    public void DisplayResources()
    {
        terrainNoiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves,
            persistance, lacunarity, offset);

        treeMap.ClearAllTiles();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float currentHeight = terrainNoiseMap[x, y];

                // On sand tiles (index 1)
                if (currentHeight > tileTypes[0].height && currentHeight <= tileTypes[1].height)
                {
                    int random = Random.Range(0, 100);
                    if (random < 1)
                    {
                        treeMap.SetTile(new Vector3Int(x, y, 0), ironRock);
                    }
                    else if (random >= 1 && random < 2)
                    {
                        treeMap.SetTile(new Vector3Int(x, y, 0), goldRock);
                    }
                    else if (random >= 2 && random < 3)
                    {
                        treeMap.SetTile(new Vector3Int(x, y, 0), rubyRock);
                    }
                    else if (random >= 3 && random < 4)
                    {
                        treeMap.SetTile(new Vector3Int(x, y, 0), sapphireRock);
                    }
                    else if (random >= 4 && random < 5)
                    {
                        treeMap.SetTile(new Vector3Int(x, y, 0), diamondRock);
                    }


                }

                // On grass tiles (index 2)
                if (currentHeight > tileTypes[1].height && currentHeight <= tileTypes[2].height)
                {
                    int random = Random.Range(0, 200);
                    if (random < 2)
                    {
                        treeMap.SetTile(new Vector3Int(x, y, 0), appleTree);
                    }
                    else if (random >= 2 && random < 5)
                    {
                        treeMap.SetTile(new Vector3Int(x, y, 0), woodTree);
                    }
                    else if (random >= 5 && random < 6)
                    {
                        treeMap.SetTile(new Vector3Int(x, y, 0), dfruitTree);
                    }

                }

                // On rock tiles (index 3)
                else if (currentHeight > tileTypes[2].height && currentHeight <= tileTypes[3].height)
                {
                    int random = Random.Range(0, 100);
                    if (random < 2)
                    {
                        treeMap.SetTile(new Vector3Int(x, y, 0), ironRock);
                    }
                    else if (random >= 2 && random < 4)
                    {
                        treeMap.SetTile(new Vector3Int(x, y, 0), goldRock);
                    }
                    else if (random >= 4 && random < 6)
                    {
                        treeMap.SetTile(new Vector3Int(x, y, 0), rubyRock);
                    }
                    else if (random >= 6 && random < 8)
                    {
                        treeMap.SetTile(new Vector3Int(x, y, 0), sapphireRock);
                    }
                    else if (random >= 8 && random < 10)
                    {
                        treeMap.SetTile(new Vector3Int(x, y, 0), diamondRock);
                    }

                }




            }
        }
    }

#if (UNITY_EDITOR)
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
                        treeMap.SetEditorPreviewTile(new Vector3Int(x, y, 0), appleTree);
                    }

                }
                

            }
        }
    }


    public void ClearAll()
    {
        tileMap.ClearAllTiles();
        treeMap.ClearAllTiles();
        waterMap.ClearAllTiles();

        tileMap.ClearAllEditorPreviewTiles();
        treeMap.ClearAllEditorPreviewTiles();
        waterMap.ClearAllEditorPreviewTiles();
    }
#endif



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
