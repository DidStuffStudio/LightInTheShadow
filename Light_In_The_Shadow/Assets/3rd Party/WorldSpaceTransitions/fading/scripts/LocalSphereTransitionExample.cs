using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.EventSystems;

namespace WorldSpaceTransitions
{
    [ExecuteInEditMode]
    public class LocalSphereTransitionExample : MonoBehaviour
    {
        private Material[] allMats;
        public float radius_max = 3;
        public float radius_editor = 1;
        public float fwdInterval = 1.5f;
        public float bwdInterval = 0.6f;
        private bool coroutineIsRunning = false;

        void Start()
        {
            Shader.DisableKeyword("FADE_PLANE");
            Shader.DisableKeyword("FADE_SPHERE");
        }

        void OnEnable()
        {
            Shader.DisableKeyword("FADE_PLANE");
            Shader.DisableKeyword("FADE_SPHERE");

            allMats = new Material[0];
            Renderer[] allrenderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in allrenderers)
            {
                Material[] mats = r.sharedMaterials;
                allMats = allMats.Union(mats).ToArray();
                Debug.Log(allMats.Length.ToString());
            }

            foreach (Material m in allMats)
            {
                m.DisableKeyword("FADE_PLANE");
                m.SetInt("_FADE_PLANE", 0);
                string kwd = "";
                foreach (string s in m.shaderKeywords) kwd += s + " ";
                Debug.Log(kwd);
            }

            if (Application.isPlaying)
            {
                foreach (Material m in allMats)
                {
                    m.DisableKeyword("FADE_SPHERE");
                    m.SetInt("_FADE_SPHERE", 0);
                }
            }
            else
            {
                Ray ray = new Ray(Camera.main.transform.position, transform.position - Camera.main.transform.position);
                RaycastHit hit;
                Collider collider = GetComponent<Collider>();
                if (collider.Raycast(ray, out hit, 1000f))
                {
                    Debug.Log(hit.transform.name);
                }

                foreach (Material m in allMats)
                {
                    m.SetVector("_SectionPoint", hit.point);
                    m.SetFloat("_Radius", radius_editor);
                    m.EnableKeyword("FADE_SPHERE");
                    m.SetInt("_FADE_SPHERE", 1);
                }
            }
        }

        void OnDisable()
        {
            foreach (Material m in allMats)
            {
                m.DisableKeyword("FADE_SPHERE");
                m.SetInt("_FADE_SPHERE", 0);
            }
            StopAllCoroutines();
            coroutineIsRunning = false;
        }

        void Update()
        {
            //Shader.SetGlobalFloat("_Radius", 0.2f);
            if(coroutineIsRunning) return;
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 10000f))
                {
                    if (hit.transform==transform)
                    {
                        Debug.Log("hit");

                        StartCoroutine(doTransition(hit.point));
                    }
                }
            }
        }



        IEnumerator doTransition(Vector3 hitPoint)
        {
            coroutineIsRunning = true;
            float startTime = Time.time;
            float t = 0f;
            foreach (Material m in allMats)
            {
                m.SetVector("_SectionPoint", hitPoint);
                m.SetFloat("_Radius", 0);
                m.EnableKeyword("FADE_SPHERE");
                m.SetInt("_FADE_SPHERE", 1);
            }

            while (t<1)
            {
                float radius = Mathf.SmoothStep(0, radius_max, t);
                foreach (Material m in allMats)
                {
                    m.SetFloat("_Radius", radius);
                }
                t = (Time.time - startTime)/ fwdInterval;
                yield return null;
            }
            t = 0f;
            startTime = Time.time;
            while (t < 1)
            {
                float radius = Mathf.SmoothStep(radius_max, 0, t);
                foreach (Material m in allMats)
                {
                    m.SetFloat("_Radius", radius);
                }
                t = (Time.time - startTime) / bwdInterval;
                yield return null;
            }
            foreach (Material m in allMats)
            {
                m.DisableKeyword("FADE_SPHERE");
                m.SetInt("_FADE_SPHERE", 0);
                Debug.Log("dis");
            }
            coroutineIsRunning = false;

        }
    }
}
