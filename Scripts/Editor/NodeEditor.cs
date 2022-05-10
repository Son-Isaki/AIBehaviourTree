using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[CustomEditor(typeof(Node), true)]
    public class NodeEditor : Editor
    {
		Node node;
		SerializedProperty titleProp;
		SerializedProperty descriptionProp;

		private void OnEnable()
		{
			node = target as Node;
			try
			{
				titleProp = serializedObject.FindProperty("title");
				descriptionProp = serializedObject.FindProperty("description");
			}
			catch (Exception e)
			{

			}
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (descriptionProp != null)
			{
				serializedObject.Update();
				titleProp.stringValue = node.GetName();
				descriptionProp.stringValue = node.GetDescription();
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}