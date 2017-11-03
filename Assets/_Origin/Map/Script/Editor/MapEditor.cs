using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MapGenerator))] // CustomEditor 사용할 클래스 선언 
[CanEditMultipleObjects]
public class MapEditor : Editor
{

    public override void OnInspectorGUI()
    {
        MapGenerator map = target as MapGenerator;

        if (DrawDefaultInspector())
        {
            


        }

        if (GUILayout.Button("TILE LIST RESET"))
        {
            map.tilelistReset();
        }

        if (GUILayout.Button("Generate Map"))
        {
            map.GeneratorMap();
        }

        if (GUILayout.Button("AllTileUpdate"))
        {
            //map.Update();
            map.reloadTile();
        }

        if (GUILayout.Button("ChangeTileSize"))
        {
            map.TileSizeChange();
        }

        if(GUILayout.Button("DicLoad"))
        {
            map.DicLoad();
        }
    }
}

[CustomEditor(typeof(Tile))]
[CanEditMultipleObjects]
public class TileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        Tile tile = target as Tile;
        base.OnInspectorGUI();

        if (DrawDefaultInspector())
        {
            tile.tileUpdate();


        }
        



        if (GUILayout.Button("Update"))
        {
            tile.tileUpdate();
        }

        if(GUILayout.Button("STACK"))
        {
            tile.StackTile();
        }

        if(GUILayout.Button("DeleteStack"))
        {
            tile.DeleteStackParent();
        }
    }
}
