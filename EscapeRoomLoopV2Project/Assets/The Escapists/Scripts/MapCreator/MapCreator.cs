using Sirenix.OdinInspector;
using System.Collections.Generic;
using TheEscapists.ActionsAndInteractions;
using TheEscapists.ActionsAndInteractions.Actions;
using TheEscapists.ActionsAndInteractions.Interaction;
using TheEscapists.Core;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    [Required("Action And Interaction Manager Prefab is necessary to access the full functionality")]
    public GameObject actionAndInteractionManager;
    [Required("Room Description is necessary to access the full functionality")]
    public RoomDescriptionSO roomDescription;
    GameObject sceneActionAndInteractionManager;

    bool toggleManager { get { return (actionAndInteractionManager || sceneActionAndInteractionManager) && roomDescription; } }

    [ShowIf("toggleManager"), Button("Spawn Prefabs")]
    private void SpawnPrefabs()
    {
        DestroyAllPrefabs();

        if (!sceneActionAndInteractionManager) {

            ActionAndInteractionManager aaim = GameObject.FindObjectOfType<ActionAndInteractionManager>();
            if(aaim)
            sceneActionAndInteractionManager = aaim.gameObject;
        }
        if (sceneActionAndInteractionManager)
        {
            ActionAndInteractionManager manager = sceneActionAndInteractionManager.GetComponent<ActionAndInteractionManager>();
            manager.roomDescription = roomDescription;
            manager.UpdateMacro();
        }
        else
        {
            GameObject go = Instantiate(actionAndInteractionManager, transform);
            go.transform.parent = null;
            ActionAndInteractionManager manager = go.GetComponent<ActionAndInteractionManager>();
            manager.roomDescription = roomDescription;
            manager.UpdateMacro();
            sceneActionAndInteractionManager = go;
        }
        Transform pointer = new GameObject("Pointer").transform;
        int xLength = roomDescription.roomMatrix.GetLength(0);
        int yLength = roomDescription.roomMatrix.GetLength(1);

        for (int i = 0; i < xLength; i++)
        {
            for (int j = 0; j < yLength; j++)
            {
                pointer.transform.position = transform.position + new Vector3(i, j, 0);
                Vector2 pos = new Vector2(i, j);
                int type = roomDescription.roomMatrix[i, j];
                RoomPrefabDescription roomPrefabDescription = roomDescription.prefabCollection.roomPrefabCollection[type];
                if (roomPrefabDescription.prefab)
                {
                    GameObject newGameObj = Instantiate(roomPrefabDescription.prefab, pointer);
                    newGameObj.transform.parent = transform;

                    if (roomPrefabDescription.interactiveType == InteractiveType.Interactor)
                    {
                        Interaction[] interactions = newGameObj.GetComponentsInChildren<Interaction>();
                        foreach (Interaction interaction in interactions)
                        {
                            for (int z = 0; z < roomDescription.interactorList.Count; z++)
                            {
                                Interactor interactor = roomDescription.interactorList[z];
                                if (interactor.position == pos)
                                {
                                    interaction.interactionIndex = z;
                                }
                            }
                        }
                    }

                    if (roomPrefabDescription.interactiveType == InteractiveType.Actor)
                    {
                        Action[] actions = newGameObj.GetComponentsInChildren<Action>();
                        foreach (Action action in actions)
                        {
                            for (int z = 0; z < roomDescription.actorList.Count; z++)
                            {
                                Actor actor = roomDescription.actorList[z];
                                if (actor.position == pos)
                                {
                                    action.actionIndex = z;
                                }
                            }
                        }
                    }

                    if (roomPrefabDescription.interactiveType == InteractiveType.Both)
                    {
                        Interaction[] interactions = newGameObj.GetComponentsInChildren<Interaction>();
                        Action[] actions = newGameObj.GetComponentsInChildren<Action>();
                        foreach (Interaction interaction in interactions)
                        {
                            for (int z = 0; z < roomDescription.interactorList.Count; z++)
                            {
                                Interactor interactor = roomDescription.interactorList[z];
                                if (interactor.position == pos)
                                {
                                    interaction.interactionIndex = z;
                                }
                            }
                        }
                        foreach (Action action in actions)
                        {
                            for (int z = 0; z < roomDescription.actorList.Count; z++)
                            {
                                Actor actor = roomDescription.actorList[z];
                                if (actor.position == pos)
                                {
                                    action.actionIndex = z;
                                }
                            }
                        }
                    }
                }
            }
        }

        DestroyImmediate(pointer.gameObject);
    }

    private void DestroyAllPrefabs()
    {
        GameObject[] objects = new GameObject[transform.childCount];
        for(int i = 0; i < transform.childCount; i++) {
            objects[i] = transform.GetChild(i).gameObject;
        }
        foreach(GameObject obj in objects)
        {
            DestroyImmediate(obj);
        }
    }
}
