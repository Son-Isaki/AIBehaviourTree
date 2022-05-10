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
		SerializedProperty descriptionProp;

		private void OnEnable()
		{
			node = target as Node;
			descriptionProp = serializedObject.FindProperty("description");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();
			descriptionProp.stringValue = node.GetDescription();
			serializedObject.ApplyModifiedProperties();
		}
	}
}