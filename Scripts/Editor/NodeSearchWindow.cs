using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System;
using UnityEditor.Experimental.GraphView;
using System.Linq;

namespace AIBehaviourTree.Node
{
	public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
	{
		private BehaviourTreeView treeView;
		private Texture2D indentationIcon; // used to fix the indentation issue

		public void Initialize(BehaviourTreeView treeView)
		{
			this.treeView = treeView;
			indentationIcon = new Texture2D(1, 1);
			indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
			indentationIcon.Apply();
		}

		public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
		{
			var tree = new List<SearchTreeEntry>()
			{
				new SearchTreeGroupEntry(new GUIContent("Create node"), 0),
			};

			{
				tree.Add(new SearchTreeGroupEntry(new GUIContent("Action"), 1));

				var types = TypeCache.GetTypesDerivedFrom<ActionNode>().ToList();
				types.Sort((Type a, Type b) => { return a.Name.CompareTo(b.Name); });
				foreach (var type in types)
				{
					tree.Add(new SearchTreeEntry(new GUIContent(NodeUtility.NicifyTypeName(type), indentationIcon))
					{
						userData = type,
						level = 2,
					});
				}
			}

			{
				tree.Add(new SearchTreeGroupEntry(new GUIContent("Composite"), 1));

				var types = TypeCache.GetTypesDerivedFrom<CompositeNode>().ToList();
				types.Sort((Type a, Type b) => { return a.Name.CompareTo(b.Name); });
				foreach (var type in types)
				{
					tree.Add(new SearchTreeEntry(new GUIContent(NodeUtility.NicifyTypeName(type), indentationIcon))
					{
						userData = type,
						level = 2,
					});
				}
			}

			{
				tree.Add(new SearchTreeGroupEntry(new GUIContent("Decorator"), 1));

				var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>().ToList();
				types.Sort((Type a, Type b) => { return a.Name.CompareTo(b.Name); });
				foreach (var type in types)
				{
					tree.Add(new SearchTreeEntry(new GUIContent(NodeUtility.NicifyTypeName(type), indentationIcon))
					{
						userData = type,
						level = 2,
					});
				}
			}

			return tree;
		}

		public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
		{
			Type type = searchTreeEntry.userData as Type;
			Vector2 position = treeView.contentViewContainer.WorldToLocal(context.screenMousePosition - BehaviourTreeWindow.CurrentPosition);
			treeView.CreateNode(type, position);

			return true;
		}
	}
}