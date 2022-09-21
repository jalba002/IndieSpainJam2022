using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : EditorWindow
{
    readonly struct SceneData
    {
        public readonly EditorBuildSettingsScene settingsScene;
        public readonly Scene scene;
        public readonly string path;
        public readonly string name;
        public readonly bool isActive;

        public bool IsLoaded => scene.isLoaded;

        public SceneData(EditorBuildSettingsScene settingsScene, Scene activeScene)
        {
            this.settingsScene = settingsScene;

            path = settingsScene.path;
            scene = SceneManager.GetSceneByPath(path);
            isActive = scene == activeScene;
            name = Path.GetFileNameWithoutExtension(path);
        }

        public Color GetGUIColor()
        {
            if (IsLoaded && isActive)
                return Color.green;
            else if (IsLoaded)
                return Color.yellow;
            else
                return Color.white;
        }
    }

    private static List<string> helpTooltips = new List<string>()
    {
        "- Left Click - Load single scene and remove all others",
        "- Middle Click - Load or unload the scene additively",
        "- Right Click - Set scene as active, if the scene is not loaded, it will be loaded"
    };

    private readonly Lazy<GUIContent> icon = new Lazy<GUIContent>(() =>
    {
        var content = new GUIContent(EditorGUIUtility.IconContent("_Help"));
        content.tooltip = string.Join("\n", helpTooltips);
        return content;
    });

    [MenuItem("Tools/" + nameof(SceneSwitcher), priority = -50)]
    private static void Open()
    {
        GetWindow<SceneSwitcher>();
    }

    protected void OnGUI()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(icon.Value, GUILayout.Width(20));
        }

        var scenes = EditorBuildSettings.scenes;
        var activeScene = SceneManager.GetActiveScene();
        int count = scenes.Length;
        for (int i = 0; i < count; i++)
        {
            var sceneData = new SceneData(scenes[i], activeScene);
            using (new EditorGUIUtils.GUIColorChanger(sceneData.GetGUIColor()))
            {
                if (GUILayout.Button(sceneData.name))
                    OnScenePressed(sceneData);
            }
        }
    }

    private void OnScenePressed(SceneData sceneData)
    {
        int currentButton = Event.current.button;
        if (currentButton == 0)
        {
            OpenScene(sceneData);
        }
        else if (currentButton == 1)
        {
            SetActiveScene(sceneData);
        }
        else if (currentButton == 2)
        {
            ToggleSceneAsAdditive(sceneData);
        }
    }

    private void OpenScene(SceneData sceneData)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene(sceneData.path);
    }

    private void SetActiveScene(SceneData sceneData)
    {
        Scene scene = !sceneData.IsLoaded ? OpenSceneAsAdditive(sceneData) : sceneData.scene;
        SceneManager.SetActiveScene(scene);
    }

    private Scene OpenSceneAsAdditive(SceneData sceneData)
        => EditorSceneManager.OpenScene(sceneData.path, OpenSceneMode.Additive);

    private void ToggleSceneAsAdditive(SceneData sceneData)
    {
        if (sceneData.IsLoaded)
        {
            if (!sceneData.isActive && EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new[] { sceneData.scene }))
            {
                EditorSceneManager.CloseScene(sceneData.scene, true);
            }
        }
        else
        {
            OpenSceneAsAdditive(sceneData);
        }
    }
}