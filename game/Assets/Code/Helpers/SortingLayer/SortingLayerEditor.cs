#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using TeamZ.Code.Game.Highlighting;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace TeamZ.Code.Helpers.SortingLayer
{
    // [CustomEditor(typeof(MonoBehaviour), true)]
    public class SortingLayerEditor : Editor
    {
        SerializedProperty[] properties;
        string[] sortingLayerNames;

        void OnEnable()
        {
            var propertyInfo = target
                .GetType()
                .GetFields()
                .Where(o => o.FieldType == typeof(StringContainer<UnityEngine.SortingLayer>))
                .ToArray();

            if (propertyInfo.Any())
            {
                properties = propertyInfo
                    .Select(o => serializedObject.FindProperty(o.Name))
                    .ToArray();

                sortingLayerNames = GetSortingLayerNames();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (properties != null && sortingLayerNames != null)
            {
                foreach (var p in properties)
                {
                    if (p == null)
                    {
                        continue;
                    }

                    int index = Mathf.Max(0, Array.IndexOf(sortingLayerNames, p.stringValue));
                    index = EditorGUILayout.Popup(p.displayName, index, sortingLayerNames);

                    p.stringValue = sortingLayerNames[index];
                }

                if (GUI.changed)
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }

        public string[] GetSortingLayerNames()
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty =
                internalEditorUtilityType.GetProperty("sortingLayerNames",
                    BindingFlags.Static | BindingFlags.NonPublic);
            var sortingLayers = (string[]) sortingLayersProperty.GetValue(null, new object[0]);
            return sortingLayers;
        }
    }
}

#endif