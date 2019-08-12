using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class GrassTile : Tile
{
    [SerializeField]
    private Sprite[] grassSprites;

    [SerializeField]
    private Sprite preview;

    // Allows us to generate colliders on our water tiles
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {

        return base.StartUp(position, tilemap, go);
    }



    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {

        base.GetTileData(position, tilemap, ref tileData);


        //GENERATE RANDOM SPRITE FOR GRASS

        int randomVal = Random.Range(0, 100);

        if (randomVal < 7)
        {
            int remainder = randomVal % (grassSprites.Length);

            tileData.sprite = grassSprites[remainder];
        }
        else
        {
            tileData.sprite = grassSprites[0];
        }


        

        
  


    }





#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/GrassTile")]

    public static void CreateGrassTile()
    {

        string path = EditorUtility.SaveFilePanelInProject("Save GrassTile", "GrassTile", "asset", "Save grasstile", "Assets");
        if (path == "")
        {
            return;

        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GrassTile>(), path);
    }

#endif
}
