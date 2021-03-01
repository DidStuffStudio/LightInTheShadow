using UnityEngine;
using UnityEditor;

namespace WorldSpaceTransitions
{
//#if UNITY_EDITOR
    [CustomEditor(typeof(TransitionGradient))]
    public class TransitionGradientEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            //Rect gradrect = GUILayoutUtility.GetLastRect();
  //          GUILayout.Label("Save gradient as texture:");
            TransitionGradient gradGenerator = (TransitionGradient)target;
            if (gradGenerator.textureChanged)
            {
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Save gradient as texture:",  GUILayout.ExpandWidth(true)))
                {
                    string path = EditorUtility.SaveFilePanel("Save Gradient Texture", Application.dataPath + "/" + gradGenerator.texturePath, gradGenerator.filename + ".png", "png");
                    if (path.Length > 0) gradGenerator.SaveTexture(path);
                }
                //GUILayout.Space(10);

                GUILayout.EndHorizontal();
            }
        }
    }
//#endif
}