using UnityEditor;
using UnityEngine;

public class DungeonGeneratorEditor : EditorWindow
{
    private DungeonGenerator dungeonGenerator;

    [MenuItem("Window/Dungeon Generator")]
    public static void ShowWindow()
    {
        GetWindow<DungeonGeneratorEditor>("Dungeon Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Dungeon Generator Tool", EditorStyles.boldLabel);

        dungeonGenerator = EditorGUILayout.ObjectField("Dungeon Generator", dungeonGenerator, typeof(DungeonGenerator), true) as DungeonGenerator;

        if (dungeonGenerator == null)
        {
            EditorGUILayout.HelpBox("Select a DungeonGenerator component to use this tool.", MessageType.Info);
            return;
        }

        if (GUILayout.Button("Generate Dungeon", GUILayout.Height(40)))
        {
            ClearDungeon();
            GenerateDungeon();
        }

        if (GUILayout.Button("Clear Dungeon", GUILayout.Height(40)))
        {
            ClearDungeon();
        }
    }

    private void GenerateDungeon()
    {
        // Call the generation methods directly
        dungeonGenerator.rooms.Clear();
        dungeonGenerator.GenerateDungeonGraph();
        dungeonGenerator.PrintRooms(false);
        dungeonGenerator.GenerateDungeon();
    }

    private void ClearDungeon()
    {
        dungeonGenerator.floorTilemap.ClearAllTiles();
        dungeonGenerator.wallTilemap.ClearAllTiles();
        dungeonGenerator.rooms.Clear();
    }
}