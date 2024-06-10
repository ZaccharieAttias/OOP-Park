using UnityEditor;
using UnityEngine;

namespace Assets.PixelFantasy.PixelTileEngine.Scripts.Editor
{
    /// <summary>
    /// Adds "Refresh" button to SpriteCollection script.
    /// </summary>
    [CustomEditor(typeof(SpriteCollectionPF))]
    public class SpriteCollectionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var collection = (SpriteCollectionPF) target;

            if (GUILayout.Button("Refresh"))
            {
                collection.Refresh();
            }
        }
    }
}