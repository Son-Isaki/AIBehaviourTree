using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    public abstract class VariableNode : Node
    {
		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State Execute()
		{
			return State.Success;
		}

		public abstract object GetValue();

		protected NodePort AddValueOutput(Type _type, string _name = "value")
        {
            return AddOutput(_name, NodeUtility.NicifyName(_name), _type, UnityEditor.Experimental.GraphView.Port.Capacity.Multi);
		}
	}
}