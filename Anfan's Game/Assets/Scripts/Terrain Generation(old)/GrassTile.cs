using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GrassTile : Tile
{
    [SerializeField]
    private Sprite[] grassSprites;

    [SerializeField]
    private Sprite preview;



    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        int randomVal = Random.Range(0, grassSprites.Length);
        tileData.sprite = grassSprites[randomVal];

    }



#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/PixelGrassTile")]
    public static void CreatePixelGrassTile() {
        string path = EditorUtility.SaveFilePanelInProject("Save GrassTile", "New GrassTile", "asset", "Save GrassTile", "Assets");
        if (path == "") {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GrassTile>(), path);

    }

#endif









}
