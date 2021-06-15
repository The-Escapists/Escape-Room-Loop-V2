using System;
using UnityEngine;

public class MapCreatorTile : MonoBehaviour
{
    public Vector3Int position;
    public void Init(Vector3Int position)
    {
        this.position = position;
        string prefabName = MapCreator.instance.currentMapData.mapLayers[position.z].layerTiles[position.x, position.y].brushPrefabName;
        int rotation = MapCreator.instance.currentMapData.mapLayers[position.z].layerTiles[position.x, position.y].tileRotation;
        if (prefabName != "")
            Instantiate(MapCreator.instance.availablePrefabs[prefabName].prefab, transform.position, Quaternion.Euler(0,0,rotation),transform);
    }

    public void UpdateTile()
    {
        if(transform.childCount > 0)
       Destroy(transform.GetChild(0).gameObject);

        string prefabName = MapCreator.instance.currentMapData.mapLayers[position.z].layerTiles[position.x, position.y].brushPrefabName;
        int rotation = MapCreator.instance.currentMapData.mapLayers[position.z].layerTiles[position.x, position.y].tileRotation;
        if (prefabName != "")
            Instantiate(MapCreator.instance.availablePrefabs[prefabName].prefab, transform.position, Quaternion.Euler(0, 0, rotation), transform);
    }
}