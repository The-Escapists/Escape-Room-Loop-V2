using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCreatorPrefabListElement : MonoBehaviour
{
    string prefabName;
    public bool selected;
    public Image icon;
    public Image indicator;

    public void Init(string prefabName, Sprite IconSprite, bool selected)
    {
        this.prefabName = prefabName;
        if(IconSprite)
        icon.sprite = IconSprite;

        if(selected)
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
        foreach(MapCreatorPrefabListElement element in transform.parent.GetComponentsInChildren<MapCreatorPrefabListElement>())
        {
            if(element.selected)
            element.Deselect();
        }
        selected = true;
        MapCreator.instance.brushPrefabName = prefabName;
        indicator.color = MapCreator.instance.selectedColor;
    }

    public void Deselect()
    {
        selected = false;
        indicator.color = MapCreator.instance.normalColor;
    }

}
