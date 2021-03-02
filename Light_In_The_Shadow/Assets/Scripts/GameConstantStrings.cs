using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameConstantStrings {

    public struct Tags {
        // Add the tag strings here
        public static string Ground { get; } = "Ground";
    }

    public struct Layers {
        // Add the layers strings here
        public static int Ground => GetLayer("Ground");
    }
    
    public struct Scenes {
        // Add the scene strings here
        public static string LoadingScreen => GetScene("LoadingScreen");
    }

    private static int GetLayer(string name) {
        var layer = LayerMask.NameToLayer(name);
        if (layer != -1) 
            return layer;
        ShowError(typeof(Layers), name);
        return -1;
    }

    private static string GetScene(string name) {
        var s = SceneManager.GetSceneByName(name).name;
        if (s != null) return s;
        ShowError(typeof(Scenes), name);
        return null;
    }

    private static void ShowError(System.Type s, string name) {
        Debug.LogError(name + " of type " + s +  " does not exist. Please make sure that it has been added");
    }

    
}