using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterTile2 : Tile
{
    [SerializeField]
    private Sprite[] waterSprites;

    [SerializeField]
    private Sprite preview;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap) {

        for (int y = -1; y <= 1; y++) {

            for (int x = -1; x <= 1 ; x++) {

                // Checks for neighbor position
                Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);

                if (HasWater(tilemap, nPos)) {

                    tilemap.RefreshTile(nPos);

                }

            }

        }

        


    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {

        string composition = string.Empty;

        for (int x = -1; x <= 1; x++) {

            for (int y = -1; y <= 1; y++) {

                if (x != 0 || y != 0) {

                    if (HasWater(tilemap, new Vector3Int(position.x + x, position.y + y, position.z))) {

                        composition += "W";

                    } else {

                        composition += "G";

                    }



                }

                



            }
        }


        //GENERATE RANDOM SPRITE FOR WATER

        int randomVal = Random.Range(0, 100);

        if (randomVal < 5) {
            tileData.sprite = waterSprites[1];

        } else if (randomVal >= 5 && randomVal < 10) {

            tileData.sprite = waterSprites[2];

        } else if (randomVal >= 10 && randomVal < 15) {

            tileData.sprite = waterSprites[3];

        } else if (randomVal >= 15 && randomVal < 20) {

            tileData.sprite = waterSprites[4];

        } else if (randomVal >= 20 && randomVal < 25) {

            tileData.sprite = waterSprites[5];

        } else if (randomVal >= 25 && randomVal < 30) {

            tileData.sprite = waterSprites[6];

        } else if (randomVal >= 25 && randomVal < 30) {

            tileData.sprite = waterSprites[7];

        } else {

            tileData.sprite = waterSprites[0];
        }



        // WATER TILE CONDITIONALS

        if (composition == "GGGWGWWG") {

            tileData.sprite = waterSprites[10];

        } else if(composition == "WWGWGWWG") {

            tileData.sprite = waterSprites[11];

        } else if (composition == "WWGWGGGG") {

            tileData.sprite = waterSprites[12];

        } else if (composition == "GGGWWWWW") {

            tileData.sprite = waterSprites[13];

        } else if (composition == "WWWWWGGG") {

            tileData.sprite = waterSprites[14];

        } else if (composition == "GGGGWGWW") {

            tileData.sprite = waterSprites[15];

        } else if (composition == "GWWGWGWW") {

            tileData.sprite = waterSprites[16];

        } else if (composition == "GWWGWGGG") {

            tileData.sprite = waterSprites[17];

        }

        
      

        






    }

    private bool HasWater(ITilemap tilemap, Vector3Int position) {

        return tilemap.GetTile(position) == this;

    }















#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/WaterTile2")]

public static void CreateWaterTile() {

        string path = EditorUtility.SaveFilePanelInProject("Save WaterTile2", "WaterTile2", "asset", "Save watertile", "Assets");
        if (path == "") {

            return;

        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WaterTile2>(), path);
    }

#endif
}
