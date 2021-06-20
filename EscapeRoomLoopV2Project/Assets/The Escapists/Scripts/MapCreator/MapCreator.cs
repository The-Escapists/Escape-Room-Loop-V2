#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCreator : MonoBehaviour
{
    [InlineEditor]
    public MapData currentMapData;

    public static MapCreator instance;

    [SerializeField]
    TMP_InputField mapNameInputField;
    public GameObject tilePrefab;
    public GameObject selectableTilePrefab;
    public Sprite selectionFrameSprite;
    public LayerMask layerMask;
    public string brushPrefabName;
    public int[] avalableBrushRotations = new int[] { 0, 90, 180, -90 };
    [SerializeField]
    TMP_Dropdown rotationDropdown;
    //[HideInInspector]
    public Dictionary<string, PrefabDescription> availablePrefabs;
    [SerializeField]
    Transform LayerListHolder;
    [SerializeField]
    int selectedLayer = 0;
    [SerializeField]
    TMP_InputField addLayerName;
    [SerializeField]
    GameObject layerElementPrefab;

    public Transform BrushListHolder;
    public GameObject brushListElement;

    public Color selectedColor;
    public Color normalColor;

    Vector2Int newMapSize;
    CameraController cameraController;

    public enum ToolType { Select, SinglePaint, FillPaint, WallPaint }
    ToolType currentTool;

    public GameObject[] SelectionModeSettings;
    public GameObject[] PaintModeSettings;
    public GameObject[] LoadMapSettings;

    public Vector3Int selectedTilePosition;
    public TMP_InputField tileNameField;
    public GameObject fillPositionMarker;
    public Transform fillPositionMarkerTransform;
    public Vector3Int firstFillPosition;
    public bool firstPositionSet;
    public GameObject cancelFillSetting;

    public List<MapDescription> availableMaps;
    public TMP_Dropdown loadMapDropdown;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

        availablePrefabs = new Dictionary<string, PrefabDescription>();
        PrefabDescription[] prefabDescriptions = Resources.LoadAll<PrefabDescription>("Prefab Descriptions");
        foreach (PrefabDescription description in prefabDescriptions)
        {
            availablePrefabs.Add(description.prefabName, description);
        }

        rotationDropdown.ClearOptions();
        foreach (int rotation in avalableBrushRotations)
        {
            rotationDropdown.options.Add(new TMP_Dropdown.OptionData(rotation.ToString()));
        }
        rotationDropdown.RefreshShownValue();

        currentMapData = new MapData(new Vector2Int(1, 1));

        GenerateMap();
        GeneratePrefabList();
        GenerateLayerList();

         cameraController = Camera.main.GetComponent<CameraController>();

        availableMaps = new List<MapDescription>();
        availableMaps.AddRange(Resources.LoadAll<MapDescription>("Map Descriptions"));

        loadMapDropdown.ClearOptions();
        if (availableMaps.Count > 0)
        {
            List<string> mapNames = new List<string>();
            foreach (MapDescription mapDescription in availableMaps) mapNames.Add(mapDescription.mapName);
            loadMapDropdown.AddOptions(mapNames);
            loadMapDropdown.RefreshShownValue();
        }
        HideLoadMapSetting();

        mapNameInputField.SetTextWithoutNotify(currentMapData.mapName);
    }
    public void MoveLayer(int layerIndex, bool direction)
    {
        currentMapData.MoveLayer(layerIndex, direction);
        GenerateLayerList();
        GenerateMap();
    }

    public void RemoveLayer(int layerIndex)
    {
        currentMapData.RemoveLayer(layerIndex);
        GenerateLayerList();
        GenerateMap();
    }

    public void AddLayer()
    {
        if (addLayerName.text == "")
            return;

        currentMapData.AddLayer(addLayerName.text, false);
        GenerateLayerList();
        GenerateMap();
    }

    public void HideLayer(int layerIndex)
    {
        currentMapData.HideLayer(layerIndex);
        GenerateMap();
    }

    public void SelectLayer(int layerIndex)
    {
        selectedLayer = layerIndex;
        GenerateMap();
    }

    public void NewMapData()
    {
        currentMapData = new MapData(new Vector2Int(1, 1));

        GenerateMap();
        GeneratePrefabList();
        GenerateLayerList();
        HideLoadMapSetting();

        mapNameInputField.SetTextWithoutNotify(currentMapData.mapName);
    }

    public void SaveMap()
    {
        if (!Directory.Exists(Application.dataPath + "/The Escapists/Resources/Map Descriptions/"))
            Directory.CreateDirectory(Application.dataPath + "/The Escapists/Resources/Map Descriptions/");

       // Debug.Log(Application.dataPath + "/The Escapists/Resources/Map Descriptions/");

        MapDescription mapDescription = null;
        string[] assets = AssetDatabase.FindAssets("t:MapDatamapDescription",new [] { "Assets/The Escapists/Resources/Map Descriptions" });
        foreach(string guid in assets)
        {
            MapDescription description = AssetDatabase.LoadAssetAtPath<MapDescription>(AssetDatabase.GUIDToAssetPath(guid));
            if (mapDescription.mapName == currentMapData.mapName)
            {
                mapDescription = description;
                break;
            }
        }

        if (!mapDescription)
        {
            mapDescription = ScriptableObject.CreateInstance<MapDescription>();
            AssetDatabase.CreateAsset(mapDescription, "Assets/The Escapists/Resources/Map Descriptions/" + currentMapData.mapName + ".asset");
            AssetDatabase.SaveAssets();
        }

        mapDescription.Init(currentMapData);
        mapDescription.CreateInteractionGraph(mapDescription);

        EditorUtility.SetDirty(mapDescription);

        availableMaps.Clear();
        availableMaps.AddRange(Resources.LoadAll<MapDescription>("Map Descriptions"));

        loadMapDropdown.ClearOptions();
        if (availableMaps.Count > 0)
        {
            List<string> mapNames = new List<string>();
            foreach (MapDescription description in availableMaps) mapNames.Add(description.mapName);
            loadMapDropdown.AddOptions(mapNames);
            loadMapDropdown.RefreshShownValue();
        }
        HideLoadMapSetting();
    }

    public void LoadMap(int index)
    {
        currentMapData = new MapData(availableMaps[loadMapDropdown.value]);

        GenerateMap();
        GeneratePrefabList();
        GenerateLayerList();
        HideLoadMapSetting();

        mapNameInputField.SetTextWithoutNotify(currentMapData.mapName);
    }

    public void SetMapName(string name)
    {
        currentMapData.mapName = name;
    }

    public void SetMapSizeX(string value)
    {
        newMapSize.x = int.Parse(value);
    }

    public void SetMapSizeY(string value)
    {
        newMapSize.y = int.Parse(value);
    }
    
    public void SetToolType(ToolType type)
    {
        currentTool = type;
        if(type == ToolType.FillPaint || type == ToolType.SinglePaint || type == ToolType.WallPaint)
        {
            if (firstPositionSet) ResetFill();

            foreach (GameObject go in SelectionModeSettings)
            {
                go.SetActive(false);
            }
            foreach (GameObject go in PaintModeSettings)
            {
                go.SetActive(true);
            }
        }
        else if(type == ToolType.Select)
        {
            foreach (GameObject go in PaintModeSettings)
            {
                go.SetActive(false);
            }
            foreach (GameObject go in SelectionModeSettings)
            {
                go.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Error: non valid ToolType - " + type);
        }
    }

    public void ShowLoadMapSetting()
    {
        foreach (GameObject go in SelectionModeSettings)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in PaintModeSettings)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in LoadMapSettings)
        {
            go.SetActive(true);
        }
    }

    public void HideLoadMapSetting()
    {
        foreach (GameObject go in LoadMapSettings)
        {
            go.SetActive(false);
        }

        if (currentTool == ToolType.FillPaint || currentTool == ToolType.SinglePaint || currentTool == ToolType.WallPaint)
        {
            if (firstPositionSet) ResetFill();

            foreach (GameObject go in SelectionModeSettings)
            {
                go.SetActive(false);
            }
            foreach (GameObject go in PaintModeSettings)
            {
                go.SetActive(true);
            }
        }
        else if (currentTool == ToolType.Select)
        {
            foreach (GameObject go in PaintModeSettings)
            {
                go.SetActive(false);
            }
            foreach (GameObject go in SelectionModeSettings)
            {
                go.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Error: non valid ToolType - " + currentTool);
        }
    }

    public void ResetFill()
    {
        Destroy(fillPositionMarkerTransform.gameObject);
        firstFillPosition = Vector3Int.zero;
        firstPositionSet = false;
        cancelFillSetting.SetActive(false);
    }

    public void SelectTile(Vector3Int position)
    {
        tileNameField.SetTextWithoutNotify(currentMapData.mapLayers[position.z].layerTiles[position.x, position.y].tileName);
        selectedTilePosition = position;
    }

    public void SetTileName(string name)
    {
        currentMapData.mapLayers[selectedTilePosition.z].layerTiles[selectedTilePosition.x, selectedTilePosition.y].tileName = name;
    }

    public void RotateTile()
    {
        if(currentTool == ToolType.Select)
        {
            currentMapData.mapLayers[selectedTilePosition.z].layerTiles[selectedTilePosition.x, selectedTilePosition.y].tileRotation = avalableBrushRotations[rotationDropdown.value];
            RenderMap();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), cameraController.transform.forward, layerMask);
            foreach (RaycastHit2D hit in hits)
            {
                MapCreatorTile tile;
                if (hit && hit.transform.gameObject.TryGetComponent<MapCreatorTile>(out tile))
                {
                    if (currentTool == ToolType.Select)
                    {
                        SelectTile(tile.position);
                    }
                    if (currentTool == ToolType.SinglePaint)
                    {
                        Vector3Int pos = tile.position;
                        if (availablePrefabs.ContainsKey(brushPrefabName))
                        {
                            currentMapData.mapLayers[pos.z].layerTiles[pos.x, pos.y].Paint(availablePrefabs[brushPrefabName].prefabName + "l" + pos.z + "x" + pos.x + "y" + pos.y, avalableBrushRotations[rotationDropdown.value], brushPrefabName, availablePrefabs[brushPrefabName].interactionSystemDescription, availablePrefabs[brushPrefabName].notificationType);
                            RenderMap();
                        }
                        else if(brushPrefabName == "")
                        {
                            currentMapData.mapLayers[pos.z].layerTiles[pos.x, pos.y].Paint("EmptyTile" + "l" + pos.z + "x" + pos.x + "y" + pos.y, 0, "", InteractionSystemDescription.None, TheEscapists.ActionsAndInteractions.NotifyType.None);
                            RenderMap();
                        }
                        else
                            Debug.LogError("Error: non valid Prefab - " + brushPrefabName);
                    }
                    if (currentTool == ToolType.FillPaint)
                    {
                        if(firstPositionSet)
                        {
                            Vector2Int pos1 = Vector2Int.zero;
                            Vector2Int pos2 = Vector2Int.zero;

                            if (firstFillPosition.x > tile.position.x)
                            {
                                pos2.x = firstFillPosition.x;
                                pos1.x = tile.position.x;
                            }
                            else
                            {
                                pos1.x = firstFillPosition.x;
                                pos2.x = tile.position.x;
                            }

                            if (firstFillPosition.y > tile.position.y)
                            {
                                pos2.y = firstFillPosition.y;
                                pos1.y = tile.position.y;
                            }
                            else
                            {
                                pos1.y = firstFillPosition.y;
                                pos2.y = tile.position.y;
                            }

                            if(firstFillPosition.z != tile.position.z)
                            {
                                ResetFill();
                                return;
                            }

                            for(int x = pos1.x; x <= pos2.x; x++)
                            {
                                for (int y = pos1.y; y <= pos2.y; y++)
                                {
                                    if (availablePrefabs.ContainsKey(brushPrefabName))
                                    {
                                        currentMapData.mapLayers[tile.position.z].layerTiles[x, y].Paint(availablePrefabs[brushPrefabName].prefabName + "l" + tile.position.z + "x" +x + "y" + y, avalableBrushRotations[rotationDropdown.value], brushPrefabName, availablePrefabs[brushPrefabName].interactionSystemDescription, availablePrefabs[brushPrefabName].notificationType);
                                    }
                                    else if (brushPrefabName == "")
                                    {
                                        currentMapData.mapLayers[tile.position.z].layerTiles[x, y].Paint("EmptyTile" + "l" + tile.position.z + "x" + x + "y" + y, 0, "", InteractionSystemDescription.None, TheEscapists.ActionsAndInteractions.NotifyType.None);
                                    }
                                    else
                                        Debug.LogError("Error: non valid Prefab - " + brushPrefabName);
                                }
                            }
                            RenderMap();

                            if (fillPositionMarkerTransform)
                            {
                                ResetFill();
                            }
                        }
                        else
                        {
                            cancelFillSetting.SetActive(true);
                            fillPositionMarkerTransform = Instantiate(fillPositionMarker, transform.position - new Vector3(Mathf.FloorToInt(currentMapData.mapSize.x / 2), Mathf.FloorToInt(currentMapData.mapSize.y / 2), -1) + new Vector3(tile.position.x, tile.position.y, -1), transform.rotation, transform).transform;
                            firstFillPosition = tile.position;
                            firstPositionSet = true;
                        }
                    }
                    if (currentTool == ToolType.WallPaint)
                    {
                        if (firstPositionSet)
                        {
                            Vector2Int pos1 = Vector2Int.zero;
                            Vector2Int pos2 = Vector2Int.zero;

                            if (firstFillPosition.x > tile.position.x)
                            {
                                pos2.x = firstFillPosition.x;
                                pos1.x = tile.position.x;
                            }
                            else
                            {
                                pos1.x = firstFillPosition.x;
                                pos2.x = tile.position.x;
                            }

                            if (firstFillPosition.y > tile.position.y)
                            {
                                pos2.y = firstFillPosition.y;
                                pos1.y = tile.position.y;
                            }
                            else
                            {
                                pos1.y = firstFillPosition.y;
                                pos2.y = tile.position.y;
                            }

                            if (firstFillPosition.z != tile.position.z)
                            {
                                ResetFill();
                                return;
                            }

                            for (int x = pos1.x; x <= pos2.x; x++)
                            {
                                for (int y = pos1.y; y <= pos2.y; y++)
                                {
                                    if (x == pos1.x || x == pos2.x || y == pos1.y || y == pos2.y)
                                    {
                                        if (availablePrefabs.ContainsKey(brushPrefabName))
                                        {
                                            currentMapData.mapLayers[tile.position.z].layerTiles[x, y].Paint(availablePrefabs[brushPrefabName].prefabName + "l" + tile.position.z + "x" + x + "y" + y, avalableBrushRotations[rotationDropdown.value], brushPrefabName, availablePrefabs[brushPrefabName].interactionSystemDescription, availablePrefabs[brushPrefabName].notificationType);
                                        }
                                        else if (brushPrefabName == "")
                                        {
                                            currentMapData.mapLayers[tile.position.z].layerTiles[x, y].Paint("EmptyTile" + "l" + tile.position.z + "x" + x + "y" + y, 0, "", InteractionSystemDescription.None, TheEscapists.ActionsAndInteractions.NotifyType.None);
                                        }
                                        else
                                            Debug.LogError("Error: non valid Prefab - " + brushPrefabName);
                                    }
                                }
                            }
                            RenderMap();

                            if (fillPositionMarkerTransform)
                            {
                                ResetFill();
                            }
                        }
                        else
                        {
                            cancelFillSetting.SetActive(true);
                            fillPositionMarkerTransform = Instantiate(fillPositionMarker, transform.position - new Vector3(Mathf.FloorToInt(currentMapData.mapSize.x / 2), Mathf.FloorToInt(currentMapData.mapSize.y / 2), -1) + new Vector3(tile.position.x, tile.position.y, -1), transform.rotation, transform).transform;
                            firstFillPosition = tile.position;
                            firstPositionSet = true;
                        }
                    }
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (rotationDropdown.value > 0)
            {
                rotationDropdown.value--;
            }
            else
            {
                rotationDropdown.value = rotationDropdown.options.Count - 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (rotationDropdown.value < rotationDropdown.options.Count - 1)
            {
                rotationDropdown.value++;
            }
            else
            {
                rotationDropdown.value = 0;
            }
        }
    }

    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void UpdateMapSize()
    {
        currentMapData.ResizeLayers(newMapSize);
        GenerateMap();
    }

    public void GenerateMap()
    {
        MapCreatorTile[] tiles = transform.GetComponentsInChildren<MapCreatorTile>();
        for (int i = tiles.Length - 1; i >= 0; i--)
        {
            Destroy(tiles[i].gameObject);
        }

        for (int layer = 0; layer < currentMapData.mapLayers.Count; layer++)
        {
            if (!currentMapData.mapLayers[layer].isHidden)
            {
                for (int x = 0; x < currentMapData.mapSize.x; x++)
                {
                    for (int y = 0; y < currentMapData.mapSize.y; y++)
                    {

                        MapCreatorTile tile;

                        if (layer == selectedLayer)
                            tile = Instantiate(selectableTilePrefab, transform.position - new Vector3(Mathf.FloorToInt(currentMapData.mapSize.x / 2), Mathf.FloorToInt(currentMapData.mapSize.y / 2), 0) + new Vector3(x, y, 0), transform.rotation, transform).GetComponent<MapCreatorTile>();
                        else
                            tile = Instantiate(tilePrefab, transform.position - new Vector3(Mathf.FloorToInt(currentMapData.mapSize.x / 2), Mathf.FloorToInt(currentMapData.mapSize.y / 2), 0) + new Vector3(x, y, 0.001f * layer), transform.rotation, transform).GetComponent<MapCreatorTile>();

                        tile.Init(new Vector3Int(x, y, layer));
                    }
                }
            }
        }

        if(!cameraController)
            cameraController = Camera.main.GetComponent<CameraController>();

        cameraController.SetMaxOrthographicSize(currentMapData.mapSize.x > currentMapData.mapSize.y ? currentMapData.mapSize.x : currentMapData.mapSize.y);
    }

    public void RenderMap()
    {
        MapCreatorTile[] tiles = transform.GetComponentsInChildren<MapCreatorTile>();

        foreach (MapCreatorTile tile in tiles)
        {
            tile.UpdateTile();
        }
    }

    public void GeneratePrefabList()
    {
        MapCreatorPrefabListElement[] brushColors = BrushListHolder.GetComponentsInChildren<MapCreatorPrefabListElement>();
        for (int i = brushColors.Length - 1; i >= 0; i--)
        {
            Destroy(brushColors[i].gameObject);
        }

        MapCreatorPrefabListElement element = Instantiate(brushListElement, BrushListHolder).GetComponent<MapCreatorPrefabListElement>();
        element.Init("", null, false);

        bool first = true;
        foreach (KeyValuePair<string, PrefabDescription> pair in availablePrefabs)
        {
            element = Instantiate(brushListElement, BrushListHolder).GetComponent<MapCreatorPrefabListElement>();
            element.Init(pair.Key, pair.Value.icon, first);
            first = false;
        }
    }

    public void GenerateLayerList()
    {
        MapCreatorLayerListElement[] elements = LayerListHolder.GetComponentsInChildren<MapCreatorLayerListElement>();
        for (int i = elements.Length - 1; i >= 0; i--)
        {
            Destroy(elements[i].gameObject);
        }

        bool first = true;
        for(int i = 0; i < currentMapData.mapLayers.Count; i++)
        {
            MapCreatorLayerListElement element = Instantiate(layerElementPrefab, LayerListHolder).GetComponent<MapCreatorLayerListElement>();
            element.Init(i, currentMapData.mapLayers[i].layerName, first);
            first = false;
        }
    }
}
#endif