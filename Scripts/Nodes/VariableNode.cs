using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    public abstract class VariableNode : Node
    {
        public abstract object GetValue();

        protected void AddVariableOutput(Type _type)
        {
            AddOutput("value", "Value", _type, UnityEditor.Experimental.GraphView.Port.Capacity.Multi);
		}
	}
}