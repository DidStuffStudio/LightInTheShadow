using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameConstantStrings {

    public struct Tags {
        // Add the tag strings here
        public static string Ground { get; } = "Ground";
        public static string LoadingScreenTrigger { get; } = "LoadingScreenTrigger";
        public static string DialNumber { get; } = "DialNumber";
        public static string Tile { get; } = "Tile";
        public static string Enemy { get; } = "Enemy";
        public static string LevelPortal { get; } = "LevelPortal";
        public static string LoadingScreen { get; } = "LoadingScreen";
        public static string PickedUp { get; } = "PickedUp";
        public static string ClickInteract { get; } = "ClickInteract";
    }

    public struct Layers {
        // Add the layers strings here
        public static int Ground => GetLayer("Ground");
        public static int FocusPoint => GetLayer("FocusPoint");
    }
    
    public struct Scenes {
        // Add the scene strings here
        public static string LoadingScreen => GetScene("LoadingScreen");
        public static string Level1Main => GetScene("Scenes/Level_1_Main");
        public static string Level2Main => GetScene("Scenes/Level_2_Main");
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