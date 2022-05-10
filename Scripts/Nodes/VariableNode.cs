using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    public abstract class VariableNode : Node
    {
		[SerializeField] new string name = string.Empty;

        public abstract object GetValue();

        protected void AddVariableOutput(Type _type)
        {
            AddOutput("value", "Value", _type, UnityEditor.Experimental.GraphView.Port.Capacity.Multi);
		}

		public override string GetName()
		{
			if (name.Trim() != string.Empty)
			{
				return name;
			}
			else
			{
				return base.GetName();
			}
		}

		public override string GetDescription()
		{
			return GetValue().ToString();
		}
	}
}