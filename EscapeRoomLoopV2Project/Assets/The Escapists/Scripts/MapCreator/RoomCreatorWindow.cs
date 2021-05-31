using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

[EditorWindowTitle(title = "Map Creator")]
public class RoomCreatorWindow : OdinEditorWindow
{

    //[MenuItem("The Escapists/Map Creator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(RoomCreatorWindow)).Show();
    }

    [HorizontalGroup("TableSize"), LabelWidth(200), OnValueChanged("CreateData")]
    public int mapHeight;
    [HorizontalGroup("TableSize"), LabelWidth(200), OnValueChanged("CreateData")]
    public int mapWidth;

    [TableMatrix(SquareCells = true, DrawElementMethod = "DrawCell")]
    public int[,] typeMatrix;

    public int brush;

    [OnInspectorInit]
    private void CreateData()
    {
        typeMatrix = new int[mapWidth, mapHeight];
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                typeMatrix[i, j] = 0;
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

        Color CurrentBrushColor = Color.white;
        EditorGUI.DrawRect(
            rect.Padding(1),
            CurrentBrushColor
        );

        return value;
    }
}
