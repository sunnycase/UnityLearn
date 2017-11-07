using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor
{
    private SerializedProperty _itemImagesProperty;
    private SerializedProperty _itemsProperty;
    private bool[] _showItemSlots = new bool[Inventory.NumItemSlots];

    private void OnEnable()
    {
        _itemImagesProperty = serializedObject.FindProperty("itemImages");
        _itemsProperty = serializedObject.FindProperty("items");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < _showItemSlots.Length; i++)
            ItemSlotGUI(i);

        serializedObject.ApplyModifiedProperties();
    }

    private void ItemSlotGUI(int index)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        _showItemSlots[index] = EditorGUILayout.Foldout(_showItemSlots[index], "Item slot " + index);
        if(_showItemSlots[index])
        {
            EditorGUILayout.PropertyField(_itemImagesProperty.GetArrayElementAtIndex(index));
            EditorGUILayout.PropertyField(_itemsProperty.GetArrayElementAtIndex(index));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
