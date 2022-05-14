using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    [Serializable]
    public class NodePort
    {

        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public string DisplayName { get; set; }
        [field: SerializeField] public PortType PortType { get; set; }
        [field: SerializeField] public Type Type { get; set; }
        [field: SerializeField] public Port.Capacity Capacity { get; set; }

        public NodePort(string _name, string _displayName, PortType _portType, Type _type, Port.Capacity _capacity = Port.Capacity.Single)
        {
            Name = _name;
            DisplayName = _displayName;
            PortType = _portType;
            Type = _type;
            Capacity = _capacity;
        }
    }
    public enum PortType
    {
        Input,
        Output,
    }
}