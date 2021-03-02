//setting the global shader variables in inspector in editor
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif
namespace WorldSpaceTransitions
{
    [ExecuteInEditMode]
    public class FadingTransition : MonoBehaviour
    {
        public enum SurfaceType
        { plane, sphere, none };
        public enum FadingCentre
        { gizmo, camera };
        public SurfaceType surfaceType = SurfaceType.plane;
        public FadingCentre fadingCentre = FadingCentre.gizmo;

        [Range(0.00001f, 2f)]
        public float transitionSpread = 1f;

        [Space(10)]
        public Texture2D ScreenNoiseTexture;
        /*[Space(10)]
        public Texture2D NoiseAtlasTexture;*/

        [Space(10)]
        public Texture3D noiseTexture3D;
        
        [Space(10)]
        public Texture2D noiseTriplanar2D;

        [System.Serializable]
        public class GradientOption
        {
            public bool useGradient = false;
            public Gradient transitionGradient;
            public bool gradientChanged = false;
            public string texturePath = "WorldSpaceTransitions/fading/textures/";
            public string filename = "_gradient";
        }
        public bool useTriplanarNoise = false;
        public Texture2D transitionTexture;
        public GradientOption gradientOption;
        //[SerializeField]
        //[HideInInspector]
        private Texture2D gradientTexture;
        private Vector3 gizmoPos;
        private Quaternion gizmoRot;
        private Transform gizmo;
        public float radius = 3f;
        public float distance = 0;//this is a transition plane distance from camera when in "tied to camera" mode.
        [Range(0.025f, 2f)]
        public float noiseScaleWorld = 1;
        [Range(0.025f, 5f)]
        public float noiseScaleScreen = 5;
        private bool dontValidate = false;

        public static FadingTransition instance;

        private void Awake()
        {
            dontValidate = true;
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        void Start()
        {
            UpdateGradientTexture();
            if (gradientOption.useGradient)
            {
                Shader.SetGlobalTexture("_TransitionGradient", gradientTexture);
            }
#if UNITY_EDITOR
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
#endif
            dontValidate = true;
            Shader.SetGlobalFloat("_spread", transitionSpread);
            if (ScreenNoiseTexture != null) Shader.SetGlobalTexture("_ScreenNoise", ScreenNoiseTexture);
            /*if (NoiseAtlasTexture != null)
            {
                Shader.SetGlobalTexture("_NoiseAtlas", NoiseAtlasTexture);
                int atlasSize = Mathf.RoundToInt(Mathf.Pow(NoiseAtlasTexture.width,1f/3));
                //Debug.Log(atlasSize.ToString());
                Shader.SetGlobalFloat("_atlasSize", atlasSize);
            }*/
            
            if (noiseTexture3D != null) Shader.SetGlobalTexture("_Noise3D", noiseTexture3D);
            
            if (noiseTriplanar2D != null) Shader.SetGlobalTexture("_Noise2D", noiseTriplanar2D);

            if (FindObjectOfType<GizmoFollow>())
            {
                gizmo = FindObjectOfType<GizmoFollow>().transform;
                if (gizmo)
                {
                    gizmoPos = gizmo.position;
                    gizmoRot = gizmo.rotation;
                }
            }

            Plane sPlane = new Plane(gizmoRot * Vector3.forward, gizmoPos);
            Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (sPlane.Raycast(cameraRay, out distance))
            {
                //Debug.DrawLine(Camera.main.transform.position, gizmo.position, Color.red, 10f);
            }

            //useDynamicGradientTexture = useDynamicGradientTexture && (FindObjectOfType<TransitionGradient>());//TransitionGradient.instance

            if (!gradientOption.useGradient) Shader.SetGlobalTexture("_TransitionGradient", gradientTexture);
            if (useTriplanarNoise)
            {
                Shader.EnableKeyword("NOISETRIPLANAR");
                Shader.SetGlobalInt("_NOISETRIPLANAR", 1);
            }
            else
            {
                Shader.DisableKeyword("NOISETRIPLANAR");
                Shader.SetGlobalInt("_NOISETRIPLANAR", 0);
            }

            dontValidate = false;
#if UNITY_EDITOR
            OnValidate();
#endif

        }

        void OnEnable()
        {
            if (surfaceType == SurfaceType.plane)
            {
                Shader.EnableKeyword("FADE_PLANE");
                Shader.EnableKeyword("CLIP_PLANE");
                Shader.SetGlobalInt("_FADE_PLANE", 1);
            }
            else if (surfaceType == SurfaceType.sphere)
            {
                Shader.EnableKeyword("FADE_SPHERE");
                Shader.EnableKeyword("CLIP_SPHERE");
                Shader.SetGlobalInt("_FADE_SPHERE", 1);
            }
            else
            {
                Shader.DisableKeyword("FADE_PLANE");
                Shader.DisableKeyword("CLIP_PLANE");
                Shader.DisableKeyword("FADE_SPHERE");
                Shader.DisableKeyword("CLIP_SPHERE");
                Shader.SetGlobalInt("_FADE_PLANE", 0);
                Shader.SetGlobalInt("_FADE_SPHERE", 0);
            }
        }

        void OnDisable()
        {
            Shader.DisableKeyword("FADE_PLANE");
            Shader.DisableKeyword("CLIP_PLANE");
            Shader.DisableKeyword("FADE_SPHERE");
            Shader.DisableKeyword("CLIP_SPHERE");
            Shader.SetGlobalInt("_FADE_PLANE", 0);
            Shader.SetGlobalInt("_FADE_SPHERE", 0);
        }

        void OnApplicationQuit()
        {

        }

#if UNITY_EDITOR
        void OnValidate()
        {
            //Debug.Log("FTvalidate");
            if (ScreenNoiseTexture != null) Shader.SetGlobalTexture("_ScreenNoise", ScreenNoiseTexture);
            /*if (NoiseAtlasTexture != null)
            {
                Shader.SetGlobalTexture("_NoiseAtlas", NoiseAtlasTexture);
                int atlasSize = Mathf.RoundToInt(Mathf.Pow(NoiseAtlasTexture.width, 1f/3));
                //Debug.Log(atlasSize.ToString());
                Shader.SetGlobalFloat("_atlasSize", atlasSize);
            }*/
            
            if (noiseTexture3D != null) Shader.SetGlobalTexture("_Noise3D", noiseTexture3D);
            
            if (noiseTriplanar2D != null) Shader.SetGlobalTexture("_Noise2D", noiseTriplanar2D);
            if (useTriplanarNoise)
            {
                Shader.EnableKeyword("NOISETRIPLANAR");
                Shader.SetGlobalInt("_NOISETRIPLANAR", 1);
            }
            else
            {
                Shader.DisableKeyword("NOISETRIPLANAR");
                Shader.SetGlobalInt("_NOISETRIPLANAR", 0);
            }

            if (dontValidate || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
            Shader.SetGlobalFloat("_spread", transitionSpread);

           // Debug.Log()
           // if(instance) gradientOption.useGradient = gradientOption.useGradient && TransitionGradient.instance;

            if (gradientOption.useGradient) UpdateGradientTexture();
            Shader.SetGlobalTexture("_TransitionGradient", (gradientOption.useGradient ? gradientTexture : transitionTexture));

            if (surfaceType == SurfaceType.plane)
            {
                Shader.DisableKeyword("FADE_SPHERE");
                Shader.DisableKeyword("CLIP_SPHERE");
                Shader.EnableKeyword("FADE_PLANE");
                Shader.EnableKeyword("CLIP_PLANE");
                Shader.SetGlobalInt("_FADE_PLANE", 1);
                Shader.SetGlobalInt("_FADE_SPHERE", 0);
            }
            else if (surfaceType == SurfaceType.sphere)
            {
                Shader.DisableKeyword("FADE_PLANE");
                Shader.DisableKeyword("CLIP_PLANE");
                Shader.EnableKeyword("FADE_SPHERE");
                Shader.EnableKeyword("CLIP_SPHERE");
                Shader.SetGlobalInt("_FADE_PLANE", 0);
                Shader.SetGlobalInt("_FADE_SPHERE", 1);
            }
            else
            {
                Shader.DisableKeyword("FADE_PLANE");
                Shader.DisableKeyword("CLIP_PLANE");
                Shader.DisableKeyword("FADE_SPHERE");
                Shader.DisableKeyword("CLIP_SPHERE");
                Shader.SetGlobalInt("_FADE_PLANE", 0);
                Shader.SetGlobalInt("_FADE_SPHERE", 0);
            }
            if (fadingCentre == FadingCentre.gizmo)
            {
                Shader.SetGlobalVector("_SectionPoint", gizmoPos);
                Shader.SetGlobalVector("_SectionPlane", gizmoRot * Vector3.forward);
                Shader.SetGlobalVector("_SectionPlane2", gizmoRot * Vector3.right);
            };

            if (fadingCentre == FadingCentre.camera)
            {
                Vector3 fpos = Camera.main.transform.position + Camera.main.transform.forward * distance;

                Shader.SetGlobalVector("_SectionPoint", fpos);
                Shader.SetGlobalVector("_SectionPlane", -Camera.main.transform.forward);
                Shader.SetGlobalVector("_SectionPlane2", Camera.main.transform.right);
            };
            Shader.SetGlobalFloat("_ScreenNoiseScale", noiseScaleScreen);
            Shader.SetGlobalFloat("_Noise3dScale", noiseScaleWorld);
            Shader.SetGlobalFloat("_Radius", radius);
        }
#endif
        public void SaveTexture(string _path)
        {

#if UNITY_EDITOR
            byte[] bytes = gradientTexture.EncodeToPNG();
            File.WriteAllBytes(_path, bytes);
            string filename = Path.GetFileNameWithoutExtension(_path);
            if (_path.Contains(Application.dataPath)) //reimport if saved within the Assets folder
            {
                AssetDatabase.Refresh();
                string assetPath = _path.Replace(Application.dataPath, "Assets");
                TextureImporter A = (TextureImporter)AssetImporter.GetAtPath(assetPath);
                string texturePath = Path.GetDirectoryName(assetPath);
                A.textureCompression = TextureImporterCompression.Uncompressed;

                A.filterMode = FilterMode.Point;
                A.wrapMode = TextureWrapMode.Clamp;
                A.mipmapEnabled = false;
                A.SaveAndReimport();
                filename = Path.GetFileNameWithoutExtension(_path);
            }
#endif
            gradientOption.gradientChanged = false;

        }
        public void UpdateGradientTexture()
        {
            //processing = true;
            //Texture2D oldTexture = gradientTexture;
            if (gradientTexture == null)
            {

                //bool colorSpaceIsLinear = PlayerSettings.colorSpace == ColorSpace.Linear;
                gradientTexture = new Texture2D(256, 2, TextureFormat.RGB24, false, false);
                gradientTexture.wrapMode = TextureWrapMode.Clamp;
                //textureChanged = false;
            }
            for (int i = 0; i < 256; i++)
            {
                float x = i * 1.0f / 255;
                Color col = new Color();
                float aCh;

                if (gradientOption.transitionGradient != null) col = gradientOption.transitionGradient.Evaluate(x);
                aCh = col.a;
                //col.a = 1;

                gradientTexture.SetPixel(i, 0, col);
                gradientTexture.SetPixel(i, 1, new Color(aCh, aCh, aCh));
                //if (gradientOption.gradientChanged || oldTexture == null) continue;
                //if (oldTexture.GetPixel(i, 0) != col) gradientOption.gradientChanged = true;
                //if (oldTexture.GetPixel(i, 0) != new Color(aCh, aCh, aCh)) gradientOption.gradientChanged = true;

            }

            gradientTexture.Apply();
            //processing = false;
            //SaveTexture();
        }
    }
}
