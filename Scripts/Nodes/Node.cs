using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

namespace AIBehaviourTree.Node
{
    public abstract class Node : ScriptableObject
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

        [field: SerializeField, HideInInspector] public string Guid { get; set; }
        [field: SerializeField, HideInInspector] public Vector2 Position { get; set; }
        [SerializeField, HideInInspector] private string title;
        [SerializeField, HideInInspector] private string description;

        [HideInInspector] public GameObject AttachedObject { get; private set; }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();

        [field: SerializeField, HideInInspector] public List<NodePort> Inputs { get; set; } = new List<NodePort>();
        [field: SerializeField, HideInInspector] public List<NodePort> Outputs { get; set; } = new List<NodePort>();
        [field: SerializeField, HideInInspector] public List<Node> Children { get; set; } = new List<Node>();

        private void OnEnable()
        {
            hideFlags = HideFlags.None;
            name = GetType().Name;

#if UNITY_EDITOR
            name = NodeUtility.NicifyTypeName(GetType());
#endif
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

            CurrentState = OnUpdate();

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

        protected void AddInput(string _name, string _displayName, Type _type, Port.Capacity _capacity = Port.Capacity.Single)
        {
            Inputs.Add(new NodePort(_name, _displayName, _type, _capacity));
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

        protected void AddOutput(string _name, string _displayName, Type _type, Port.Capacity _capacity = Port.Capacity.Single)
        {
            Outputs.Add(new NodePort(_name, _displayName, _type, _capacity));
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

        protected Type GetNodePortType()
		{
            return typeof(Node);
		}

        public void SetAttachedObject(GameObject _attachedObject)
		{
            AttachedObject = _attachedObject;
		}
    }
}