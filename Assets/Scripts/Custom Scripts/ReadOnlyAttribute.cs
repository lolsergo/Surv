using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;  // <- ������ ��� ���������
#endif

// 1. ������� (����� ���� � ����� ����� �������)
public class ReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR
// 2. PropertyDrawer (������ ��� ���������)
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
