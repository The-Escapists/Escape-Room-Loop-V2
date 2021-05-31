using Bolt;
using Ludiq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using TheEscapists.Core;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "The Escapists/Room Creator/Room Description")]
public class RoomDescriptionSO : SerializedScriptableObject
{
    [OnValueChanged("UpdateGroupSize")]
    public Vector2Int roomSize;
    [TableMatrix(SquareCells = true, DrawElementMethod = "DrawCell"), OnValueChanged("UpdateInteractionIndexArray")]
    public int[,] roomMatrix;
    private int brushMaxRange;
    [HorizontalGroup("brush"), Button("▼")]
    public void left() { if (brush - 1 >= 0) brush--; GetPrefabCollectionSize(); UpdateInteractionIndexArray(); }
    [HorizontalGroup("brush"), ReadOnly, PropertyRange(0, "$brushMaxRange")]
    public int brush;
    [HorizontalGroup("brush"), Button("▲")]
    public void right() { if (brush + 1 <= brushMaxRange) brush++; GetPrefabCollectionSize(); UpdateInteractionIndexArray(); }
    [HorizontalGroup("prefabCollection"), OnValueChanged("GetPrefabCollectionSize"), InlineEditor]
    public RoomPrefabCollectionSO prefabCollection;
    [VerticalGroup("prefabCollection/Button"), Button("New")]
    public void createNewPrefabCollection()
    {
        RoomPrefabCollectionSO asset = ScriptableObject.CreateInstance<RoomPrefabCollectionSO>();
        asset.roomPrefabCollection.Add(new RoomPrefabDescription("Empty", null, Color.white, InteractiveType.None));
        asset.roomPrefabCollection.Add(new RoomPrefabDescription("Example", null, Color.black, InteractiveType.None));

        AssetDatabase.CreateAsset(asset, "Assets/The Escapists/Room Prefab Collection/MyRoomPrefabCollection.asset");
        AssetDatabase.SaveAssets();

        //EditorUtility.FocusProjectWindow();

        //Selection.activeObject = asset;

        prefabCollection = asset;
        GetPrefabCollectionSize();
    }
    [VerticalGroup("prefabCollection/Button"), Button("Update")]
    private void GetPrefabCollectionSize()
    {
        if (prefabCollection)
            brushMaxRange = prefabCollection.roomPrefabCollection.Count - 1;
        else
            brushMaxRange = 0;

        while (brush > brushMaxRange) { brush--; }
        while (brush < 0) { brush++; }
        UpdateInteractionIndexArray();
    }

    public List<Actor> actorList;
    public List<Interactor> interactorList;
    [HorizontalGroup("flowMacro"), OnValueChanged("UpdatePointer")]
    public Bolt.FlowMacro flowMacro;
    public void UpdatePointer() { if (flowMacro) { graphPointer = flowMacro.GetReference(); } else { graphPointer = null; } }
    private GraphPointer graphPointer;
    [HorizontalGroup("flowMacro"), Button("New")]
    public void NewMacro()
    {
        FlowMacro asset = FlowMacro.CreateInstance<FlowMacro>();
        AssetDatabase.CreateAsset(asset, "Assets/The Escapists/Macros/" + name + "_macro.asset");
        AssetDatabase.SaveAssets();
        flowMacro = asset;
    }

    [ShowIf("$flowMacro"), Button("Set Actors and Interactors in FlowMacro")]
    public void setActorsAndInteractors()
    {
        UpdateInteractionIndexArray();

        if(graphPointer == null) UpdatePointer();
        foreach (Interactor actor in interactorList)
        {
            Bolt.Variables.Graph(graphPointer).Set(actor.name, actor.state);
        }

        foreach (Actor actor in actorList)
        {
            Bolt.Variables.Graph(graphPointer).Set(actor.name, actor.state);
        }
    }

    private void UpdateGroupSize()
    {
        roomMatrix = new int[roomSize.x, roomSize.y];
        for (int i = 0; i < roomSize.x; i++)
        {
            for (int j = 0; j < roomSize.y; j++)
            {
                roomMatrix[i, j] = 0;
            }
        }
    }

    public int DrawCell(Rect rect, int value)
    {
        if (Event.current.type == EventType.MouseDown
        && rect.Contains(Event.current.mousePosition))
        {
            value = brush;
            GUI.changed = true;
            Event.current.Use();
        }

        Color currentBrushColor = Color.white;
        if (brushMaxRange > 0 && prefabCollection)
        {
            RoomPrefabDescription roomPrefabDescription = prefabCollection.roomPrefabCollection[value];
            currentBrushColor = roomPrefabDescription.roomDescriptionColor;
        }

        EditorGUI.DrawRect(
            rect.Padding(1),
            currentBrushColor
        );

        return value;
    }

    public void UpdateInteractionIndexArray()
    {

        for (int i = 0; i < roomSize.x; i++)
        {
            for (int j = 0; j < roomSize.y; j++)
            {
                InteractiveType type = prefabCollection.roomPrefabCollection[roomMatrix[i, j]].interactiveType;
                string name = type.ToString().Substring(0, 1) + prefabCollection.roomPrefabCollection[roomMatrix[i, j]].name + i + j;
                Vector2 pointer = new Vector2(i, j);
                if (type == InteractiveType.None)
                {
                    for (int z = 0; z < actorList.Count; z++)
                    {
                        if (actorList[z].position == pointer)
                        {
                            actorList.RemoveAt(z);
                        }
                    }
                    for (int z = 0; z < interactorList.Count; z++)
                    {
                        if (interactorList[z].position == pointer)
                        {
                            interactorList.RemoveAt(z);
                        }
                    }
                }
                else if (type == InteractiveType.Actor)
                {
                    bool existsAlready = false;
                    for (int z = 0; z < interactorList.Count; z++)
                    {
                        if (interactorList[z].position == pointer)
                        {
                            interactorList.RemoveAt(z);
                        }
                    }

                    for (int z = 0; z < actorList.Count; z++)
                    {
                        if (actorList[z].position == pointer)
                        {
                            if (actorList[z].id != roomMatrix[i, j])
                            {
                                actorList.RemoveAt(z);
                                actorList.Add(new Actor(name, false, pointer, roomMatrix[i, j]));
                                existsAlready = true;
                            }
                            else if (actorList[z].id == roomMatrix[i, j])
                            {
                                existsAlready = true;
                            }
                        }
                    }

                    if (!existsAlready)
                    {
                        actorList.Add(new Actor(name, false, pointer, roomMatrix[i, j]));
                    }
                }
                else if (type == InteractiveType.Interactor)
                {
                    bool existsAlready = false;
                    for (int z = 0; z < actorList.Count; z++)
                    {
                        if (actorList[z].position == pointer)
                        {
                            actorList.RemoveAt(z);
                        }
                    }

                    for (int z = 0; z < interactorList.Count; z++)
                    {
                        if (interactorList[z].position == pointer)
                        {
                            if (interactorList[z].id != roomMatrix[i, j])
                            {
                                interactorList.RemoveAt(z);
                                interactorList.Add(new Interactor(name, false, pointer, roomMatrix[i, j]));
                                existsAlready = true;
                            }
                            else if (interactorList[z].id == roomMatrix[i, j])
                            {
                                existsAlready = true;
                            }
                        }
                    }

                    if (!existsAlready)
                    {
                        interactorList.Add(new Interactor(name, false, pointer, roomMatrix[i, j]));
                    }
                }
                else if (type == InteractiveType.Both)
                {
                    bool existsAlready1 = false;
                    bool existsAlready2 = false;

                    for (int z = 0; z < actorList.Count; z++)
                    {
                        if (actorList[z].position == pointer)
                        {
                            if (actorList[z].id != roomMatrix[i, j])
                            {
                                actorList.RemoveAt(z);
                                actorList.Add(new Actor(name, false, pointer, roomMatrix[i, j]));
                                existsAlready1 = true;
                            }
                            else if (actorList[z].id == roomMatrix[i, j])
                            {
                                existsAlready1 = true;
                            }
                        }
                    }

                    for (int z = 0; z < interactorList.Count; z++)
                    {

                        if (interactorList[z].position == pointer)
                        {
                            if (interactorList[z].id != roomMatrix[i, j])
                            {
                                interactorList.RemoveAt(z);
                                interactorList.Add(new Interactor(name, false, pointer, roomMatrix[i, j]));
                                existsAlready2 = true;
                            }
                            else if (interactorList[z].id == roomMatrix[i, j])
                            {
                                existsAlready2 = true;
                            }
                        }
                    }

                    if (!existsAlready1)
                    {
                        actorList.Add(new Actor(name, false, pointer, roomMatrix[i, j]));
                    }
                    if (!existsAlready2)
                    {

                        interactorList.Add(new Interactor(name, false, pointer, roomMatrix[i, j]));
                    }
                }
            }
        }
    }
}