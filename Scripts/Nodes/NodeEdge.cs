using System;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    [Serializable]
    public class NodeEdge
    {
        [field: SerializeField] public string OutputNodeGuid { get; private set; }
        [field: SerializeField] public string OutputPortName { get; private set; }
        [field: SerializeField] public string InputNodeGuid { get; private set; }
        [field: SerializeField] public string InputPortName { get; private set; }

        public NodeEdge(string _outputNodeGuid, string _outputPortName, string _inputNodeGuid, string _inputPortName)
        {
            OutputNodeGuid = _outputNodeGuid;
            OutputPortName = _outputPortName;
            InputNodeGuid = _inputNodeGuid;
            InputPortName = _inputPortName;
        }
    }
}