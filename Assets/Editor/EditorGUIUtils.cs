using System;
using UnityEngine;

public static class EditorGUIUtils
{
    public struct GUIColorChanger : IDisposable
    {
        private Color previousColor;

        public GUIColorChanger(Color newColor)
        {
            previousColor = GUI.color;
            GUI.color = newColor;
        }

        public void Dispose()
        {
            GUI.color = previousColor;
        }
    }
}
