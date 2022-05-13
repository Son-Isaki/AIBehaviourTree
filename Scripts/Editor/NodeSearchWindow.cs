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

			Dictionary<string, List<Type>> typesList = new Dictionary<string, List<Type>>();

			AddTypesOfType(typeof(RootNode), ref typesList);
			AddTypesOfType(typeof(ActionNode), ref typesList);
			AddTypesOfType(typeof(CompositeNode), ref typesList);
			AddTypesOfType(typeof(DecoratorNode), ref typesList);
			AddTypesOfType(typeof(VariableNode), ref typesList);

			foreach (var types in typesList)
			{
				typesList[types.Key].Sort((Type a, Type b) => { return a.Name.CompareTo(b.Name); });
			}

			foreach (var types in typesList.OrderBy(t => t.Key))
			{
				tree.Add(new SearchTreeGroupEntry(new GUIContent(types.Key), 1));

				foreach (var type in types.Value)
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

		private static void AddTypesOfType(Type type, ref Dictionary<string, List<Type>> typesList)
		{
			foreach (var t in TypeCache.GetTypesDerivedFrom(type))
			{
				if (t.CustomAttributes.ToList().Count > 0)
				{
					foreach (var attribute in t.CustomAttributes.ToList())
					{
						foreach (var argument in attribute.ConstructorArguments.ToList())
						{
							var category = argument.Value.ToString();
							if (!typesList.ContainsKey(category))
							{
								typesList.Add(category, new List<Type>());
							}
							typesList[category].Add(t);
						}
					}
				}
				else
				{
					var category = "Uncategorized";
					if (!typesList.ContainsKey(category))
					{
						typesList.Add(category, new List<Type>());
					}
					typesList[category].Add(t);
				}
			}
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