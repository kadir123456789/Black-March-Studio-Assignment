using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObstacleManager : EditorWindow
{
    public ObstacleData obstacleData;
    public GameObject spherePrefab; 
    GridManager gridManager;
    ObstacleData _nodeData;

    Rect _headerSection;
    Rect _bodySection;

    private Dictionary<(int, int), GameObject> sphereDictionary = new Dictionary<(int, int), GameObject>();

    [MenuItem("Window/Obstacle Designer")]
    static void OpenWindow()
    {
        ObstacleManager window = (ObstacleManager)GetWindow(typeof(ObstacleManager));
        window.minSize = new Vector2(300, 300);
        window.Show();
    }

    private void OnEnable()
    {
        _nodeData = AssetDatabase.LoadAssetAtPath<ObstacleData>("Assets/ObstacleData.asset");
        if (_nodeData == null)
        {
            Debug.Log("ObstacleData not found, creating new instance.");
            _nodeData = ScriptableObject.CreateInstance<ObstacleData>();
            AssetDatabase.CreateAsset(_nodeData, "Assets/ObstacleData.asset");
            AssetDatabase.SaveAssets();
        }
        

        // load the sphere prefab
        spherePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SpherePrefab.prefab");
        if (spherePrefab == null)
        {
            Debug.LogError("Sphere prefab not found");
        }
    }

    private void OnGUI()
    {
        DrawLayouts();
        DrawHeader();

        if (gridManager == null)
        {
            gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        }

        if (gridManager.gridGenerated == false) { return; }

        DrawBody();
    }

    void DrawLayouts()
    {
        _headerSection.x = 0;
        _headerSection.y = 0;
        _headerSection.width = Screen.width;
        _headerSection.height = 50;

        _bodySection.x = 0;
        _bodySection.y = 50;
        _bodySection.width = Screen.width;
        _bodySection.height = Screen.height - 50;
    }

    void DrawHeader()
    {
        GUILayout.BeginArea(_headerSection);
        GUILayout.Label("Press Play.");
        GUILayout.Label("Toggle nodes to activate/deactivate obstacles.");
        GUILayout.EndArea();
    }

    void DrawBody()
    {
        if (_nodeData == null)
        {
            GUILayout.Label("No ObstacleData assigned.");
            return;
        }

        GUILayout.BeginArea(_bodySection);

        for (int j = 9; j >= 0; j--)
        {
            EditorGUILayout.BeginHorizontal();

            for (int i = 0; i < 10; i++)
            {
                bool isActive = _nodeData.GetNodeState(i, j);
                bool newActiveState = GUILayout.Toggle(isActive, "(" + i + "," + j + ")", GUILayout.MaxWidth(50));

                if (newActiveState != isActive)
                {
                    _nodeData.SetNodeState(i, j, newActiveState);
                    EditorUtility.SetDirty(_nodeData);
                    AssetDatabase.SaveAssets();
                    Debug.Log($"Node {i},{j} state changed to {newActiveState}");

                    if (newActiveState)
                    {
                        gridManager.grid[i, j].NodeDeactivate();
                        gridManager.grid[i, j].isActive = false;
                        PlaceSphereOnNode(i, j); 
                    }
                    else
                    {
                        gridManager.grid[i, j].NodeActivate();
                        gridManager.grid[i, j].isActive = true;
                        RemoveSphereFromNode(i, j); 
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndArea();
    }

    void PlaceSphereOnNode(int x, int y)
    {
        if (spherePrefab != null)
        {
            Vector3 position = gridManager.grid[x, y].transform.position;
            position.y += 0.3f; 
            GameObject sphere = Instantiate(spherePrefab, position, Quaternion.identity);
            sphereDictionary[(x, y)] = sphere; 
        }
    }

    void RemoveSphereFromNode(int x, int y)
    {
        if (sphereDictionary.TryGetValue((x, y), out GameObject sphere))
        {
            Destroy(sphere); 
            sphereDictionary.Remove((x, y)); 
        }
    }
}
