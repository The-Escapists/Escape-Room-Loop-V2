using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TheEscapists.ActionsAndInteractions;
using UnityEngine;

[System.Serializable]
public class MapTileData
{
    public string tileName;
    public int tileRotation;
    public string brushPrefabName;
    public InteractionSystemDescription interactionSystemDescription;
    public NotifyType notifyType;
    public MapTileData(string tileName, int tileRotation, string brushPrefabName, InteractionSystemDescription interactionSystemDescription, NotifyType notifyType)
    {
        this.tileName = tileName;
        this.tileRotation = tileRotation;
        this.brushPrefabName = brushPrefabName;
        this.interactionSystemDescription = interactionSystemDescription;
    }

    public void Paint(string tileName, int tileRotation, string brushPrefabName, InteractionSystemDescription interactionSystemDescription, NotifyType notifyType)
    {
        this.tileName = tileName;
        this.tileRotation = tileRotation;
        this.brushPrefabName = brushPrefabName;
        this.interactionSystemDescription = interactionSystemDescription;
    }

    public void Remove()
    {
        Paint("EmptyTile", 0, "", InteractionSystemDescription.None, NotifyType.None);
    }
}

[System.Serializable]
public class MapLayerData
{
    public int layerIndex;
    public string layerName;
    public bool isHidden;
    public MapTileData[,] layerTiles;

    public MapLayerData(int layerIndex, string layerName, bool isHidden, MapTileData[,] layerTiles)
    {
        this.layerIndex = layerIndex;
        this.layerName = layerName;
        this.isHidden = isHidden;
        this.layerTiles = layerTiles;
    }

    public void SetTileData(MapTileData[,] layerTiles)
    {
        this.layerTiles = layerTiles;
    }
}

public class MapData
{
    public List<MapLayerData> mapLayers;
    public string mapName;
    public Vector2Int mapSize;

    public MapData(Vector2Int mapSize)
    {

        this.mapSize = mapSize;
        mapName = "DefaultMapName";
        mapLayers = new List<MapLayerData>();
        mapLayers.Add(new MapLayerData(0, "Base Layer", false, new MapTileData[mapSize.x, mapSize.y]));
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                mapLayers[0].layerTiles[x, y] = new MapTileData("EmptyTile" + "l" + (mapLayers.Count-1) + "x" + x + "y" + y, 0, "", InteractionSystemDescription.None, NotifyType.None);
            }
        }
    }

    public MapData(MapDescription mapDescription)
    {
        mapSize = mapDescription.mapSize;
        mapName = mapDescription.mapName;
        mapLayers = new List<MapLayerData>();

        for (int z = 0; z < mapDescription.layerCount; z++)
        {
            MapTileData[,] data = new MapTileData[mapSize.x, mapSize.y];
            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    data[x, y] = new MapTileData(
                        mapDescription.tileName[(z * mapSize.x * mapSize.y) + (y * mapSize.x) + x],
                        mapDescription.tileRotation[(z * mapSize.x * mapSize.y) + (y * mapSize.x) + x],
                        mapDescription.brushPrefabName[(z * mapSize.x * mapSize.y) + (y * mapSize.x) + x],
                        (InteractionSystemDescription)mapDescription.interactionSystemDescriptions[(z * mapSize.x * mapSize.y) + (y * mapSize.x) + x],
                        (NotifyType)mapDescription.notifyTypes[(z * mapSize.x * mapSize.y) + (y * mapSize.x) + x]
                        );
                }
            }

            mapLayers.Add(new MapLayerData(mapDescription.layerIndex[z], mapDescription.layerName[z], mapDescription.isHidden[z], data));
        }
    }

    public void AddLayer(string layerName, bool isHidden)
    {
        mapLayers.Add(new MapLayerData(mapLayers.Count, layerName, false, new MapTileData[mapSize.x, mapSize.y]));
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                mapLayers[mapLayers.Count - 1].layerTiles[x, y] = new MapTileData("EmptyTile" + "l" + (mapLayers.Count - 1) + "x" + x + "y" + y, 0, "", InteractionSystemDescription.None, NotifyType.None);
            }
        }

        UpdateLayerIndex();
    }

    public void ResizeLayers(Vector2Int mapSize)
    {
        for (int z = 0; z < mapLayers.Count; z++)
        {
            MapTileData[,] data = new MapTileData[mapSize.x, mapSize.y];
            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {

                    if (this.mapSize.x > x && this.mapSize.y > y)
                        data[x, y] = mapLayers[z].layerTiles[x, y];
                    else
                        data[x, y] = new MapTileData("EmptyTile" + "l" + z + "x"+ x + "y" + y, 0, "", InteractionSystemDescription.None, NotifyType.None);
                }
            }
            mapLayers[z].SetTileData(data);
        }

        this.mapSize = mapSize;
    }

    public void RemoveLayer(int layerIndex)
    {
        if (mapLayers.Count > 1)
        {
            mapLayers.RemoveAt(layerIndex);
            UpdateLayerIndex();
        }
    }

    public void MoveLayer(int layerIndex, bool direction)
    {
        if (mapLayers.Count > 1)
        {
            if ((layerIndex == mapLayers.Count - 1 && direction == false) || (layerIndex == 0 && direction == true))
                return;

            MapLayerData data = mapLayers[layerIndex];
            if (direction)
            {
                mapLayers.RemoveAt(layerIndex);
                mapLayers.Insert(layerIndex - 1, data);
            }
            else
            {
                mapLayers.RemoveAt(layerIndex);
                mapLayers.Insert(layerIndex + 1, data);
            }

            UpdateLayerIndex();
        }
    }

    public void HideLayer(int layerIndex)
    {
        if (mapLayers.Count > 1)
            mapLayers[layerIndex].isHidden = !mapLayers[layerIndex].isHidden;
    }

    public void UpdateLayerIndex()
    {
        for (int i = 0; i < mapLayers.Count; i++)
        {
            mapLayers[i].layerIndex = i;
        }
    }
}