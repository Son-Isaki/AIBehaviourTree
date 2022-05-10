using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System;
using System.Linq;

namespace AIBehaviourTree.Node
{
	public class BehaviourTreeView : GraphView
	{
		public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }

		public Action<NodeView> OnNodeSelected;
		public BehaviourTree tree;

		private NodeSearchWindow searchWindow;

		public BehaviourTreeView()
		{
			var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(BehaviourTreeWindow.UIBUILDER_PATH + "BehaviourTreeEditor.uss");
			styleSheets.Add(styleSheet);

			GridBackground grid = new GridBackground();
			Insert(0, grid);

			AddManipulator();
			AddSearchWindow();

			Undo.undoRedoPerformed += OnUndoRedo;
		}

		private void AddManipulator()
		{
			this.AddManipulator(new ContentZoomer());
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());
		}

		private void AddSearchWindow()
		{
			searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
			searchWindow.Initialize(this);
			nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
		}

		private void OnUndoRedo()
		{
			PopulateView(tree);
			AssetDatabase.SaveAssets();
		}

		NodeView FindNodeView(Node node)
		{
			return GetNodeByGuid(node.Guid) as NodeView;
		}

		public void PopulateView(BehaviourTree tree)
		{
			this.tree = tree;

			graphViewChanged -= OnGraphViewChanged;
			DeleteElements(graphElements);
			graphViewChanged += OnGraphViewChanged;

			if (tree.rootNode == null)
			{
				tree.rootNode = tree.CreateNode(typeof(RootNode), Vector2.zero) as RootNode;
				EditorUtility.SetDirty(tree);
				AssetDatabase.SaveAssets();
			}

			// create node views
			tree.nodes.ForEach(node => CreateNodeView(node));

			// create edges
			tree.nodes.ForEach(parent => {
				var children = tree.GetChildren(parent);
				children.ForEach(child =>
				{
					NodeView parentView = FindNodeView(parent);
					NodeView childView = FindNodeView(child);

					try
					{
						if (parentView != null && childView != null)
						{
							int inputIndex = 0;
							int outputIndex = parent.Children.IndexOf(child);
							outputIndex = Mathf.Min(outputIndex, parentView.Outputs.Count - 1);

							Edge edge = parentView.Outputs[outputIndex].ConnectTo(childView.Inputs[inputIndex]);
							AddElement(edge);
						}
					}
					catch (Exception e)
					{
						Debug.LogError($"{e}");
					}
				});
			});
		}

		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
		{
			return ports.ToList().Where(endPort => 
				endPort.direction != startPort.direction && 
				endPort.node != startPort.node &&
				endPort.portType == startPort.portType
			).ToList();
		}

		private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
		{
			if (graphViewChange.elementsToRemove != null)
			{
				graphViewChange.elementsToRemove.ForEach((elem) =>
				{
					// node has been removed
					if (elem is NodeView nodeView && nodeView != null)
					{
						tree.DeleteNode(nodeView.node);
					}

					// edge has been removed
					if (elem is Edge edge && edge != null)
					{
						NodeView parentView = edge.output.node as NodeView;
						NodeView childView = edge.input.node as NodeView;
						if (parentView != null && childView != null)
						{
							tree.RemoveChild(parentView.node, childView.node);
						}
					}
				});
			}

			if (graphViewChange.edgesToCreate != null)
			{
				graphViewChange.edgesToCreate.ForEach(edge =>
				{
					NodeView parentView = edge.output.node as NodeView;
					NodeView childView = edge.input.node as NodeView;
					tree.AddChild(parentView.node, childView.node);
					parentView.SortChildren();
				});
			}

			if (graphViewChange.movedElements != null)
			{
				nodes.ForEach((node) =>
				{
					NodeView nodeView = node as NodeView;
					nodeView.SortChildren();
				});
			}

			return graphViewChange;
		}

		public void CreateNode(Type type, Vector2 position)
		{
			Node node = tree.CreateNode(type, position);
			CreateNodeView(node);
		}

		private void CreateNodeView(Node node)
		{
			NodeView nodeView = new NodeView(node);
			nodeView.OnNodeSelected = OnNodeSelected;
			AddElement(nodeView);
		}

		public void UpdateNodeStates()
		{
			nodes.ForEach((node) =>
			{
				NodeView view = node as NodeView;
				view.UpdateState();
			});
		}
	}
}