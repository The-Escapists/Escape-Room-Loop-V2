using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapCreatorLayerListElement : MonoBehaviour
{
    public int layerIndex;
    public bool selected;
    public Image indicator;
    public TMP_Text layerName;

    public void Init(int layerIndex, string layerName, bool selected)
    {
        this.layerIndex = layerIndex;

        if (this.layerName)
            this.layerName.text = layerName;

        if (selected)
        {
            Select();
        }
        else
        {
            Deselect();
        }
    }

    public void Select()
    {
        foreach (MapCreatorLayerListElement element in transform.parent.GetComponentsInChildren<MapCreatorLayerListElement>())
        {
            if (element.selected)
                element.Deselect();
        }
        selected = true;
        MapCreator.instance.SelectLayer(layerIndex);
        indicator.color = MapCreator.instance.selectedColor;
    }

    public void Deselect()
    {
        selected = false;
        indicator.color = MapCreator.instance.normalColor;
    }

    public void MoveLayerUp()
    {
        MapCreator.instance.MoveLayer(layerIndex, true);
    }

    public void MoveLayerDown()
    {
        MapCreator.instance.MoveLayer(layerIndex, false);
    }

    public void RemoveLayer()
    {
        MapCreator.instance.RemoveLayer(layerIndex);
    }

    public void HideLayer()
    {
        MapCreator.instance.HideLayer(layerIndex);
    }
}
