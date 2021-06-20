using Sirenix.OdinInspector;
using TheEscapists.ActionsAndInteractions;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PrefabDescription : ScriptableObject
{
    public string prefabName;
    public Color frameColor;
    public GameObject prefab;
    public Sprite icon;
    public InteractionSystemDescription interactionSystemDescription;
    public NotifyType notificationType;
#if UNITY_EDITOR
    [InlineEditor]
    public PrefabChecker prefabChecker;

    private void OnEnable()
    {
        if (prefabChecker)
        {
            if (!prefabChecker.hasDescription)
                prefabChecker.Init(this);

            prefabChecker.Check();
        }
        else if (interactionSystemDescription == InteractionSystemDescription.Actor || interactionSystemDescription == InteractionSystemDescription.Interactor)
        {
            prefabChecker = CreateInstance<PrefabChecker>();
            prefabChecker.Init(this);
            if (prefabChecker) AssetDatabase.AddObjectToAsset(prefabChecker, "Assets/The Escapists/Resources/Prefab Descriptions/" + prefabName + ".asset");
            AssetDatabase.SaveAssets();
        }
    }

    private void OnValidate()
    {
        if (prefabChecker)
        {
            if (!prefabChecker.hasDescription)
                prefabChecker.Init(this);

            prefabChecker.Check();
        }
        else if (interactionSystemDescription == InteractionSystemDescription.Actor || interactionSystemDescription == InteractionSystemDescription.Interactor)
        {
            prefabChecker = CreateInstance<PrefabChecker>();
            prefabChecker.Init(this);
            if (prefabChecker) AssetDatabase.AddObjectToAsset(prefabChecker, "Assets/The Escapists/Resources/Prefab Descriptions/" + prefabName + ".asset");
            AssetDatabase.SaveAssets();
        }
    }
#endif
}

public enum InteractionSystemDescription { None, Actor, Interactor};