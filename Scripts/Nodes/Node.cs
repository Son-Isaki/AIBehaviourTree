using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace AIBehaviourTree.Node
{
    public abstract class Node : ScriptableObject
    {
        public const string DEFAULT_INPUT_NAME = "input";
        public const string DEFAULT_OUTPUT_NAME = "output";

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
        [SerializeField, TextArea] private string description;

        public string Description => description;

        public Blackboard blackboard;

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();

        [field: SerializeField, HideInInspector] public List<NodePort> Inputs { get; set; } = new List<NodePort>();
        [field: SerializeField, HideInInspector] public List<NodePort> Outputs { get; set; } = new List<NodePort>();
        [field: SerializeField, HideInInspector] public List<Node> Children { get; set; } = new List<Node>();

        // Hides the node asset.
        // Sets up the name via type information.
        void OnEnable()
        {
            hideFlags = HideFlags.None;
            name = GetType().Name;

#if UNITY_EDITOR
            name = NodeUtility.NicifyTypeName(GetType());
#endif
        }

        public virtual void Initialize()
		{
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

        public virtual Node Clone()
        {
            return Instantiate(this);
        }

        protected void AddInput(string _name, string _displayName, Port.Capacity _capacity = Port.Capacity.Single)
        {
            Inputs.Add(new NodePort(_name, _displayName, _capacity));
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

        protected void AddOutput(string _name, string _displayName, Port.Capacity _capacity = Port.Capacity.Single)
        {
            Outputs.Add(new NodePort(_name, _displayName, _capacity));
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
    }

    [System.Serializable]
    public class NodePort
    {
        [field: SerializeField] public string Name { get; set; }
        public string DisplayName { get; set; }
        public Port.Capacity Capacity { get; set; }

        public NodePort (string _name, string _displayName, Port.Capacity _capacity = Port.Capacity.Single)
        {
            Name = _name;
            DisplayName = _displayName;
            Capacity = _capacity;
		}
    }
}