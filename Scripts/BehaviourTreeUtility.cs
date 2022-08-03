using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace AIBehaviourTree
{
    public static class BehaviourTreeUtility
    {
        public static string GetPath(string filename)
        {
            var filter = $"glob:Plugins/UnityAIBehaviourTree/UIBuilder/{filename}";
            var guid = AssetDatabase.FindAssets(filter).FirstOrDefault();

            if (guid == null)
            {
                Debug.Log($"{filter} => {guid}");
                return null;
            }

            var path = AssetDatabase.GUIDToAssetPath(guid);
            return path;

        }
    }
}