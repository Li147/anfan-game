using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class NoDiagonalTile : Tile
{

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/NoDiagonalTile")]

    public static void CreateTreeTile() {

        string path = EditorUtility.SaveFilePanelInProject("Save NoDiagonalTile", " NoDiagonalTile", "asset", "Save  NoDiagonalTile", "Assets");
        if (path == "") {

            return;

        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<NoDiagonalTile>(), path);
    }

#endif
}
