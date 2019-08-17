using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class TreeTile : Tile
{

    // Add tree tiles to hashset of tiles considered as "blocked"
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        GameManager.MyInstance.Blocked.Add(position);

        return base.StartUp(position, tilemap, go);
    }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/TreeTile")]

    public static void CreateTreeTile() {

        string path = EditorUtility.SaveFilePanelInProject("Save Treetile", "TreeTile", "asset", "Save treetile", "Assets");
        if (path == "") {

            return;

        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TreeTile>(), path);
    }

#endif
}
