using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using System.Linq;
using System.Reflection;

namespace AIBehaviourTree.Node
{
	public class NodeView : UnityEditor.Experimental.GraphView.Node
	{
		public Action<NodeView> OnNodeSelected;
		public Node node;
		public List<Port> Inputs { get; } = new List<Port>();
		public List<Port> Outputs { get; } = new List<Port>();

		private Label titleLabel;
		private Label descriptionLabel;
		private Label variableNameLabel;

		private const Orientation portOrientation = Orientation.Horizontal;

		private SerializedObject serializedNode;
		private GroupBox propertyBox;

		public NodeView(Node _node) : base(BehaviourTreeUtility.GetPath("NodeView.uxml"))
		{
			var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(BehaviourTreeUtility.GetPath("NodeView.uss"));
			styleSheets.Add(styleSheet);

			node = _node;
			title = node.name;
			viewDataKey = node.Guid;
			serializedNode = new SerializedObject(node);

			var nodeBorder = this.Q<VisualElement>("node-border");
			if (nodeBorder != null) nodeBorder.tooltip = $"Guid : {node.Guid}";

			node.Initialize();

			if (node is RootNode)
			{
				//capabilities &= ~Capabilities.Movable;
				//capabilities &= ~Capabilities.Deletable;
				capabilities &= ~Capabilities.Copiable;
			}

			style.left = node.Position.x;
			style.top = node.Position.y;

			CreateInputPorts();
			CreateOutputPorts();
			SetupClasses();

			// description label
			titleLabel = this.Q<Label>("title-label");
			if (titleLabel != null)
			{
				titleLabel.bindingPath = "title";
				titleLabel.Bind(serializedNode);
			}

			// description label
			descriptionLabel = this.Q<Label>("description");
			if (descriptionLabel != null)
			{
				descriptionLabel.text = string.Empty;
				descriptionLabel.bindingPath = "description";
				descriptionLabel.Bind(serializedNode);
				descriptionLabel.RegisterValueChangedCallback(OnDescriptionChanged);
			}

			// variable name label
			variableNameLabel = this.Q<Label>("variableName");
			if (variableNameLabel != null)
			{
				variableNameLabel.text = string.Empty;
				variableNameLabel.bindingPath = "variableName";
				variableNameLabel.Bind(serializedNode);
				variableNameLabel.RegisterValueChangedCallback(OnDescriptionChanged);
			}

			// node properties
			propertyBox = this.Q<GroupBox>("properties");
			CreatePropertyBox(propertyBox, serializedNode);
		}

		private void OnDescriptionChanged(ChangeEvent<string> evt)
		{
			descriptionLabel.style.display = (descriptionLabel.text.Trim() == string.Empty)
				? DisplayStyle.None
				: DisplayStyle.Flex;
			variableNameLabel.style.display = (variableNameLabel.text.Trim() == string.Empty)
				? DisplayStyle.None
				: DisplayStyle.Flex;
		}

		private void CreateInputPorts()
		{
			foreach (var input in node.Inputs)
			{
				Port port = InstantiatePort(portOrientation, Direction.Input, input.Capacity, input.Type);
				SetupPort(ref port, input);
				inputContainer.Add(port);
				Inputs.Add(port);
			}
		}

		private void CreateOutputPorts()
		{
			foreach(var output in node.Outputs)
			{
				Port port = InstantiatePort(portOrientation, Direction.Output, output.Capacity, output.Type);
				SetupPort(ref port, output);
				outputContainer.Add(port);
				Outputs.Add(port);
			}
		}

		private void SetupPort(ref Port port, NodePort portData)
		{
			port.name = portData.Name;
			port.portName = portData.DisplayName;
			switch (portData.Type)
			{
				case Type nodeType when nodeType == typeof(Node):
					port.portColor = NodeUtility.ToColor("#feffff");
					break;
				case Type boolType when boolType == typeof(bool):
					port.portColor = NodeUtility.ToColor("#910202");
					break;
				case Type stringType when stringType == typeof(string):
					port.portColor = NodeUtility.ToColor("#fe00d4");
					break;
				case Type intType when intType == typeof(int):
					port.portColor = NodeUtility.ToColor("#22e1af");
					break;
				case Type floatType when floatType == typeof(float):
					port.portColor = NodeUtility.ToColor("#3bd305");
					break;
				case Type vector2Type when vector2Type == typeof(Vector2):
				case Type vector3Type when vector3Type == typeof(Vector3):
					port.portColor = NodeUtility.ToColor("#ffca24");
					break;
				case Type gameObjectType when gameObjectType == typeof(GameObject):
					port.portColor = NodeUtility.ToColor("#10a5e9");
					break;
				case Type transformType when transformType == typeof(Transform):
					port.portColor = NodeUtility.ToColor("#fb7407");
					break;
				case Type quaternionType when quaternionType == typeof(Quaternion):
					port.portColor = NodeUtility.ToColor("#a0b4ff");
					break;
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
			else if (node is VariableNode)
			{
				AddToClassList("node-variable");
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

		private void CreatePropertyBox(GroupBox box, SerializedObject serializedObject)
		{
			box.contentContainer.Clear();

			var iterator = serializedObject.GetIterator();
			int propertyCount = 0;
			if (iterator.NextVisible(true))
			{
				do
				{
					if (iterator.name == "m_Script") continue;

					var label = GetSerializedFieldTitle<NaughtyAttributes.LabelAttribute>(iterator)?.Label ?? iterator.displayName;
					var field = new PropertyField(iterator, label);
					field.Bind(serializedObject);

					box.contentContainer.Add(field);
					propertyCount++;
				} while (iterator.NextVisible(false));
			}

			box.style.display = (propertyCount > 0) ? DisplayStyle.Flex : DisplayStyle.None;
		}

		private T GetSerializedFieldTitle<T>(SerializedProperty serializedProperty) where T : Attribute
		{
			var obj = serializedProperty.serializedObject.targetObject;
			var fieldInfo = obj.GetType().GetField(serializedProperty.name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			return fieldInfo?.GetCustomAttribute<T>(true);
		}
	}
}