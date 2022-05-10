using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    [System.Serializable]
    public class NodePort
    {
        [field: SerializeField] public string Name { get; set; }
        public string DisplayName { get; set; }
        public Port.Capacity Capacity { get; set; }

        public NodePort(string _name, string _displayName, Port.Capacity _capacity = Port.Capacity.Single)
        {
            Name = _name;
            DisplayName = _displayName;
            Capacity = _capacity;
        }
    }
}