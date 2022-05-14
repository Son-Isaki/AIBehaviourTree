using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Linq;

namespace AIBehaviourTree.Node
{
    public abstract class Node : ScriptableObject, IDisposable
    {
        public const string N_INPUT = "input";
        public const string N_OUTPUT = "output";

        public enum State
        {
            Running,
            Failure,
            Success,
        }

        public State CurrentState { get; protected set; } = State.Running;
        public bool HasStarted { get; protected set; } = false;
        public BehaviourTree Tree { get; private set; }

        [field: SerializeField, HideInInspector] public string Guid { get; set; }
        [field: SerializeField, HideInInspector] public Vector2 Position { get; set; }
        [SerializeField, HideInInspector] private string title;
        [SerializeField, HideInInspector] private string description;

        [HideInInspector] public GameObject AttachedObject { get; private set; }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State Execute();
        public virtual object GetValue()
		{
            return null;
		}

        [field: SerializeField, HideInInspector] public List<NodePort> Inputs { get; set; } = new List<NodePort>();
        [field: SerializeField, HideInInspector] public List<NodePort> Outputs { get; set; } = new List<NodePort>();
        [field: SerializeField, HideInInspector] public List<Node> Children { get; set; } = new List<Node>();

        private void OnEnable()
        {
            hideFlags = HideFlags.None;
            name = GetName();
        }

        public virtual void Initialize()
		{
            title = GetName();
            description = GetDescription();
            ClearPorts();
		}

        public State Update()
        {
            if (!HasStarted)
            {
                OnStart();
                HasStarted = true;
            }

            CurrentState = Execute();

            if (CurrentState == State.Failure || CurrentState == State.Success)
            {
                OnStop();
                HasStarted = false;
            }

            return CurrentState;
        }

        public Node Clone()
        {
            var node = Instantiate(this);
            node.Children = Children.ConvertAll(c => c.Clone());
            return node;
        }

        protected NodePort AddInput(string _name, string _displayName, Type _type = null, Port.Capacity _capacity = Port.Capacity.Single)
        {
            if (_type == null) _type = typeof(Node);
            var port = new NodePort(_name, _displayName, PortType.Input, _type, _capacity);
            Inputs.Add(port);
            return port;
        }

        protected void RemoveInput(NodePort _input)
        {
            if (Inputs.Contains(_input))
            {
                Inputs.Remove(_input);
            }
        }

        protected void ClearInputs()
		{
            Inputs.Clear();
		}

        protected NodePort AddOutput(string _name, string _displayName, Type _type = null, Port.Capacity _capacity = Port.Capacity.Single)
        {
            if (_type == null) _type = typeof(Node);
            var port = new NodePort(_name, _displayName, PortType.Output, _type, _capacity);
            Outputs.Add(port);
            return port;
        }

        protected NodePort AddValueOutput(Type _type, string _name = "value")
        {
            return AddOutput(_name, NodeUtility.NicifyName(_name), _type, UnityEditor.Experimental.GraphView.Port.Capacity.Multi);
        }

        protected void RemoveOutput(NodePort _output)
        {
            if (Outputs.Contains(_output))
            {
                Outputs.Remove(_output);
            }
        }

        protected void ClearOutputs()
        {
            Outputs.Clear();
        }

        protected void ClearPorts()
		{
            ClearInputs();
            ClearOutputs();
        }

        public virtual string GetName()
        {
            return NodeUtility.NicifyTypeName(GetType());
        }

        public virtual string GetDescription()
        {
            return string.Empty;
        }

        public void SetTree(BehaviourTree tree)
		{
            Tree = tree;
		}

        public void SetAttachedObject(GameObject _attachedObject)
		{
            AttachedObject = _attachedObject;
        }

        public Node GetLinkedNode(NodePort port)
        {
            if (port == null)
			{
                Debug.Log($"port is null");
                return null;
            }

            NodeEdge edge = null;
            if (port.PortType == PortType.Input) 
                edge = Tree.edges.Where(e => e.InputNodeGuid == Guid && e.InputPortName == port.Name).FirstOrDefault();
            else 
                edge = Tree.edges.Where(e => e.OutputNodeGuid == Guid && e.OutputPortName == port.Name).FirstOrDefault();

            if (edge != null)
            {
                return Tree.GetNode(port.PortType == PortType.Input ? edge.OutputNodeGuid : edge.InputNodeGuid);
            }
            return null;
        }

        public List<Node> GetLinkedNodes(NodePort port)
        {
            List<Node> nodes = new List<Node>();

            if (port == null)
            {
                Debug.Log($"port is null");
                return nodes;
            }

            List<NodeEdge> edges = new List<NodeEdge>();
            if (port.PortType == PortType.Input)
                edges = Tree.edges.Where(e => e.InputNodeGuid == Guid && e.InputPortName == port.Name).ToList();
            else
                edges = Tree.edges.Where(e => e.OutputNodeGuid == Guid && e.OutputPortName == port.Name).ToList();

            foreach(var edge in edges)
            {
                using (var node = Tree.GetNode(port.PortType == PortType.Input ? edge.OutputNodeGuid : edge.InputNodeGuid))
                {
                    nodes.Add(node);
                }
            }

            return nodes;
        }

        public T GetValue<T>(NodePort port)
        {
            VariableNode node = GetLinkedNode(port) as VariableNode;
            if (node != null) return (T)node.GetValue();
            return default(T);
        }

        public void Dispose()
        {

        }
	}
}