using System;
using UnityEditor;
using UnityEngine;

namespace RPG.Node
{
	[CustomEditor(typeof(BehaviourTree), true)]
    public class BehaviourTreeEditor : Editor
    {
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUILayout.Space();

			if (GUILayout.Button(new GUIContent("Open Tree Behaviour Window")))
			{
				BehaviourTree tree = target as BehaviourTree;
				BehaviourTreeWindow.OpenWindow();
			}
		}
	}
}