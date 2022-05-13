using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[CreateAssetMenu(menuName = "Isaki/AI Behaviour Tree/Behaviour Tree", order = 0)]
	public class BehaviourTree : ScriptableObject
	{
		[SerializeField, HideInInspector] public Node.State treeState = Node.State.Running;
		[SerializeField, HideInInspector] public List<Node> nodes = new List<Node>();
		[SerializeField, HideInInspector] public List<NodeEdge> edges = new List<NodeEdge>();

		[HideInInspector] public GameObject AttachedObject { get; private set; }

		public StartNode StartNode {
			get {
				if (startNode == null)
				{
					startNode = nodes.Where(n => n.GetType() == typeof(StartNode)).FirstOrDefault() as StartNode;
				}
				return startNode;
			}
		}

		public UpdateNode UpdateNode {
			get {
				if (startNode == null)
				{
					updateNode = nodes.Where(n => n.GetType() == typeof(UpdateNode)).FirstOrDefault() as UpdateNode;
				}
				return updateNode;
			}
		}

		private StartNode startNode;
		private UpdateNode updateNode;

		public void Execute(BehaviourTreeRunner runner, Type nodeType)
		{
			Node node = nodes.Where(n => n.GetType() == nodeType).FirstOrDefault();
			if (node != null)
			{
				Debug.Log($"Start node execution : {node.GetType().Name}");
				runner.StartCoroutine(Job(node));
			}
		}

		private IEnumerator Job(Node node)
		{
			bool process = true;
			while (process)
			{
				treeState = node.Update();
				if (node is StartNode) process = false;
				yield return null;
			}
			yield return null;
		}

		public Node CreateNode(Type type, Vector2 position)
		{
			Node node = CreateInstance(type) as Node;
			node.name = node.GetName();
			node.Guid = GUID.Generate().ToString();
			node.Position = position;

			Undo.RecordObject(node, "Behaviour Tree (Create node)");
			nodes.Add(node);

			if (!Application.isPlaying)
			{
				AssetDatabase.AddObjectToAsset(node, this);
			}
			Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (Create node)");

			AssetDatabase.SaveAssets();

			return node;
		}

		public void DeleteNode(Node node)
		{
			Undo.RecordObject(node, "Behaviour Tree (Delete node)");
			nodes.Remove(node);

			Undo.DestroyObjectImmediate(node);

			AssetDatabase.SaveAssets();
		}

		public void AddChild(Node parent, Node child)
		{
			if (!parent.Children.Contains(child))
			{
				Undo.RecordObject(parent, "Behaviour Tree (Add child)");
				parent.Children.Add(child);
				EditorUtility.SetDirty(parent);
			}
		}

		public void RemoveChild(Node parent, Node child)
		{
			if (parent.Children.Contains(child))
			{
				Undo.RecordObject(parent, "Behaviour Tree (Remove child)");
				parent.Children.Remove(child);
				EditorUtility.SetDirty(parent);
			}
		}

		public void AddEdge(string _outputNodeGuid, string _outputPortName, string _inputNodeGuid, string _inputPortName)
		{
			edges.Add(new NodeEdge(_outputNodeGuid, _outputPortName, _inputNodeGuid, _inputPortName));
		}

		public void RemoveEdge(string _outputNodeGuid, string _outputPortName, string _inputNodeGuid, string _inputPortName)
		{
			if (HasEdge(_outputNodeGuid, _outputPortName, _inputNodeGuid, _inputPortName))
			{
				edges.RemoveAll(e =>
					e.OutputNodeGuid == _outputNodeGuid &&
					e.OutputPortName == _outputPortName &&
					e.InputNodeGuid == _inputNodeGuid &&
					e.InputPortName == _inputPortName
				);
			}
		}

		public bool HasEdge(string _outputNodeGuid, string _outputPortName, string _inputNodeGuid, string _inputPortName)
		{
			return edges.Exists(e =>
				e.OutputNodeGuid == _outputNodeGuid &&
				e.OutputPortName == _outputPortName &&
				e.InputNodeGuid == _inputNodeGuid &&
				e.InputPortName == _inputPortName
			);
		}

		public List<Node> GetChildren(Node parent)
		{
			return parent.Children;
		}

		public BehaviourTree Clone()
		{
			BehaviourTree tree = Instantiate(this);

			if (tree.StartNode != null)
			{
				tree.startNode = tree.StartNode.Clone() as StartNode;
				tree.nodes = new List<Node>();
				Traverse(tree.startNode, (n) =>
				{
					tree.nodes.Add(n);
				});
			}

			if (tree.UpdateNode != null)
			{
				tree.updateNode = tree.UpdateNode.Clone() as UpdateNode;
				tree.nodes = new List<Node>();
				Traverse(tree.updateNode, (n) =>
				{
					tree.nodes.Add(n);
				});
			}

			return this;
		}

		public void SetAttachedObject(GameObject _attachedObject)
		{
			AttachedObject = _attachedObject;
		}

		public void Traverse(Node node, Action<Node> visiter)
		{
			if (node == null) return;

			visiter.Invoke(node);
			var children = GetChildren(node);
			children.ForEach(n => Traverse(n, visiter));
		}

		public void Bind()
		{
			if (StartNode != null)
			{
				Traverse(StartNode, node =>
				{
					node.SetAttachedObject(AttachedObject);
				});
			}
			if (UpdateNode != null)
			{
				Traverse(UpdateNode, node =>
				{
					node.SetAttachedObject(AttachedObject);
				});
			}
		}
	}
}