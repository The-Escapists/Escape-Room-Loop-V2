#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(MapInteractionNodeGraph))]
public class InteractionGraphEditor : NodeGraphEditor
{

    public override void AddContextMenuItems(GenericMenu menu, Type compatibleType = null, NodePort.IO direction = NodePort.IO.Input)
    {
        menu.AddItem(new GUIContent("MapDescription/Init Map Nodes"), false, () => { (window.graph as MapInteractionNodeGraph).InitNodes(); });
        menu.AddItem(new GUIContent("MapDescription/Update Map Nodes"), false, () => { (window.graph as MapInteractionNodeGraph).AddNodes(); });
        menu.AddSeparator("");
        base.AddContextMenuItems(menu, compatibleType, direction);
        menu.AddItem(new GUIContent("Select Graph"), (Selection.activeObject == window.graph), () => { Selection.SetActiveObjectWithContext(window.graph, null); });
    }

}
#endif