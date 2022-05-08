using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace RPG.Node
{
	public class InspectorView : VisualElement
	{
		public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

		Editor editor;

		public InspectorView()
		{
		}

		internal void UpdateSelection(NodeView nodeView)
		{
			Clear();

			UnityEngine.Object.DestroyImmediate(editor);

			if (nodeView == null || nodeView.node == null)
				return;

			editor = Editor.CreateEditor(nodeView.node);
			IMGUIContainer container = new IMGUIContainer(() => {
				if (editor.target)
				{
					editor.OnInspectorGUI();
				}
			});
			Add(container);
		}
	}
}