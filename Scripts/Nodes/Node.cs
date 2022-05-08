using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEditor;

namespace RPG.Node
{
    public abstract class Node : ScriptableObject
    {
        public enum State
        {
            Running,
            Failure,
            Success,
        }

        public State CurrentState { get; protected set; } = State.Running;
        public bool HasStarted { get; protected set; } = false;

        [field: SerializeField] public string Guid { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
        [SerializeField, TextArea] private string description;

        public string Description => description;

        public Blackboard blackboard;

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();

        // Hides the node asset.
        // Sets up the name via type information.
        void OnEnable()
        {
            hideFlags = HideFlags.None;
            name = GetType().Name;

#if UNITY_EDITOR
            name = ObjectNames.NicifyVariableName(name);
#endif

        }

        public State Update()
        {
            if (!HasStarted)
            {
                OnStart();
                HasStarted = true;
            }

            // Debug.Log($"{Blackboard.self.name} : {GetType().Name} (OnUpdate)");
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
    }
}