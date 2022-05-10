using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[CreateAssetMenu(menuName = "Isaki/AI Behaviour Tree/Behaviour Tree", order = 0)]
	public class BehaviourTree : ScriptableObject
	{
		[SerializeField, HideInInspector] public Node rootNode;
		[SerializeField, HideInInspector] public Node.State treeState = Node.State.Running;
		[SerializeField, HideInInspector] public List<Node> nodes = new List<Node>();

		[SerializeField, HideInInspector] private Blackboard blackboard;

		public Node.State Update()
		{
			if (rootNode.CurrentState == Node.State.Running)
			{
				treeState = rootNode.Update();
			}
			return treeState;
		}

#if UNITY_EDITOR
		public Node CreateNode(System.Type type, Vector2 position)
		{
			Node node = ScriptableObject.CreateInstance(type) as Node;
			node.name = NodeUtility.NicifyTypeName(type);
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
#endif

		public List<Node> GetChildren(Node parent)
		{
			return parent.Children;
		}

		public BehaviourTree Clone()
		{
			BehaviourTree tree = Instantiate(this);
			tree.rootNode = tree.rootNode.Clone();
			tree.nodes = new List<Node>();
			Traverse(tree.rootNode, (n) =>
			{
				tree.nodes.Add(n);
			});
			return tree;
		}

		public void SetBlackboard(Blackboard _blackboard)
		{
			blackboard = _blackboard;
		}

		public void Traverse(Node node, System.Action<Node> visiter)
		{
			if (node)
			{
				visiter.Invoke(node);
				var children = GetChildren(node);
				children.ForEach(n => Traverse(n, visiter));
			}
		}

		public void Bind()
		{
			Traverse(rootNode, node =>
			{
				node.blackboard = blackboard;
			});
		}
	}
}