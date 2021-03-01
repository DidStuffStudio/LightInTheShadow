using UnityEngine;
using UnityEditor;
using System.Collections;

namespace WorldSpaceTransitions
{
    [CustomPropertyDrawer(typeof(FadingTransition.GradientOption))]
    class GradientOptionDrawer : PropertyDrawer
    {
        int rows = 1;
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property. 
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Rect boolRect = new Rect(position.x, position.y, position.width, 20);
            Rect gradRect = new Rect(position.x, position.y + 20, position.width, 16);
            Rect btnRect = new Rect(position.x + position.width - 150, position.y + 40, 150, 20);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            SerializedProperty m_useGradient = property.FindPropertyRelative("useGradient");
            SerializedProperty m_gradientChanged = property.FindPropertyRelative("gradientChanged");
            SerializedProperty m_texturePath = property.FindPropertyRelative("texturePath");
            SerializedProperty m_filename = property.FindPropertyRelative("filename");

            EditorGUI.PropertyField(boolRect, m_useGradient, new GUIContent("useGradient"));//EditorGUILayout.PropertyField(m_VectorProp, new GUIContent("Vector Object"));
            rows = m_useGradient.boolValue ? 3 : 1;
            if (m_useGradient.boolValue)
            {
                GUI.changed = false;
                EditorGUI.PropertyField(gradRect, property.FindPropertyRelative("transitionGradient"));
                FadingTransition ft = property.serializedObject.targetObject as FadingTransition;
                if (GUI.changed)
                {
                    Debug.Log("changed"); m_gradientChanged.boolValue = true;
                    //ft.UpdateGradientTexture();
                }
                GUI.enabled = m_gradientChanged.boolValue;
                if (GUI.Button(btnRect, "Save gradient as texture:"))
                {
                    string path = EditorUtility.SaveFilePanel("Save Gradient Texture", Application.dataPath + "/" + m_texturePath.stringValue, m_filename.stringValue + ".png", "png");
                    if (path.Length > 0) ft.SaveTexture(path);
                }
                rows = 4;
                GUI.enabled = true;
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * rows;  // assuming original is one row
        }

    }
}