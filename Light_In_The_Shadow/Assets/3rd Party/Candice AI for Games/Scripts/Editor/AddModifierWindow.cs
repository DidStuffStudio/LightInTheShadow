using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ViridaxGameStudios.AI
{
    public class AddModifierWindow : EditorWindow
    {
        public CharacterStat stat;
        float value;
        int order;
        StatModType type;
        private void OnGUI()
        {
            GUIContent label = new GUIContent("Value");
            if (stat != null)
            {
                GUILayout.Space(2);
                GUILayout.BeginVertical("box");
                value = EditorGUILayout.FloatField(label, value);
                type = (StatModType)EditorGUILayout.EnumPopup("Type", type);
                order = EditorGUILayout.IntField("Order", order);
                GUILayout.EndVertical();
                GUILayout.Space(2);
                if (GUILayout.Button("Add"))
                {
                    if (type == 0)
                    {
                        EditorUtility.DisplayDialog("Add Modifier", "Please make sure to select a Modifier Type.", "Okay");
                        return;
                    }


                    StatModifier mod = new StatModifier(value, type, order);
                    stat.AddModifier(mod);
                    Close();
                }
            }
            if (GUILayout.Button("Close"))
            {
                Close();
            }
        }
        public void Draw()
        {

        }
    }

}
