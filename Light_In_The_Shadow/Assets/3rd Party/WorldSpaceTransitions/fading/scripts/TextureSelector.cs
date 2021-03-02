//Script to store the dissolving noise textures and set a selected one as global shader texture variables at play time.
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;


namespace WorldSpaceTransitions
{
    [RequireComponent(typeof(Dropdown))]
    public class TextureSelector : MonoBehaviour
    {
        [System.Serializable]
        public struct TextureSet
        {
            public Texture texture;
            public Sprite sprite;
        }

        public List<TextureSet> sets;
        private Dropdown dropdown;
        [FormerlySerializedAs("keyword")]
        public string globalVarName;
        public int selected=0;

        void Start()
        {
            dropdown = GetComponent<Dropdown>();
            dropdown.ClearOptions();
            SetGlobalTexture(selected);

                List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

            foreach (TextureSet ts in sets)
            {
                Dropdown.OptionData optionData = new Dropdown.OptionData();
                optionData.image = ts.sprite;
                options.Add(optionData);
            }
            dropdown.options = options;

            dropdown.onValueChanged.AddListener(SetGlobalTexture);

        }

        void SetGlobalTexture(int i)
        {
            Shader.SetGlobalTexture(globalVarName, sets[i].texture);
            Debug.Log(sets[i].texture.name);
            if (globalVarName == "_NoiseAtlas")
            {
                int atlasSize = Mathf.RoundToInt(Mathf.Pow(sets[i].texture.width, 1f / 3));
                //Debug.Log(atlasSize.ToString());
                Shader.SetGlobalFloat("_atlasSize", atlasSize);
            }
        }
    }
}
