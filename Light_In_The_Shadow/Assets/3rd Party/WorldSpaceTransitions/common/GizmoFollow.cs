using System;
using UnityEngine;
using System.Collections;

namespace WorldSpaceTransitions
{
    public class GizmoFollow : MonoBehaviour
    {
        private static int m_referenceCount = 0;

        private static GizmoFollow m_instance;
        private Vector3 tempPos;
        private Quaternion tempRot;

        public bool followPosition = true;
        public bool followRotation = true;

        public static GizmoFollow Instance
        {
            get
            {
                return m_instance;
            }
        }

        void Awake()
        {
            m_referenceCount++;
            if (m_referenceCount > 1)
            {
                DestroyImmediate(this.gameObject);
                return;
            }

            m_instance = this;
            // Use this line if you need the object to persist across scenes
            //DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            transform.SetParent(MasterManager.Instance.transform);
        }

        void Update()
        {


            if (tempPos != transform.position || tempRot != transform.rotation)
            {

                tempPos = transform.position;
                tempRot = transform.rotation;
                SetSection();
            }
        }


        void OnEnable()
        {
            SetSection();
        }

        void OnDestroy()
        {
            m_referenceCount--;
            if (m_referenceCount == 0)
            {
                m_instance = null;
            }
        }

        void SetSection()
        {

            if (followPosition) Shader.SetGlobalVector("_SectionPoint", transform.position);
            if (followRotation)
            {
                Shader.SetGlobalVector("_SectionPlane", transform.forward);
                Shader.SetGlobalVector("_SectionPlane2", transform.right);
            }
        }


    }
}