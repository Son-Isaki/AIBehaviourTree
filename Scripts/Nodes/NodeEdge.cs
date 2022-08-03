using System;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    [Serializable]
    public class NodeEdge
    {
        [field: SerializeField] public Node OutputNode { get; private set; }
        [field: SerializeField] public string OutputNodeGuid { get; private set; }
        [field: SerializeField] public string OutputPortName { get; private set; }

        [field: SerializeField] public Node InputNode { get; private set; }
        [field: SerializeField] public string InputNodeGuid { get; private set; }
        [field: SerializeField] public string InputPortName { get; private set; }


        public NodeEdge(Node _outputNode, string _outputPortName, Node _inputNode, string _inputPortName)
        {
            OutputNode = _outputNode;
            OutputNodeGuid = _outputNode.Guid;
            OutputPortName = _outputPortName;

            InputNode = _inputNode;
            InputNodeGuid = _inputNode.Guid;
            InputPortName = _inputPortName;
        }
    }
}