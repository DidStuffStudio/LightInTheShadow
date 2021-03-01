using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace WorldSpaceTransitions.Examples
{
    //[ExecuteInEditMode]
    public class EnvironmentTransitionExample : MonoBehaviour
    {
        private GameObject[] players;
        public Collider belowPlane;
        public float forcemultiplier = 1.0f;
        //public float radius_speed = 0.5f;
        public float spread = 0.5f;
        public float spreadIncrement = 0.1f;
        public float tmax = 3.0f;
        public float rmax = 150.0f;
        public float timeUp = 0.4f;
        public LayerMask ignoreLayer;
        public AnimationCurve radiusCurve;
        public AnimationCurve moveUpCurve;
        //public float fwdInterval = 1.5f;
        private bool onwardCoroutineIsRunning = false;
        private bool backwardCoroutineIsRunning = false;
        private Plane plane;
        private float clickTime = 0f;
        public float doubleClickInterval = 0.2f;
        private float transitionTime = 0f;
        private float radius;

        /*
        //The software cursor to get recorded in unity recorder
        public Texture2D cursorTexture;
        public CursorMode cursorMode = CursorMode.ForceSoftware;
        public Vector2 hotSpot = Vector2.zero;
        void OnMouseEnter()
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }

        void OnMouseExit()
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }*/

        void Start()
        {
            Shader.DisableKeyword("FADE_SPHERE");
            Shader.DisableKeyword("FADE_PLANE");
            Shader.SetGlobalInt("_FADE_SPHERE", 0);
            Shader.SetGlobalInt("_FADE_PLANE", 0);
            if (FadingTransition.instance)
            {
                Shader.SetGlobalFloat("_ScreenNoiseScale", FadingTransition.instance.noiseScaleScreen);
                Shader.SetGlobalFloat("_Noise3dScale", FadingTransition.instance.noiseScaleWorld);
            }
            plane = new Plane(transform.up, transform.position);
        }

        void OnEnable()
        {
            Shader.DisableKeyword("FADE_SPHERE");
            Shader.DisableKeyword("FADE_PLANE");
            Shader.SetGlobalInt("_FADE_SPHERE", 0);
            Shader.SetGlobalInt("_FADE_PLANE", 0);
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        void OnDisable()
        {
            Shader.DisableKeyword("FADE_SPHERE");
            Shader.SetGlobalInt("_FADE_SPHERE", 0);
            StopAllCoroutines();
            onwardCoroutineIsRunning = false;
        }

        void Update()
        {
            //Shader.SetGlobalFloat("_Radius", 0.2f);
            //if(onwardCoroutineIsRunning) return;
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 10000f, ignoreLayer)) return;

                //Initialise the enter variable
                float enter = 0.0f;

                if (plane.Raycast(ray, out enter))
                //              if (Physics.Raycast(ray, out hit, 10000f))
                {
                    // if (hit.transform == transform)
                    // {
                    if (Time.time - clickTime < doubleClickInterval)
                    {
                        Debug.Log("hit");
                        Vector3 hitPoint = ray.GetPoint(enter);
                        Debug.Log(hitPoint.ToString() + " |1| " + transform.up.ToString());
                        Ray ray2 = new Ray(hitPoint, -transform.up);
                        RaycastHit hit2;
                        if (belowPlane.Raycast(ray2, out hit2, 10000f))
                        {
                            Debug.Log(hit2.point.ToString() + " |2| " + hit2.normal.ToString());
                            Debug.Log(hit2.distance.ToString());
                            Debug.DrawLine(hit.point, hit2.point, Color.red, 2.0f);
                            if (!onwardCoroutineIsRunning)
                            {
                                StartCoroutine(doOnwardTransition(hit2.point, hit2.distance));
                            }
                            else
                            {
                                StartCoroutine(doBackwardTransition(hit2.point, hit2.distance));
                            }
                        }
                    }
                    clickTime = Time.time;
                    //}
                    //else if(hit.transform == belowPlane.transform)
                    //{
                    //StopAllCoroutines();
                    //}
                }
            }
        }


        IEnumerator doOnwardTransition(Vector3 hitPoint, float _radius)
        {
            radius = _radius;
            onwardCoroutineIsRunning = true;
            backwardCoroutineIsRunning = false;
            float startTime = Time.time;
            transitionTime = 0f;
            foreach (GameObject p in players)
            {
                p.GetComponent<Rigidbody>().isKinematic = true;
            }
            //GetComponent<Collider>().enabled = false;
            Shader.EnableKeyword("FADE_SPHERE");
            Shader.SetGlobalInt("_FADE_SPHERE", 1);
            Shader.SetGlobalVector("_SectionPoint", hitPoint);
            while ((transitionTime < tmax) && onwardCoroutineIsRunning)
            {
                //float r = radius + transitionTime * (transitionTime* transitionTime + transitionTime + 0.6f) * radius_speed;
                float r = radius + radiusCurve.Evaluate(transitionTime/tmax)*rmax;
                Shader.SetGlobalFloat("_Radius", r);
                Shader.SetGlobalFloat("_spread", spread + r * spreadIncrement);

                transitionTime = Time.time - startTime;

                foreach (GameObject p in players)
                {
                    Rigidbody rb = p.GetComponent<Rigidbody>();
                    if (!rb.isKinematic) continue;
                    Vector3 force = forcemultiplier * Camera.main.transform.right;
                    Vector3 planeposition = p.transform.position;
                    planeposition.y = -1.005f;
                    if (Vector3.Distance(planeposition, hitPoint) < r)
                    {
                        rb.isKinematic = false;
                        rb.AddForce(force, ForceMode.Impulse);
                    }
                }

                yield return null;
            }
            //onwardCoroutineIsRunning = false;
        }

        IEnumerator doBackwardTransition(Vector3 hitPoint, float radius)
        {
            onwardCoroutineIsRunning = false;
            backwardCoroutineIsRunning = true;
            /*float startTime = Time.time;
            //float t = 0f;
            foreach (GameObject p in players)
            {
                p.GetComponent<Rigidbody>().isKinematic = true;
            }
            GetComponent<Collider>().enabled = false;
            Shader.EnableKeyword("CLIP_SPHERE");
            Shader.SetGlobalInt("_FADE_SPHERE", 1);*/
            Shader.SetGlobalVector("_SectionPoint", hitPoint);
            while ((transitionTime > 0) && backwardCoroutineIsRunning)
            {
                //float r = radius + transitionTime * (transitionTime * transitionTime + transitionTime + 1) * radius_speed;
                float r = radius + radiusCurve.Evaluate(transitionTime/tmax) * rmax;

                Shader.SetGlobalFloat("_Radius", r);
                Shader.SetGlobalFloat("_spread", spread + r * spreadIncrement);

                transitionTime -= Time.deltaTime;

                foreach (GameObject p in players)
                {
                    Rigidbody rb = p.GetComponent<Rigidbody>();
                    if (rb.isKinematic) continue;
                    //Vector3 force = forcemultiplier * Camera.main.transform.right;
                    Vector3 planeposition = p.transform.position;
                    planeposition.y = -1.005f;
                    if (Vector3.Distance(planeposition, hitPoint) > r)
                    {
                        rb.isKinematic = true;
                        //MoveUp(p);
                        StartCoroutine(_MoveUp(p));
                        //rb.AddForce(force, ForceMode.Impulse);
                    }
                }

                yield return null;
            }
            if (backwardCoroutineIsRunning)
            {
                Shader.DisableKeyword("FADE_SPHERE");
                Shader.SetGlobalInt("_FADE_SPHERE", 0);
            }
            //backwardCoroutineIsRunning = false;
        }

        void MoveUp(GameObject g)
        {
            Vector3 colliderMin = g.GetComponent<Collider>().bounds.min;
            g.transform.position += new Vector3(0, transform.position.y - colliderMin.y, 0);

        }
        IEnumerator _MoveUp(GameObject g)
        {
            Vector3 colliderMin = g.GetComponent<Collider>().bounds.min;
            float t = 0f;
            Vector3 startPos = g.transform.position;
            float y = transform.position.y - colliderMin.y;
            while ((t < timeUp)&& backwardCoroutineIsRunning)
            {
                g.transform.position = startPos + new Vector3(0, y*moveUpCurve.Evaluate(t/timeUp), 0);
                t += Time.deltaTime;
                yield return null;
            }
            g.transform.position = startPos + new Vector3(0, y, 0);
        }
    }
}
