using UnityEngine;
using UnityEditor;

public class BatchTreeAdder : EditorWindow
{
    private Terrain terrain;
    [SerializeField] private GameObject[] treePrefabs;

    [MenuItem("Tools/Batch Tree Adder")]
    public static void ShowWindow()
    {
        GetWindow<BatchTreeAdder>("Batch Tree Adder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Batch Tree Adder", EditorStyles.boldLabel);

        terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true);

        if (treePrefabs == null)
        {
            treePrefabs = new GameObject[0];
        }

        SerializedObject serializedObject = new SerializedObject(this);
        serializedObject.Update();
        SerializedProperty treePrefabsProperty = serializedObject.FindProperty("treePrefabs");

        if (treePrefabsProperty != null)
        {
            EditorGUILayout.PropertyField(treePrefabsProperty, new GUIContent("Tree Prefabs"), true);
        }
        else
        {
            Debug.LogError("Failed to find property 'treePrefabs'. Make sure it is properly serialized.");
        }

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Add Trees"))
        {
            AddTrees();
        }

        if (GUILayout.Button("Remove All Trees"))
        {
            RemoveAllTrees();
        }
    }

    private void AddTrees()
    {
        if (terrain == null)
        {
            Debug.LogError("Please assign a terrain.");
            return;
        }

        if (treePrefabs == null || treePrefabs.Length == 0)
        {
            Debug.LogError("Please assign at least one tree prefab.");
            return;
        }

        TerrainData terrainData = terrain.terrainData;
        var existingPrototypes = terrainData.treePrototypes;

        TreePrototype[] newPrototypes = new TreePrototype[existingPrototypes.Length + treePrefabs.Length];

        for (int i = 0; i < existingPrototypes.Length; i++)
        {
            newPrototypes[i] = existingPrototypes[i];
        }

        for (int i = 0; i < treePrefabs.Length; i++)
        {
            TreePrototype treePrototype = new TreePrototype { prefab = treePrefabs[i] };
            newPrototypes[existingPrototypes.Length + i] = treePrototype;
        }

        terrainData.treePrototypes = newPrototypes;

        Debug.Log("Trees added successfully!");
    }

    private void RemoveAllTrees()
    {
        if (terrain == null)
        {
            Debug.LogError("Please assign a terrain.");
            return;
        }

        TerrainData terrainData = terrain.terrainData;
        terrainData.treePrototypes = new TreePrototype[0];
        terrainData.RefreshPrototypes();

        Debug.Log("All trees removed from the terrain.");
    }
}
