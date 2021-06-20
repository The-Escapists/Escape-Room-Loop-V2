using Sirenix.OdinInspector;
using System.IO;
using TheEscapists.ActionsAndInteractions;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class MapDescription : ScriptableObject
{
#if UNITY_EDITOR
    [Button]
    public void CreateInteractionGraph()
    {
        if (interactionGraph) return;

        if (!Directory.Exists(Application.dataPath + "/The Escapists/Resources/Interaction Graphs/"))
            Directory.CreateDirectory(Application.dataPath + "/The Escapists/Resources/Interaction Graphs/");
        interactionGraph = CreateInstance<MapInteractionNodeGraph>();
        interactionGraph.name = mapName + "Graph";
        AssetDatabase.CreateAsset(interactionGraph, "Assets/The Escapists/Resources/Interaction Graphs/" + mapName + "Graph.asset");
        AssetDatabase.SaveAssets();

        interactionGraph.mapDescription = this;
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
    public InteractionSystemDescription[] interactionSystemDescriptions;
    public NotifyType[] notifyTypes;

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
        interactionSystemDescriptions = new InteractionSystemDescription[mapSize.x * mapSize.y * layerCount];
        notifyTypes = new NotifyType[mapSize.x * mapSize.y * layerCount];

        for (int i = 0; i < layerCount; i++)
        {
            layerIndex[i] = map.mapLayers[i].layerIndex;
            layerName[i] = map.mapLayers[i].layerName;
            isHidden[i] = map.mapLayers[i].isHidden;

            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    tileName[x * mapSize.y + y * layerCount + i] = map.mapLayers[i].layerTiles[x, y].tileName;
                    tileRotation[x * mapSize.y + y * layerCount + i] = map.mapLayers[i].layerTiles[x, y].tileRotation;
                    brushPrefabName[x * mapSize.y + y * layerCount + i] = map.mapLayers[i].layerTiles[x, y].brushPrefabName;
                    interactionSystemDescriptions[x * mapSize.y + y * layerCount + i] = map.mapLayers[i].layerTiles[x, y].interactionSystemDescription;
                    notifyTypes[x * mapSize.y + y * layerCount + i] = map.mapLayers[i].layerTiles[x, y].notifyType;
                }
            }
        }
    }
}