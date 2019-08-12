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

        if (GUILayout.Button("Generate Map Game"))
        {
            mapGen.DisplayMap();
        }


        if (GUILayout.Button("Generate Map GUI"))
        {
            mapGen.DisplayMapGUI();
        }

        if (GUILayout.Button("Generate Trees Game"))
        {
            mapGen.DisplayTrees();
        }


        if (GUILayout.Button("Generate Trees GUI"))
        {
            mapGen.DisplayTreesGUI();
        }

        if (GUILayout.Button("Clear All"))
        {
            mapGen.ClearAll();
        }







    }
}
