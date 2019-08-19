using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlainWaterTile : Tile
{
    [SerializeField]
    private Sprite[] waterSprites;


    // Allows us to generate colliders on our water tiles
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        GameManager.MyInstance.Blocked.Add(position);

        return base.StartUp(position, tilemap, go);
    }



    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {

        base.GetTileData(position, tilemap, ref tileData);

        //GENERATE RANDOM SPRITE FOR WATER

        int randomVal = Random.Range(0, 300);

        if (randomVal < 2) {
            tileData.sprite = waterSprites[1];

        } else if (randomVal >= 2 && randomVal < 4) {

            tileData.sprite = waterSprites[2];

        } else if (randomVal >= 4 && randomVal < 6) {

            tileData.sprite = waterSprites[3];

        } else if (randomVal >= 6 && randomVal < 8) {

            tileData.sprite = waterSprites[4];

        } else if (randomVal >= 8 && randomVal < 10) {

            tileData.sprite = waterSprites[5];

        } else if (randomVal >= 10 && randomVal < 12) {

            tileData.sprite = waterSprites[6];

        } else if (randomVal >= 12 && randomVal < 14) {

            tileData.sprite = waterSprites[7];

        } else {

            tileData.sprite = waterSprites[0];
        }


    }

    private bool HasWater(ITilemap tilemap, Vector3Int position) {

        return tilemap.GetTile(position) == this;

    }




#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/PlainWaterTile")]

public static void CreateWaterTile() {

        string path = EditorUtility.SaveFilePanelInProject("Save PlainWaterTile", "PlainWaterTile", "asset", "Save PlainWatertile", "Assets");
        if (path == "") {

            return;

        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<PlainWaterTile>(), path);
    }

#endif
}
