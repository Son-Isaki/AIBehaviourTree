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
			var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(BehaviourTreeUtility.GetPath("BehaviourTreeEditor.uss"));
			styleSheets.Add(styleSheet);

			GridBackground grid = new GridBackground();
			Insert(0, grid);

			AddManipulator();
			AddSearchWindow();

			Undo.undoRedoPerformed += OnUndoRedo;
		}

		private void AddManipulator()
		{
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());
			this.AddManipulator(new ContentZoomer());
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

			// create node views
			List<Node> nodesToRemove = new List<Node>();
			tree.nodes.ForEach(n =>
			{
				if (n != null) {
					CreateNodeView(n);
					return;
				}

				// remove broken nodes
				tree.RemoveNode(n);
			});

			// create edges
			List<NodeEdge> edgesToRemove = new List<NodeEdge>();
			tree.edges.ForEach(e =>
			{
				Node outputNode = tree.nodes.Where(n => n.Guid == e.OutputNodeGuid).FirstOrDefault();
				Node inputNode = tree.nodes.Where(n => n.Guid == e.InputNodeGuid).FirstOrDefault();

				if (outputNode != null && inputNode != null)
				{
					NodeView parentView = FindNodeView(outputNode);
					NodeView childView = FindNodeView(inputNode);

					if (parentView != null && childView != null)
					{
						Port outputPort = parentView.Outputs.Where(p => p.name == e.OutputPortName).FirstOrDefault();
						Port inputPort = childView.Inputs.Where(p => p.name == e.InputPortName).FirstOrDefault();

						if (outputPort != null && inputPort != null)
						{
							Edge edge = outputPort.ConnectTo(inputPort);
							AddElement(edge);
							return;
						}
					}
				}
				
				// remove broken edges
				tree.RemoveEdge(e);
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
							// Debug.Log($"Remove edge : {parentView.node.GetName()} ({edge.output.name}) => {childView.node.GetName()} ({edge.input.name})");
							tree.RemoveEdge(parentView.node.Guid, edge.output.name, childView.node.Guid, edge.input.name);
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
					// Debug.Log($"Add edge : {parentView.node.GetName()} ({edge.output.name}) => {childView.node.GetName()} ({edge.input.name})");
					//tree.AddEdge(parentView.node.Guid, edge.output.name, childView.node.Guid, edge.input.name);
					tree.AddEdge(parentView.node, edge.output.name, childView.node, edge.input.name);
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