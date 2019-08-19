using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if (UNITY_EDITOR)
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

        if (GUILayout.Button("Generate Resources Game"))
        {
            mapGen.DisplayResources();
        }


        if (GUILayout.Button("Generate Resources GUI"))
        {
            mapGen.DisplayTreesGUI();
        }

        if (GUILayout.Button("Clear All"))
        {
            mapGen.ClearAll();
        }







    }
}
#endif