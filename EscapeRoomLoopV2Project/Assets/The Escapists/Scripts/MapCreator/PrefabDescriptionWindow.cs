using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabDescriptionWindow : OdinMenuEditorWindow
{
    [MenuItem("The Escapists/Prefab Descriptions")]
    private static void OpenWindow()
    {
        GetWindow<PrefabDescriptionWindow>().Show();
    }

    private CreateNewPrefabDescription createNewPrefabDescription;

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (createNewPrefabDescription != null)
            DestroyImmediate(createNewPrefabDescription.prefabDescription);
    }

    protected override OdinMenuTree BuildMenuTree()
    {

        OdinMenuTree tree = new OdinMenuTree();
        createNewPrefabDescription = new CreateNewPrefabDescription();
        tree.Add("Create New", createNewPrefabDescription);
        tree.AddAllAssetsAtPath("Prefab Descriptions", "Assets/The Escapists/Resources/Prefab Descriptions", typeof(PrefabDescription), true, true);

        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        //gets reference to the currently selected item
        OdinMenuTreeSelection selected = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();

            if(SirenixEditorGUI.ToolbarButton("Delete Current"))
            {
                PrefabDescription asset = selected.SelectedValue as PrefabDescription;

                if (asset)
                {
                    string path = AssetDatabase.GetAssetPath(asset);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                }
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();

        //base.OnBeginDrawEditors();
    }
}

public class CreateNewPrefabDescription
{
    public CreateNewPrefabDescription()
    {
        prefabDescription = ScriptableObject.CreateInstance<PrefabDescription>();
        prefabDescription.prefabName = "new Prefab Description";
    }

    [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
    public PrefabDescription prefabDescription;

    [Button("Create New Prefab Description")]
    private void CreateNewDesription()
    {
        AssetDatabase.CreateAsset(prefabDescription, "Assets/The Escapists/Resources/Prefab Descriptions/" + prefabDescription.prefabName + ".asset");
        AssetDatabase.SaveAssets();
        
        prefabDescription = ScriptableObject.CreateInstance<PrefabDescription>();
        prefabDescription.prefabName = "new Prefab Description";

    }
}
