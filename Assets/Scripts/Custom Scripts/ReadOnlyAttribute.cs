using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;  // <- “олько дл€ редактора
#endif

// 1. јтрибут (может быть в любом месте проекта)
public class ReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR
// 2. PropertyDrawer (только дл€ редактора)
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif
