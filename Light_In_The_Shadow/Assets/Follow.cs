using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform transformToFollow;

    void Update()
    {
        var transform1 = transform;
        transform1.position = transformToFollow.position;
        transform1.rotation = transformToFollow.rotation;
    }
}
