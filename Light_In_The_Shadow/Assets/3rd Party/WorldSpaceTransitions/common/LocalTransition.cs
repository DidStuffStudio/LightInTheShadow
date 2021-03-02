using UnityEngine;
using System.Collections;

namespace WorldSpaceTransitions
{
    public class LocalTransition : MonoBehaviour
    {
        private Vector3 tempPos;
        private Quaternion tempRot;
        private Material mat;

        public bool followPosition = true;
        public bool followRotation = true;

        void Start()
        {
            Renderer rend = GetComponent<Renderer>();
            mat = new Material(rend.material);
            rend.material = mat;
            SetSection();
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


        void SetSection()
        {

            if (followPosition) mat.SetVector("_SectionPoint", transform.position);
            if (followRotation)
            {
                mat.SetVector("_SectionPlane", transform.forward);
                mat.SetVector("_SectionPlane2", transform.right);
            }
        }

    }
}