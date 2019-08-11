using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnfanTileMap))]
public class AnfanGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AnfanTileMap mapGen = (AnfanTileMap)target;

        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.DisplayMapGUI();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.DisplayMapGUI();
        }
    }
}
