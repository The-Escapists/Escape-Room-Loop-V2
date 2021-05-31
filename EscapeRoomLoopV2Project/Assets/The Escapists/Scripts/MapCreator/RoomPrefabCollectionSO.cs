using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "The Escapists/Room Creator/Room Prefab Collection")]
public class RoomPrefabCollectionSO : SerializedScriptableObject
{
    [TableList(ShowIndexLabels = true), OnValueChanged("CheckFirstValue")]
    public List<RoomPrefabDescription> roomPrefabCollection = new List<RoomPrefabDescription>();
    public void CheckFirstValue()
    {
        if (!roomPrefabCollection[0].roomDescriptionColor.Equals(Color.white))
        {
            roomPrefabCollection.Insert(0, new RoomPrefabDescription("Empty", null, Color.white, InteractiveType.None));
        }
    }
}

public enum InteractiveType { None, Interactor, Actor, Both };

public struct RoomPrefabDescription
{
    public string name;
    [AssetsOnly]
    public GameObject prefab;
    [Tooltip("Color used for the Room Description Tool"), OnValueChanged("ResetAlpha")]
    public Color roomDescriptionColor;

    public InteractiveType interactiveType;

    private void ResetAlpha()
    {
        roomDescriptionColor.a = 1;
    }

    public RoomPrefabDescription(string _name, GameObject _prefab, Color _roomDescriptionColor, InteractiveType _interactiveType)
    {
        name = _name;
        prefab = _prefab;
        roomDescriptionColor = _roomDescriptionColor;
        interactiveType = _interactiveType;
    }
}