using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using System.Linq;

namespace AIBehaviourTree.Node
{
	public class NodeView : UnityEditor.Experimental.GraphView.Node
	{
		public Action<NodeView> OnNodeSelected;
		public Node node;
		public List<Port> Inputs { get; } = new List<Port>();
		public List<Port> Outputs { get; } = new List<Port>();
		Label descriptionLabel;

		private const Orientation portOrientation = Orientation.Horizontal;

		public NodeView(Node _node) : base(BehaviourTreeWindow.UIBUILDER_PATH + "NodeView.uxml")
		{
			node = _node;
			title = node.name;
			viewDataKey = node.Guid;

			node.Initialize();

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
			foreach (var inputData in node.Inputs)
			{
				var input = InstantiatePort(portOrientation, Direction.Input, inputData.Capacity, typeof(float));
				input.portName = inputData.DisplayName;
				inputContainer.Add(input);
				Inputs.Add(input);
			}
		}

		private void CreateOutputPorts()
		{
			foreach(var outputData in node.Outputs)
			{
				var output = InstantiatePort(portOrientation, Direction.Output, outputData.Capacity, typeof(float));
				output.portName = outputData.DisplayName;
				outputContainer.Add(output);
				Outputs.Add(output);
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
				composite.Children.Sort(SortByVerticalPosition);
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