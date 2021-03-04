using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {
    [SerializeField] private Portal[] portals;

    private void OnPreCull() {
        // foreach (var portal in portals) portal.Render();
    }
}