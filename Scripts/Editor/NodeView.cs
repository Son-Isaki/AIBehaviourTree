using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

namespace RPG.Node
{
	public class NodeView : UnityEditor.Experimental.GraphView.Node
	{
		public Action<NodeView> OnNodeSelected;
		public Node node;
		public Port input;
		public Port output;
		Label descriptionLabel;

		private const Orientation portOrientation = Orientation.Horizontal;

		public NodeView(Node node) : base(BehaviourTreeWindow.BASE_PATH + "NodeView.uxml")
		{
			this.node = node;
			this.title = node.name;
			this.viewDataKey = node.Guid;

			if (node is RootNode)
			{
				capabilities &= ~Capabilities.Movable;
				capabilities &= ~Capabilities.Deletable;
				capabilities &= ~Capabilities.Copiable;
			}

			style.left = node.Position.x;
			style.top = node.Position.y;

			CreateInputPorts();
			CreateOutputPorts();
			SetupClasses();

			descriptionLabel = this.Q<Label>("description");
			if (descriptionLabel != null)
			{
				descriptionLabel.bindingPath = "description";
				descriptionLabel.Bind(new SerializedObject(node));
				descriptionLabel.RegisterValueChangedCallback(OnDescriptionChanged);
			}
		}

		private void OnDescriptionChanged(ChangeEvent<string> evt)
		{
			if (evt.newValue.Trim() == "")
			{
				descriptionLabel.parent.AddToClassList("hide");
			}
			else
			{
				descriptionLabel.parent.RemoveFromClassList("hide");
			}
		}

		private void CreateInputPorts()
		{
			if (node is ActionNode)
			{
				input = InstantiatePort(portOrientation, Direction.Input, Port.Capacity.Single, typeof(float));
			}
			else if (node is CompositeNode)
			{
				input = InstantiatePort(portOrientation, Direction.Input, Port.Capacity.Single, typeof(float));
			}
			else if (node is DecoratorNode)
			{
				input = InstantiatePort(portOrientation, Direction.Input, Port.Capacity.Single, typeof(float));
			}
			else if (node is RootNode)
			{
			}

			if (input != null)
			{
				input.portName = "";
				inputContainer.Add(input);
			}
		}

		private void CreateOutputPorts()
		{
			if (node is ActionNode)
			{
			}
			else if (node is CompositeNode)
			{
				output = InstantiatePort(portOrientation, Direction.Output, Port.Capacity.Multi, typeof(float));
			}
			else if (node is DecoratorNode)
			{
				output = InstantiatePort(portOrientation, Direction.Output, Port.Capacity.Single, typeof(float));
			}
			else if (node is RootNode)
			{
				output = InstantiatePort(portOrientation, Direction.Output, Port.Capacity.Single, typeof(float));
			}

			if (output != null)
			{
				output.portName = "";
				outputContainer.Add(output);
			}
		}

		private void SetupClasses()
		{
			if (node is ActionNode)
			{
				AddToClassList("node-action");
			}
			else if (node is CompositeNode)
			{
				AddToClassList("node-composite");
			}
			else if (node is DecoratorNode)
			{
				AddToClassList("node-decorator");
			}
			else if (node is RootNode)
			{
				AddToClassList("node-root");
			}

			if (output != null)
			{
				output.portName = "";
				outputContainer.Add(output);
			}
		}

		public override void SetPosition(Rect newPos)
		{
			base.SetPosition(newPos);

			Undo.RecordObject(node, "Behaviour Tree (Set Position)");
			node.Position = new Vector2(newPos.x, newPos.y);
			EditorUtility.SetDirty(node);
		}

		public override void OnSelected()
		{
			base.OnSelected();
			OnNodeSelected?.Invoke(this);
		}

		public void SortChildren()
		{
			CompositeNode composite = node as CompositeNode;
			if (composite)
			{
				composite.children.Sort(SortByVerticalPosition);
			}
		}

		private int SortByHorizontalPosition(Node left, Node right)
		{
			return left.Position.x < right.Position.x ? -1 : 1;
		}

		private int SortByVerticalPosition(Node left, Node right)
		{
			return left.Position.y < right.Position.y ? -1 : 1;
		}

		public void UpdateState()
		{
			RemoveFromClassList("running");
			RemoveFromClassList("failure");
			RemoveFromClassList("success");

			if (Application.isPlaying)
			{
				switch (node.CurrentState)
				{
					case Node.State.Running:
						if (node.HasStarted)
							AddToClassList("running");
						break;
					case Node.State.Failure:
						AddToClassList("failure");
						break;
					case Node.State.Success:
						AddToClassList("success");
						break;
				}
			}
		}
	}
}