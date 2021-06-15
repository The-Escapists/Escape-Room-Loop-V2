using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTypeSetter : MonoBehaviour
{
    [SerializeField]
    MapCreator.ToolType toolType;
    [SerializeField]
    Image selectionIndicator;
    public bool selected;

    private void Start()
    {
        if (toolType == MapCreator.ToolType.SinglePaint) Select();
        else Deselect();
    }

    public void Select()
    {
        foreach (ToolTypeSetter element in transform.parent.GetComponentsInChildren<ToolTypeSetter>())
        {
            if (element.selected)
                element.Deselect();
        }
        selected = true;
        MapCreator.instance.SetToolType(toolType);
        selectionIndicator.color = MapCreator.instance.selectedColor;
    }

    public void Deselect()
    {
        selected = false;
        selectionIndicator.color = MapCreator.instance.normalColor;
    }
}
