using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheEscapists.ActionsAndInteractions;
using TheEscapists.ActionsAndInteractions.Actions;
using TheEscapists.Core;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

//[CreateAssetMenu]
public class MapInteractionNodeGraph : NodeGraph
{
    public MapDescription mapDescription;

    [Button, HideIf("@NodeEditorWindow.current == null")]
    public void Reset()
    {
        List<LogicNode> logicNodes = new List<LogicNode>();

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] is LogicNode) logicNodes.Add(nodes[i] as LogicNode);
        }

        foreach (LogicNode logicNode in logicNodes) logicNode.Reset();
    }

    [Button, HideIf("@NodeEditorWindow.current == null")]
    public void RunDebug()
    {
        List<OutputNode> outputs = new List<OutputNode>();

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] is InputNode) (nodes[i] as InputNode).Update();

            if (nodes[i] is OutputNode) outputs.Add(nodes[i] as OutputNode);
        }

        foreach (OutputNode output in outputs)
        {
            Debug.Log(output.GetValue(null));
        }
    }
	public Dictionary<string, bool> Run()
    {

        Dictionary<string, bool> actorsStates = new Dictionary<string, bool>();

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] is InputNode)
                (nodes[i] as InputNode).Update();

            if (nodes[i] is OutputNode)
                actorsStates.Add((nodes[i] as OutputNode).name, (nodes[i] as OutputNode).GetValue());
        }

        return actorsStates;
    }

    [Button, HideIf("@NodeEditorWindow.current == null")]
    public void Notify(string name, NotifyType notifyType)
    {
        List<InputNode> inputs = new List<InputNode>();

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] is InputNode && nodes[i].name == name) inputs.Add(nodes[i] as InputNode);
        }

        foreach (InputNode input in inputs) input.Notify(notifyType);
    }


    [Button, HideIf("@mapDescription == null || NodeEditorWindow.current == null")]
    public void InitNodes()
    {
        if (mapDescription == null)
            return;

        //mapDescription.interactionGraph = this;

        NodeEditorWindow window = NodeEditorWindow.current;

        for(int i = nodes.Count-1; i >= 0; i--)
        {
            window.graphEditor.RemoveNode(nodes[i]);
            i--;
        }

        AddNodes();
    }

    [Button, HideIf("@mapDescription == null || NodeEditorWindow.current == null")]
    public void AddNodes()
    {
        NodeEditorWindow window = NodeEditorWindow.current;

        int createdInputNodes = 0;
        int createdOutputNodes = 0;

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] is InputNode) createdInputNodes++;
            if (nodes[i] is OutputNode) createdOutputNodes++;
        }

        for (int i = 0; i < mapDescription.mapSize.x * mapDescription.mapSize.y * mapDescription.layerCount; i++)
        {
            bool skip = false;
            foreach(Node node in nodes)
            {
                if(node.name == mapDescription.tileName[i])
                {
                    skip = true;
                    break;
                }
            }
            if (skip)
                continue;
            InteractionSystemDescription interactionSystemDescription = (InteractionSystemDescription)mapDescription.interactionSystemDescriptions[i];
            if (interactionSystemDescription == InteractionSystemDescription.Interactor)
            {
                InputNode inputNode = window.graphEditor.CreateNode(typeof(InputNode), new Vector2(-400, 0 + 40 * createdInputNodes)) as InputNode;
                inputNode.allowedNotifyType = (NotifyType)mapDescription.notifyTypes[i];
                inputNode.name = mapDescription.tileName[i];
                createdInputNodes++;
            }

            if (interactionSystemDescription == InteractionSystemDescription.Actor || interactionSystemDescription == InteractionSystemDescription.Interactor)
            {
                OutputNode outputNode = window.graphEditor.CreateNode(typeof(OutputNode), new Vector2(400, 0 + 40 * createdOutputNodes)) as OutputNode;
                outputNode.name = mapDescription.tileName[i];
                createdOutputNodes++;
            }
        }
    }
}