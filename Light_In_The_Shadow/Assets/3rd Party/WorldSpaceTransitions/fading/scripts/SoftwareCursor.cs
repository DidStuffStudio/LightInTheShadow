using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace WorldSpaceTransitions
{
    //The software cursor to get recorded in unity recorder
    public class SoftwareCursor : MonoBehaviour
    {
        public float doubleClickInterval = 0.3f;
        //The software cursor to get recorded in unity recorder
        [System.Serializable]
        public class CustomCursor
        {
            public Texture2D cursorTexture;
            public CursorMode cursorMode = CursorMode.ForceSoftware;
            public Vector2 hotSpot = Vector2.zero;
        }
        public CustomCursor normalCursor;
        public CustomCursor singleClickCursor;
        public CustomCursor[] cursorAnimation;

        private float clickTime = 0f;
        private bool animationIsRunning = false;

        //public TextMesh txt;


        void OnEnable()
        {
            //txt.text = "";
            Cursor.SetCursor(normalCursor.cursorTexture, normalCursor.hotSpot, normalCursor.cursorMode);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                if (animationIsRunning) return;
                StartCoroutine(CursorAnimation());
            }
        }

        IEnumerator CursorAnimation()
        {
            animationIsRunning = true;
            //txt.text = "SINGLE CLICK";
            bool doubleClick = false;
            clickTime = Time.time;
            Cursor.SetCursor(singleClickCursor.cursorTexture, singleClickCursor.hotSpot, singleClickCursor.cursorMode);
            yield return new WaitForFixedUpdate();
            while ((Time.time - clickTime < doubleClickInterval)&&!doubleClick)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    doubleClick = true;
                    //txt.text = "DOUBLE CLICK";
                }
                yield return null;
            }
            int i = 0;
            int n = cursorAnimation.Length;
            while ((i<n)&&doubleClick)
            {
                Cursor.SetCursor(cursorAnimation[i].cursorTexture, cursorAnimation[i].hotSpot, cursorAnimation[i].cursorMode);
                i++;
                yield return null;
            }
            animationIsRunning = false;
            //txt.text = "";
            Cursor.SetCursor(normalCursor.cursorTexture, normalCursor.hotSpot, normalCursor.cursorMode);
            yield return null;
        }
    }
}