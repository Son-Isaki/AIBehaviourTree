using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Node
{
	[CreateAssetMenu(menuName = "RPG/AI/Behaviour Tree")]
	public class BehaviourTree : ScriptableObject
	{
		[SerializeField] public Node rootNode;
		[SerializeField] public Node.State treeState = Node.State.Running;
		[SerializeField] public List<Node> nodes = new List<Node>();
		[SerializeField] public BlackboardAI blackboard;

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

			Undo.RecordObject(node, "Behaviour Tree (Create Node)");
			nodes.Add(node);

			if (!Application.isPlaying)
			{
				AssetDatabase.AddObjectToAsset(node, this);
			}
			Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (Create Node)");

			AssetDatabase.SaveAssets();

			return node;
		}

		public void DeleteNode(Node node)
		{
			Undo.RecordObject(node, "Behaviour Tree (Delete Node)");
			nodes.Remove(node);

			Undo.DestroyObjectImmediate(node);

			AssetDatabase.SaveAssets();
		}

		public void AddChild(Node parent, Node child)
		{
			if (parent is RootNode root)
			{
				Undo.RecordObject(root, "Behaviour Tree (Add Child)");
				root.child = child;
				EditorUtility.SetDirty(root);
			}

			if (parent is DecoratorNode decorator)
			{
				Undo.RecordObject(decorator, "Behaviour Tree (Add Child)");
				decorator.child = child;
				EditorUtility.SetDirty(decorator);
			}

			if (parent is CompositeNode composite)
			{
				Undo.RecordObject(composite, "Behaviour Tree (Add Child)");
				composite.children.Add(child);
				EditorUtility.SetDirty(composite);
			}
		}

		public void RemoveChild(Node parent, Node child)
		{
			if (parent is RootNode root)
			{
				Undo.RecordObject(root, "Behaviour Tree (Remove Child)");
				root.child = null;
				EditorUtility.SetDirty(root);
			}

			if (parent is DecoratorNode decorator)
			{
				Undo.RecordObject(decorator, "Behaviour Tree (Remove Child)");
				decorator.child = null;
				EditorUtility.SetDirty(decorator);
			}

			if (parent is CompositeNode composite)
			{
				Undo.RecordObject(composite, "Behaviour Tree (Remove Child)");
				composite.children.Remove(child);
				EditorUtility.SetDirty(composite);
			}
		}
#endif

		public List<Node> GetChildren(Node parent)
		{
			List<Node> children = new List<Node>();

			if (parent is RootNode root && root.child != null)
			{
				children.Add(root.child);
			}

			if (parent is DecoratorNode decorator && decorator.child != null)
			{
				children.Add(decorator.child);
			}

			if (parent is CompositeNode composite)
			{
				children = composite.children;
			}

			return children;
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