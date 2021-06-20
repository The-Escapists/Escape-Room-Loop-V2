using System;
using UnityEngine;
using UnityEngine.UI;

public class MapCreatorTile : MonoBehaviour
{
    [SerializeField]
    Image icon;

    public Vector3Int position;
    public void Init(Vector3Int position)
    {
        this.position = position;
        string prefabName = MapCreator.instance.currentMapData.mapLayers[position.z].layerTiles[position.x, position.y].brushPrefabName;
        int rotation = MapCreator.instance.currentMapData.mapLayers[position.z].layerTiles[position.x, position.y].tileRotation;
        if (prefabName != "")
        {
            //Instantiate(MapCreator.instance.availablePrefabs[prefabName].prefab, transform.position, Quaternion.Euler(0,0,rotation),transform);
            icon.sprite = MapCreator.instance.availablePrefabs[prefabName].icon;
            icon.GetComponentInParent<Canvas>().sortingOrder = position.z;
            icon.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotation);
        }
    }

    public void UpdateTile()
    {
       // if(transform.childCount > 0)
       //Destroy(transform.GetChild(0).gameObject);

        string prefabName = MapCreator.instance.currentMapData.mapLayers[position.z].layerTiles[position.x, position.y].brushPrefabName;
        int rotation = MapCreator.instance.currentMapData.mapLayers[position.z].layerTiles[position.x, position.y].tileRotation;
        if (prefabName != "")
        {
            //Instantiate(MapCreator.instance.availablePrefabs[prefabName].prefab, transform.position, Quaternion.Euler(0,0,rotation),transform);
            icon.sprite = MapCreator.instance.availablePrefabs[prefabName].icon;
            icon.GetComponentInParent<Canvas>().sortingOrder = position.z;
            icon.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotation);
        }
    }
}