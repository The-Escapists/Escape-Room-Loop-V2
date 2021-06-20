using Sirenix.OdinInspector;
using System.IO;
using TheEscapists.ActionsAndInteractions;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class MapDescription : SerializedScriptableObject
{ 
#if UNITY_EDITOR
    [Button]
    public void CreateInteractionGraph(MapDescription mapDescription)
    {
        if (interactionGraph) return;

        if (!Directory.Exists(Application.dataPath + "/The Escapists/Resources/Interaction Graphs/"))
            Directory.CreateDirectory(Application.dataPath + "/The Escapists/Resources/Interaction Graphs/");
        interactionGraph = CreateInstance<MapInteractionNodeGraph>();
        interactionGraph.name = mapName + "Graph";
        interactionGraph.mapDescription = mapDescription;
        AssetDatabase.CreateAsset(interactionGraph, "Assets/The Escapists/Resources/Interaction Graphs/" + mapName + "Graph.asset");
        AssetDatabase.SaveAssets();
    }
#endif
    
    public string mapName;
    public Vector2Int mapSize;
    public int layerCount;
    public MapInteractionNodeGraph interactionGraph;
    
    public int[] layerIndex;
    public string[] layerName;
    public bool[] isHidden;

    public string[] tileName;
    public int[] tileRotation;
    public string[] brushPrefabName;
    public int[] interactionSystemDescriptions;
    public int[] notifyTypes;

    public void Init(MapData map)
    {
        mapName = map.mapName;
        mapSize = map.mapSize;
        layerCount = map.mapLayers.Count;

        layerIndex = new int[layerCount];
        layerName = new string[layerCount];
        isHidden = new bool[layerCount];
        tileName = new string[mapSize.x * mapSize.y * layerCount];
        tileRotation = new int[mapSize.x * mapSize.y * layerCount];
        brushPrefabName = new string[mapSize.x * mapSize.y * layerCount];
        interactionSystemDescriptions = new int[mapSize.x * mapSize.y * layerCount];
        notifyTypes = new int[mapSize.x * mapSize.y * layerCount];

        for (int z = 0; z < layerCount; z++)
        {
            layerIndex[z] = map.mapLayers[z].layerIndex;
            layerName[z] = map.mapLayers[z].layerName;
            isHidden[z] = map.mapLayers[z].isHidden;

            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    tileName[(z * mapSize.x * mapSize.y) + (y * mapSize.x) + x] = map.mapLayers[z].layerTiles[x, y].tileName;
                    tileRotation[(z * mapSize.x * mapSize.y) + (y * mapSize.x) + x] = map.mapLayers[z].layerTiles[x, y].tileRotation;
                    brushPrefabName[(z * mapSize.x * mapSize.y) + (y * mapSize.x) + x] = map.mapLayers[z].layerTiles[x, y].brushPrefabName;
                    interactionSystemDescriptions[(z * mapSize.x * mapSize.y) + (y * mapSize.x) + x] = (int)map.mapLayers[z].layerTiles[x, y].interactionSystemDescription;
                    notifyTypes[(z * mapSize.x * mapSize.y) + (y * mapSize.x) + x] = (int)map.mapLayers[z].layerTiles[x, y].notifyType;
                }
            }
        }
    }
}